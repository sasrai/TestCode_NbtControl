using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RegionFileAccess.Chunk
{
    public class ChunkTimestampList
    {
        protected uint[] timestamps = new uint[1024];

        public ChunkTimestampList()
        {
        }

        public DateTime this[int i]
        {
            set
            {
                if (i < 0 || i > 1023)
                    throw new IndexOutOfRangeException("0～1023の範囲しかアクセスできません");
                var ts = value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                this.timestamps[i] = (uint)ts.TotalSeconds;
            }
            get
            {
                if (i < 0 || i > 1023)
                    throw new IndexOutOfRangeException("0～1023の範囲しかアクセスできません");
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(this.timestamps[i]).ToLocalTime();
            }
        }
        public DateTime this[int x, int z]
        {
            set
            {
                if (z < 0 || z > 31 || x < 0 || x > 31)
                    throw new IndexOutOfRangeException("0～31の範囲しかアクセスできません");
                var ts = value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                this.timestamps[z * 32 + x] = (uint)ts.TotalSeconds;
            }
            get
            {
                if (z < 0 || z > 31 || x < 0 || x > 31)
                    throw new IndexOutOfRangeException("0～31の範囲しかアクセスできません");
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(this.timestamps[z * 32 + x]).ToLocalTime();
            }
        }

        public void LoadTimestamp(Stream stream)
        {
            byte[] timestampBuffer = new byte[4096];

            stream.Seek(4096, SeekOrigin.Begin);
            stream.Read(timestampBuffer, 0, timestampBuffer.Length);

            Buffer.BlockCopy(timestampBuffer, 0, timestamps, 0, timestampBuffer.Length);
        }

        public byte[] ToByteArray()
        {
            byte[] timestampBuffer = new byte[4096];

            Buffer.BlockCopy(timestamps, 0, timestampBuffer, 0, 4096);

            return timestampBuffer;
        }
    }
}