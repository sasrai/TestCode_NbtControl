using System;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace RegionFileAccess.Chunk
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 4)]
    public class ChunkLocation
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        [FieldOffset(0)]protected byte[] chunkData = new byte[4] { 0, 0, 0, 0 };

        public ChunkLocation(byte[] data, int offset)
        {
            if (data.Length < offset + 4)
                throw new ArgumentException("指定された位置に読み込めるデータがありません");
            Buffer.BlockCopy(data, offset, chunkData, 0, 3);
            chunkData[3] = data[offset + 3];

        }
        public ChunkLocation(byte[] data) : this(data, 0) { }

        public int Offset
        {
            get
            {
                return chunkData[2] + (((int)chunkData[1]) << 8) + (((int)chunkData[0]) << 16);
            }
            set
            {
                chunkData[2] = (byte)(value & 0x000000ff);
                chunkData[1] = (byte)(value & 0x0000ff00 >> 8);
                chunkData[0] = (byte)(value & 0x00ff0000 >> 16);
            }
        }
        public byte SectorCount
        {
            get
            {
                return chunkData[3];
            }
            set
            {
                chunkData[3] = value;
            }
        }
        public override string ToString()
        {
            return string.Format("{0}:{1}", Offset, SectorCount);
        }

        public byte[] ToByteArray()
        {
            return chunkData.ToArray();
        }

        public bool IsCreatedChunk
        {
            get
            {
                return (SectorCount > 0);
            }
        }
    }

    public class ChunkLocationList
    {
        protected ChunkLocation[] locations = new ChunkLocation[1024];

        public ChunkLocationList()
        {
        }
        public ChunkLocationList(Stream stream)
        {
            LoadLocation(stream);
        }

        public ChunkLocation this[int i]
        {
            set
            {
                if (i < 0 || i > 1023)
                    throw new IndexOutOfRangeException("0～1023の範囲しかアクセスできません");
                this.locations[i] = value;
            }
            get
            {
                if (i < 0 || i > 1023)
                    throw new IndexOutOfRangeException("0～1023の範囲しかアクセスできません");
                return locations[i];
            }
        }
        public ChunkLocation this[int x, int z]
        {
            set
            {
                if (z < 0 || z > 31 || x < 0 || x > 31)
                    throw new IndexOutOfRangeException("0～31の範囲しかアクセスできません");
                this.locations[z * 32 + x] = value;
            }
            get
            {
                if (z < 0 || z > 31 || x < 0 || x > 31)
                    throw new IndexOutOfRangeException("0～31の範囲しかアクセスできません");
                return locations[z * 32 + x];
            }
        }

        public void LoadLocation(Stream stream)
        {
            byte[] locationBuffer = new byte[4096];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(locationBuffer, 0, locationBuffer.Length);

            for (int i = 0; i < 1024; i++)
            {
                locations[i] = new ChunkLocation(locationBuffer, i * 4);
            }
        }

        public byte[] ToByteArray()
        {
            byte[] locationBuffer = new byte[4096];

            for (int i = 0; i < 1024; i++)
                Buffer.BlockCopy(locations[i].ToByteArray(), 0, locationBuffer, i * 4, 4); 

            return locationBuffer;
        }
    }
}
