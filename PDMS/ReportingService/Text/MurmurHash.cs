using System;
using System.Text;

namespace ReportingService.Common.Text
{
    public class MurmurHash
    {
        const uint seed = 0xc58f1a7a;
        const uint m = 0x5bd1e995;
        const int r = 24;

        public static string Hash(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            var bytes = Encoding.UTF8.GetBytes(data);
            return Hash(bytes, seed);
        }

        public static string Hash(byte[] data, uint seed)
        {
            if (data == null || data.Length == 0) return null;

            uint length = (uint)data.Length;
            uint x = seed ^ length;
            int idx = 0;

            while (length >= 4)
            {
                uint k = (uint)(data[idx++] | data[idx++] << 8 | data[idx++] << 16 | data[idx++] << 24);

                k *= m;
                k ^= k >> r;
                k *= m;

                x *= m;
                x ^= k;

                length -= 4;
            }

            switch (length)
            {
                case 3:
                    x ^= (UInt16)(data[idx++] | data[idx++] << 8);
                    x ^= (uint)(data[idx] << 16);
                    x *= m;
                    break;

                case 2:
                    x ^= (UInt16)(data[idx++] | data[idx] << 8);
                    x *= m;
                    break;

                case 1:
                    x ^= data[idx];
                    x *= m;
                    break;
            }

            x ^= x >> 13;
            x *= m;
            x ^= x >> 15;

            return Convert.ToBase64String(BitConverter.GetBytes(x)).Replace("=", "");
        }
    }
}