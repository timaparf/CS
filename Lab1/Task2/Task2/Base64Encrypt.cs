using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    class Base64Encrypt
    {
        byte[] inp;
        int lengthText, lengthEncrypt;
        int blockCount;
        int paddingCount;

        public Base64Encrypt(byte[] text)
        {
            inp = text;
            lengthText = text.Length;

            if (lengthText % 3 == 0)
            {
                paddingCount = 0;
                blockCount = lengthText / 3;
            }
            else
            {
                paddingCount = 3 - (lengthText % 3);
                blockCount = (lengthText + paddingCount) / 3;
            }
            lengthEncrypt = lengthText + paddingCount;
        }

        public char[] GetEncoded()
        {
            byte b1, b2, b3;
            byte t, t1, t2, t3, t4;
            byte[] b = new byte[blockCount * 4];
            byte[] inp2;
            char[] res = new char[blockCount * 4];
            inp2 = new byte[lengthEncrypt];

            for (int i = 0; i < lengthEncrypt; i++)
            {
                if (i < lengthText)
                {
                    inp2[i] = inp[i];
                }
                else
                {
                    inp2[i] = 0;
                }
            }

            for (int i = 0; i < blockCount; i++)
            {
                b1 = inp2[i * 3];
                b2 = inp2[i * 3 + 1];
                b3 = inp2[i * 3 + 2];

                t1 = (byte)((b1 & 252) >> 2);

                t = (byte)((b1 & 3) << 4);
                t2 = (byte)((b2 & 240) >> 4);
                t2 += t;

                t = (byte)((b2 & 15) << 2);
                t3 = (byte)((b3 & 192) >> 6);
                t3 += t;

                t4 = (byte)(b3 & 63);

                b[i * 4] = t1;
                b[i * 4 + 1] = t2;
                b[i * 4 + 2] = t3;
                b[i * 4 + 3] = t4;
            }

            for (int i = 0; i < blockCount * 4; i++)
            {
                res[i] = SymbolsTable(b[i]);
            }

            switch (paddingCount)
            {
                case 0:
                    break;
                case 1:
                    res[blockCount * 4 - 1] = '=';
                    break;
                case 2:
                    res[blockCount * 4 - 1] = '=';
                    res[blockCount * 4 - 2] = '=';
                    break;
                default:
                    break;
            }
            return res;
        }

        private char SymbolsTable(byte num)
        {
            char[] symbolsTable = new char[64]
            {   'A','B','C','D','E','F','G','H','I','J','K','L','M',
                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                '0','1','2','3','4','5','6','7','8','9','+','/'
            };

            if ((num >= 0) && (num <= 63))
            {
                return symbolsTable[num];
            }
            else
            {
                return ' ';
            }
        }
    }
}
