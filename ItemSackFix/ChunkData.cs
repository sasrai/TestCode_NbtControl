using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.IO.Compression;

namespace RegionFileAccess.Chunk
{
    public enum ChunkCompressionType: byte
    {
        GZip = 1,
        Zlib = 2
    }

    public class ChunkData
    {
        protected byte[] chunkNBTBinary;
        protected byte[] cacheCompressData;
        protected bool doUseCache = false;
#if DEBUG
        protected byte[] originalSectorDump;
#endif

        public ChunkCompressionType CompressionType { get; set; }

        public ChunkData(Stream stream, ChunkLocation location)
        {
            byte[] loadBuffer = new byte[location.SectorCount * 4096];
            stream.Seek(location.Offset * 4096, SeekOrigin.Begin);
            stream.Read(loadBuffer, 0, loadBuffer.Length);

            // Javaのビッグエンディアンで記述されている為必要なら反転
            if (BitConverter.IsLittleEndian)
                Array.Reverse(loadBuffer, 0, 4);
            uint dataLength = BitConverter.ToUInt32(loadBuffer, 0);
            // 直前に反転させた場合念のため元に戻す
            if (BitConverter.IsLittleEndian)
                Array.Reverse(loadBuffer, 0, 4);

            if (1 == loadBuffer[4])
                CompressionType = ChunkCompressionType.GZip;
            else if (2 == loadBuffer[4])
                CompressionType = ChunkCompressionType.Zlib;
            else
                throw new InvalidDataException("Unknown chunk compression type.");

            // length(4byte), 圧縮方式(1byte), RFC1950ヘッダ(2byte:0x789C)をスキップしたデータを渡す
            // lengthは圧縮方式とRFC1950ヘッダも含む為3byte減らす
            chunkNBTBinary = Decompress(loadBuffer, 7, (int)dataLength - 3, CompressionType);

            // キャッシュへ登録
            cacheCompressData = new byte[dataLength - 3];
            Buffer.BlockCopy(loadBuffer, 7, cacheCompressData, 0, cacheCompressData.Length);
            doUseCache = true;

#if DEBUG
            originalSectorDump = loadBuffer;
#endif
        }

        public byte[] Compress(byte[] rawData) { return Compress(rawData, ChunkCompressionType.Zlib); }
        public byte[] Compress(byte[] rawData, ChunkCompressionType compressType) { return Compress(rawData, 0, rawData.Length); }
        public byte[] Compress(byte[] rawData, int offset, int length) { return Compress(rawData, offset, length, ChunkCompressionType.Zlib); }
        public byte[] Compress(byte[] rawData, int offset, int length, ChunkCompressionType compressType)
        {
            if (null == rawData)
                throw new ArgumentNullException("データがぬるぬるしてるよ");

            if (doUseCache)
                return cacheCompressData.ToArray();

            byte[] result;

            using (MemoryStream compressDataStream = new MemoryStream())
            {
                Stream compressStream;
                if (compressType == ChunkCompressionType.GZip)
                    compressStream = new GZipStream(compressDataStream, CompressionMode.Compress);
                else if (compressType == ChunkCompressionType.Zlib)
                    compressStream = new DeflateStream(compressDataStream, CompressionMode.Compress);
                else
                    throw new InvalidDataException("Unknown chunk compression type.");

                compressStream.Write(rawData, offset, length);
                compressStream.Close();

                result = compressDataStream.ToArray();

                cacheCompressData = result.ToArray();
                doUseCache = true;
            }

            return result;
        }

