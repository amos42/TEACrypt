/*
TEACrypt.TEA

* Copyright (c) 2018 Amos74
 
* Contact:
  - Gyeongmin Ju{literal <amos74@gmail.com>}

This code refers to the following code.
    1. David Wheeler and Roger Needham, "TEA, a tiny encryption algorithm"
    2. Block Encryption : https://www.movable-type.co.uk/scripts/tea-block.html

MIT License

Copyright (c) 2018 amos74

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

* Contributors:
  - Amos
*/
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
            //long time = now.ToUnixTimeMilliseconds();  // for above .NET Ver 3.6
            long time = (long)((now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
            long random = (long)(new Random().NextDouble() * 65536);
            long keyValue = time * random;
            return String.Format("{0:D16}", keyValue);
        }

        /**
         * sum = 0 
         */
        public uint Encrypt(uint[] v, uint[] k, uint sum)
        {
            uint v0 = v[0], v1 = v[1];
            uint k0 = k[0], k1 = k[1], k2 = k[2], k3 = k[3];
            for (int i = 0; i < 32; i++)
            {
                sum += DELTA;
                v0 += ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
                v1 += ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
            }
            v[0] = v0; v[1] = v1;
            return sum;
        }

        /**
         * sum = 0xC6EF3720 
         */
        public uint Decrypt(uint[] v, uint[] k, uint sum)
        {
            uint v0 = v[0], v1 = v[1];
            uint k0 = k[0], k1 = k[1], k2 = k[2], k3 = k[3];
            for (int i = 0; i < 32; i++)
            {
                v1 -= ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
                v0 -= ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
                sum -= DELTA;
            }
            v[0] = v0; v[1] = v1;
            return sum;
        }

        public static byte[] EncryptBlock(uint[] v, uint[] k)
        {
            if (v == null || k == null) return null;
            if (k.Length < 4) return null;

            int n = v.Length;
            if (n == 0) return null;
            if (n <= 1) return new byte[1] { 0 }; // algorithm doesn't work for n<2 so fudge by adding a null

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

        public static byte[] DecryptBlock(uint[] v, uint[] k)
        {
            if (v == null || k == null) return null;
            if (k.Length < 4) return null;

            uint n = (uint)v.Length;
            if (n == 0) return null;
            if (n <= 1) return new byte[1] { 0 }; // algorithm doesn't work for n<2 so fudge by adding a null

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

        public static string Encrypt(string plainText, string teaKey)
        {
            if (String.IsNullOrEmpty(plainText)) return null;
            if (String.IsNullOrEmpty(teaKey)) return null;
            if (teaKey.Length < 16) return null;

            byte[] x = Encoding.UTF8.GetBytes(plainText);
            uint[] v = StrConvert.StrToLongs(x, 0, 0);
            // simply convert first 16 chars of password as key
            x = Encoding.UTF8.GetBytes(teaKey);
            uint[] k = StrConvert.StrToLongs(x, 0, 16);

            byte[] encryptText = EncryptBlock(v, k);

            return Convert.ToBase64String(encryptText);
        }

        public static string Decrypt(string cipherText, string teaKey)
        {
            if (String.IsNullOrEmpty(cipherText)) return null;
            if (String.IsNullOrEmpty(teaKey)) return null;
            if (teaKey.Length < 16) return null;

            byte[] x = Convert.FromBase64String(cipherText);
            uint[] v = StrConvert.StrToLongs(x, 0, 0);
            // simply convert first 16 chars of password as key
            x = Encoding.UTF8.GetBytes(teaKey);
            uint[] k = StrConvert.StrToLongs(x, 0, 16);

            byte[] decryptText = DecryptBlock(v, k);

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

        public bool SetTeaKey(string teaKey)
        {
            if (String.IsNullOrEmpty(teaKey)) return false;
            if (teaKey.Length < 16) return false;

            this.teakey = teaKey;
            byte[] x = Encoding.UTF8.GetBytes(teaKey);
            this.teakeyArr = StrConvert.StrToLongs(x, 0, 16);

            return true;
        }

        public string Encrypt(string plainText)
        {
            if (String.IsNullOrEmpty(plainText)) return null;

            byte[] x = Encoding.UTF8.GetBytes(plainText);
            uint[] v = StrConvert.StrToLongs(x, 0, 0);

            return Convert.ToBase64String(EncryptBlock(v, teakeyArr));
        }

        public string Decrypt(string cipherText)
        {
            if (String.IsNullOrEmpty(cipherText)) return null;

            byte[] x = Convert.FromBase64String(cipherText);
            uint[] v = StrConvert.StrToLongs(x, 0, 0);

            return Encoding.UTF8.GetString(DecryptBlock(v, teakeyArr));
        }
    }

}
