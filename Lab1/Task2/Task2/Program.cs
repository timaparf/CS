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
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.Unicode;

            string folderpath = @"I:\Google Drive\Навчання\Комп'ютерні системи\Lab1\";
            string textFile;
            char[] encodeText;
            int totalCountCharacters;
            long fileSize;
            double entropy, infoQuant;
            Dictionary<char, double> characters = new Dictionary<char, double>();
            Base64Encrypt base64;


            Console.Write("Enter the name of text file: ");
            textFile = Console.ReadLine();
            Console.WriteLine();


            var path = folderpath + textFile + ".txt";

            Console.WriteLine("Original file:");
            fileSize = GetFileSize(path, characters, out totalCountCharacters);

            CountProbability(characters, totalCountCharacters);
            entropy = Entropy(characters);
            infoQuant = entropy * totalCountCharacters;

            Console.WriteLine("Size of file = {0} bytes", fileSize);
            Console.WriteLine("Info Quantity = {0} bytes", infoQuant / 8);
            Console.WriteLine("Info Quantity = {0} bits\n", infoQuant);


            fileSize = GetFileSize(folderpath + textFile + "_64.txt", characters, out totalCountCharacters);
            CountProbability(characters, totalCountCharacters);
            entropy = Entropy(characters);
            infoQuant = entropy * totalCountCharacters;

            base64 = new Base64Encrypt(GetTextInBytes(path));
            encodeText = base64.GetEncoded();

            string encryptRow = new string(encodeText);
            using (StreamWriter sw = new StreamWriter(folderpath + textFile + "_64.txt", false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(encryptRow);
            }

            Console.WriteLine("Check Encoding: ");
            Console.WriteLine(CheckEncoding(path));
            Console.WriteLine("Encoded File Info:");
            Console.WriteLine("Size of file = {0} bytes", fileSize);
            Console.WriteLine("Info Quantity = {0} bytes", infoQuant / 8);
            Console.WriteLine("Info Quantity = {0} bits\n", infoQuant);
            Console.WriteLine("My Base64 encodeing: ");
            Console.WriteLine(encryptRow);

            Console.WriteLine("Archived File Info:");
            fileSize = GetFileSize(folderpath + textFile + "_64.txt.bz2", characters, out totalCountCharacters);
            CountProbability(characters, totalCountCharacters);
            entropy = Entropy(characters);
            Console.WriteLine("Size of file = {0} bytes", fileSize);
            Console.WriteLine("Info Quantity = {0} bytes", entropy * (totalCountCharacters / 8));
            Console.WriteLine("Info Quantity = {0} bits\n", entropy * totalCountCharacters);

            Console.ReadLine();

        }
        public static void CountProbability(Dictionary<char, double> chrcts, int totalCount)
        {
            int countKeysDict = chrcts.Keys.Count;
            char[] keysDict = new char[countKeysDict];
            chrcts.Keys.CopyTo(keysDict, 0);

            for (int iter = 0; iter < countKeysDict; iter++)
            {
                chrcts[keysDict[iter]] /= totalCount;
            }
        }

        public static double Entropy(Dictionary<char, double> chrcts)
        {
            int countChrct = chrcts.Keys.Count;
            char[] keysDict = new char[countChrct];
            chrcts.Keys.CopyTo(keysDict, 0);
            double probability = 0, entropy = 0;

            for (int iter = 0; iter < countChrct; iter++)
            {
                probability = chrcts[keysDict[iter]];
                entropy -= probability * Math.Log(probability, 2);
            }
            return entropy;
        }



        public static long GetFileSize(string pathFile, Dictionary<char, double> chrcts, out int totalCountChrcts)
        {
            FileInfo fileSize = new FileInfo(pathFile);
            int iterator;
            //Repetition of a character in the text
            double chrctRecur;
            totalCountChrcts = 0;

            string allText = File.ReadAllText(pathFile);
            iterator = 0;
            while (iterator < allText.Length)
            {
                chrctRecur = 1;
                if (!chrcts.ContainsKey(allText[iterator]))
                {
                    chrcts.Add(allText[iterator], chrctRecur);
                }
                else
                    if (chrcts.ContainsKey(allText[iterator]))
                {
                    chrcts[allText[iterator]]++;
                }
                iterator++;
                totalCountChrcts++;
            }
            return fileSize.Length;
        }

        public static byte[] GetTextInBytes(string pathFile)
        {
            Encoding encode = Encoding.UTF8;

            string allText = File.ReadAllText(pathFile);
            char[] chars = allText.ToCharArray(0, allText.Length);
            byte[] bytes = encode.GetBytes(chars);

            return bytes;
        }
        public static string CheckEncoding(string pathFile)
        {
            string allText = File.ReadAllText(pathFile);
            string base64Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(allText));

            return base64Text;
        }



    }
}