        public byte[] Decompress(byte[] compressData) { return Decompress(compressData, ChunkCompressionType.Zlib); }
        public byte[] Decompress(byte[] compressData, ChunkCompressionType compressType) { return Decompress(compressData, 0, compressData.Length); }
        public byte[] Decompress(byte[] compressData, int offset, int length) { return Decompress(compressData, offset, length, ChunkCompressionType.Zlib); }
        public byte[] Decompress(byte[] compressData, int offset, int length, ChunkCompressionType compressType)
        {
            if (null == compressData)
                throw new ArgumentNullException("データがぬるぬるしてるよ");

            byte[] result;

            using (MemoryStream compressDataStream = new MemoryStream(compressData, offset, length, false))
            {
                Stream decompressStream;
                if (compressType == ChunkCompressionType.GZip)
                    decompressStream = new GZipStream(compressDataStream, CompressionMode.Decompress);
                else if (compressType == ChunkCompressionType.Zlib)
                    decompressStream = new DeflateStream(compressDataStream, CompressionMode.Decompress);
                else
                    throw new InvalidDataException("Unknown chunk compression type.");

                using (MemoryStream rawDataStream = new MemoryStream())
                {

                    byte[] readBuffer = new byte[4096];
                    int readBytes;
                    while (0 != (readBytes = decompressStream.Read(readBuffer, 0, readBuffer.Length)))
                    {
                        rawDataStream.Write(readBuffer, 0, readBytes);
                    }
                    rawDataStream.Close();
                    result = rawDataStream.ToArray();
                }
            }

            return result;
        }

        public byte[] ToByteArray()
        {
            byte[] compressData = Compress(chunkNBTBinary);
            byte[] buffer = new byte[7] { 0, 0, 0, 0, (byte)CompressionType, 0x78, 0x9C };

            Buffer.BlockCopy(BitConverter.GetBytes(compressData.Length + 3), 0, buffer, 0, 4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer, 0, 4);

            // 4096byte単位にバッファ拡張
            Array.Resize(ref buffer, ((int)Math.Ceiling((double)(compressData.Length + 7) / 4096.0f)) * 4096);

            Buffer.BlockCopy(compressData, 0, buffer, 7, compressData.Length);

            return buffer;
        }

        public int UseSectorSize
        {
            get
            {
                return Compress(chunkNBTBinary).Length / 4096 + 1;
            }
        }

        public fNbt.NbtCompound GetRootNBT()
        {
            fNbt.NbtFile nbt = new fNbt.NbtFile();
            nbt.LoadFromStream(new MemoryStream(chunkNBTBinary), fNbt.NbtCompression.None);
            return nbt.RootTag;
        }
        public void SetRootNbt(fNbt.NbtCompound rootNbtCompound)
        {
            // NBTデータを更新した場合にはキャッシュ無効化
            doUseCache = false;
            cacheCompressData = null;

            fNbt.NbtFile nbt = new fNbt.NbtFile();
            nbt.RootTag = rootNbtCompound;

            using (MemoryStream nbtStream = new MemoryStream())
            {
                nbt.SaveToStream(nbtStream, fNbt.NbtCompression.None);
                nbtStream.Close();
                chunkNBTBinary = nbtStream.ToArray();
            }
        }
    }

    public class ChunkDataList : List<ChunkData>
    {
        public ChunkDataList()
        {
            this.Clear();
            for (int i = 0; i < 1024; i++)
                this.Add(null);
        }

        public ChunkData this[int x, int z]
        {
            get
            {
                if (z < 0 || z > 31 || x < 0 || x > 31)
                    throw new IndexOutOfRangeException("0～31の範囲しかアクセスできません");
                return this[x + z * 32];
            }
            set
            {
                if (z < 0 || z > 31 || x < 0 || x > 31)
                    throw new IndexOutOfRangeException("0～31の範囲しかアクセスできません");
                this[x + z * 32] = value;
            }
        }

        public bool IsLoadedChunk(int x, int z)
        {
            return IsLoadedChunk(x + z * 32);
        }
        public bool IsLoadedChunk(int index)
        {
            return (null != this[index]);
        }

        public int LoadAllChunkData(Stream stream) { return LoadAllChunkData(stream, new ChunkLocationList(stream)); }
        public int LoadAllChunkData(Stream stream, ChunkLocationList locations)
        {
            for (int i = 0; i < 1024; i++)
            {
                if (locations[i].SectorCount > 0)
                    this[i] = new ChunkData(stream, locations[i]);
            }
            return 0;
        }

        public void WriteAllChunkData(Stream stream, ChunkLocationList locations)
        {
            for (int i = 0; i < 1024; i++)
            {
                if (!locations[i].IsCreatedChunk)
                    continue;

                stream.Seek(locations[i].Offset * 4096, SeekOrigin.Begin);
                stream.Write(this[i].ToByteArray(), 0, this[i].UseSectorSize * 4096);
            }
        }
    }
}
