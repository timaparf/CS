using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    static class Program
    {
        public static byte[] GetTextInBytes(string pathFile)
        {
            Encoding ascii = Encoding.ASCII;

            string allText = File.ReadAllText(pathFile);
            char[] chars = allText.ToCharArray(0, allText.Length);
            byte[] bytes = ascii.GetBytes(chars);

            return bytes;
        }
        static void Main(string[] args)
        {
            string pathFile = @"I:\Google Drive\Навчання\Комп'ютерні системи\Lab1\text1.txt";
            string base64 = "";
            byte[] bytes;

            bytes = File.ReadAllBytes(pathFile);



            base64 = Convert.ToBase64String(bytes);
            Console.WriteLine(base64);
            if (base64 == Convert.ToBase64String(GetTextInBytes(pathFile))) Console.WriteLine("Ura!");
            Console.ReadLine();
        }

    }
}
