using System;
using System.Text;
using StrConvertUtil;

namespace TEACrypt
{
    public class TEA
    {
        private const uint DELTA = 0x9E3779B9;

        private string teakey;
        private uint[] teakeyArr;

        public static string GenerateTeaKey()
        {
            DateTimeOffset now = DateTime.Now;
            //long time = now.ToUnixTimeMilliseconds();
            long time = (long)((now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
            long random = (long)(new Random().NextDouble() * 65536);
            long keyValue = time * random;
            return String.Format("{0:D16}", keyValue);
        }

        public static byte[] Encrypt(uint[] v, uint[] k)
        {
            if (v == null || k == null) return null;

            int n = v.Length;
            if (n == 0) return null;
            if (n <= 1) v[1] = 0;  // algorithm doesn't work for n<2 so fudge by adding a null

            uint q = (uint)(6 + 52 / n);

            n--;
            uint z = v[n], y = v[0];
            uint mx, e, sum = 0;

            while (q-- > 0)
            {  // 6 + 52/n operations gives between 6 & 32 mixes on each word
                sum += DELTA;
                e = sum >> 2 & 3;

                for (int p = 0; p < n; p++)
                {
                    y = v[p + 1];
                    mx = (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
                    z = v[p] += mx;
                }
                y = v[0];
                mx = (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[n & 3 ^ e] ^ z);
                z = v[n] += mx;
            }

            return StrConvert.LongsToStr(v);
        }

        public static string Encrypt(string plainText, string teaKey)
        {
            if (String.IsNullOrEmpty(plainText)) return null;
            if (String.IsNullOrEmpty(teaKey)) return null;

            byte[] x = Encoding.UTF8.GetBytes(plainText);
            uint[] v = StrConvert.StrToLongs(x, 0, 0);
            // simply convert first 16 chars of password as key
            x = Encoding.UTF8.GetBytes(teaKey);
            uint[] k = StrConvert.StrToLongs(x, 0, 16);

            byte[] encryptText = Encrypt(v, k);

            return Convert.ToBase64String(encryptText);
        }

        public static byte[] Decrypt(uint[] v, uint[] k)
        {
            if (v == null || k == null) return null;

            uint n = (uint)v.Length;
            if (n == 0) return null;
            if (n <= 1) v[1] = 0;  // algorithm doesn't work for n<2 so fudge by adding a null

            uint q = (uint)(6 + 52 / n);

            n--;
            uint z = v[n], y = v[0];
            uint mx, e, sum = q * DELTA;
            uint p = 0;

            while (sum != 0)
            {
                e = sum >> 2 & 3;

                for (p = n; p > 0; p--)
                {
                    z = v[p - 1];
                    mx = (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
                    y = v[p] -= mx;
                }

                z = v[n];
                mx = (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
                y = v[0] -= mx;

                sum -= DELTA;
            }

            return StrConvert.LongsToStr(v);
        }

        public static string Decrypt(string cipherText, string teaKey)
        {
            if (String.IsNullOrEmpty(cipherText)) return null;
            if (String.IsNullOrEmpty(teaKey)) return null;

            byte[] x = Convert.FromBase64String(cipherText);
            uint[] v = StrConvert.StrToLongs(x, 0, 0);
            // simply convert first 16 chars of password as key
            x = Encoding.UTF8.GetBytes(teaKey);
            uint[] k = StrConvert.StrToLongs(x, 0, 16);

            byte[] decryptText = Decrypt(v, k);

            return Encoding.UTF8.GetString(decryptText);
        }

        public TEA(string teaKey)
        {
            SetTeaKey(teaKey);
        }

        public string GetTeaKey()
        {
            return teakey;
        }

        public void SetTeaKey(string teaKey)
        {
            this.teakey = teaKey;
            byte[] x = Encoding.UTF8.GetBytes(teaKey);
            this.teakeyArr = StrConvert.StrToLongs(x, 0, 16);
        }

        public string Encrypt(string plainText)
        {
            if (String.IsNullOrEmpty(plainText)) return null;

            byte[] x = Encoding.UTF8.GetBytes(plainText);
            uint[] v = StrConvert.StrToLongs(x, 0, 0);

            return Convert.ToBase64String(Encrypt(v, teakeyArr));
        }

        public string Decrypt(string cipherText)
        {
            if (String.IsNullOrEmpty(cipherText)) return null;

            byte[] x = Convert.FromBase64String(cipherText);
            uint[] v = StrConvert.StrToLongs(x, 0, 0);

            return Encoding.UTF8.GetString(Encrypt(v, teakeyArr));
        }
    }

}
