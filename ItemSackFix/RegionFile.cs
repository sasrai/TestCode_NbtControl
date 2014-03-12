using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RegionFileAccess
{
    public class RegionFile: IDisposable
    {
        string fileName = "";
        MemoryStream regionStream;

        Chunk.ChunkLocationList chunkLocations = new Chunk.ChunkLocationList();
        Chunk.ChunkTimestampList chunkTimestamps = new Chunk.ChunkTimestampList();
        Chunk.ChunkDataList chunkData = new Chunk.ChunkDataList();

        public RegionFile()
        {
        }
        public RegionFile(string filename)
        {
            this.fileName = filename;
        }

        public Chunk.ChunkDataList ChunkData
        {
            get
            {
                return chunkData;
            }
        }

        public bool LoadFile(string filename)
        {
            this.fileName = filename;

            return LoadFile();
        }
        public bool LoadFile()
        {
            if (!File.Exists(fileName))
                return false;

            using (FileStream fs = new FileStream(this.fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fs.Length];

                if (fs.Length != fs.Read(buffer, 0, (int)fs.Length))
                    throw new InvalidDataException("なんか読み込んだファイルが変");
                regionStream = new MemoryStream(buffer);
            }

            regionStream.Seek(0, SeekOrigin.Begin);

            chunkLocations.LoadLocation(regionStream);
            chunkTimestamps.LoadTimestamp(regionStream);
            chunkData.LoadAllChunkData(regionStream, chunkLocations);

            return true;
        }

        protected void UpdateOffset()
        {
            for (int i = 0; i < 1024; i++)
            {
                if (chunkLocations[i].IsCreatedChunk && chunkData[i].UseSectorSize > chunkLocations[i].SectorCount)
                {
                    // 後方のチャンクのオフセット値をずらす
                    for (int j = 0; j < 1024; j++)
                    {
                        if (chunkLocations[j].IsCreatedChunk && chunkLocations[j].Offset > chunkLocations[i].Offset)
                            chunkLocations[j].Offset += chunkData[i].UseSectorSize - chunkLocations[i].SectorCount;
                    }
                }
            }
        }

        public bool SaveFile(string filename)
        {
            UpdateOffset();

            using (FileStream writeStream = new FileStream(filename, FileMode.OpenOrCreate))
            {
                byte[] buffer = chunkLocations.ToByteArray();
                writeStream.Write(buffer, 0, buffer.Length);

                buffer = chunkTimestamps.ToByteArray();
                writeStream.Write(buffer, 0, buffer.Length);

                chunkData.WriteAllChunkData(writeStream, chunkLocations);
            }

            return true;
        }

        void IDisposable.Dispose()
        {
            if (null != regionStream)
            {
                regionStream.Close();
                regionStream.Dispose();
            }
        }
    }
}
