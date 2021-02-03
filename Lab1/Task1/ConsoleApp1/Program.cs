using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static string alphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ ,.-!?:;()абвгґдеєжзиіїйклмнопрстуфхцчшщьюя ,.-!?:;()";


        static void CompareWithSizeOfArchive(double amountOfInformation, string path)
        {
            string[] archive = new string[] { 
                ".rar", 
                ".zip", 
                ".gz", 
                ".bz2", 
                ".xz" 
            };
            foreach (string extention in archive)
            {
                FileInfo file = new FileInfo(path + extention);
                Console.WriteLine("{0}: {1}", extention, file.Length);
            }

        }
        static double CountEntropy(double[,] array)
        {
            double entropy = 0;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                if (array[i, 0] != 0)
                    entropy += array[i, 1] * Math.Log(1 / array[i, 1], 2);
            }
            return entropy;
        }
        static string ReadFile(string pathFile)
        {
            string text = "";
            string line;
            if (File.Exists(pathFile))
            {
                using (StreamReader sr = new StreamReader(pathFile, System.Text.Encoding.GetEncoding(1251)))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        text += line + "\n";
                    }
                }
            }
            else
                throw new Exception("NotExists");
            Console.WriteLine(text);
            return text;
        }
        static double GetInfo(double[,] array, string path, int countOfAllLetters)
        {
            double entropy = CountEntropy(array);

            FileInfo file = new FileInfo(path);
            double amountOfInformation;
            Console.WriteLine("Entropy (bits): {0:F4}", entropy);
            Console.WriteLine("Ammount of information (bits): {0:F4}", entropy * countOfAllLetters); // Множимо на к-сть символів тексту, а не алфавіту
            Console.WriteLine("Ammount of information (bytes): {0:F4}\n", amountOfInformation = entropy * countOfAllLetters / 8); // 1байт = 8бит
            Console.WriteLine("Filesize: {0} bytes", file.Length);
            return amountOfInformation;
        }
        static void PrintArray (double[,] array, string alphabet)
        {
            Console.WriteLine("Letter           Number      Frequency");
            for (int i = 0; i < array.GetLength(0); i++)
            {
                Console.Write("{0}         ", alphabet[i]);
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write("{0,15:F4}", array[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        static void CountFrequency(double[,] array, int countOfAllLetters)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                if (array[i, 0] != 0)
                {
                    array[i, 1] = array[i, 0] / countOfAllLetters;
                }
            }
        }
        static void CountLetters(double[,] array, string text, out int countOfAllLetters, string alphabet)
        {
            int counterLetter;
            countOfAllLetters = 0;
            bool firstTime = true;
            for (int i = 0; i < alphabet.Length - alphabet.Length/2; i++)
            {
                counterLetter = 0;
                foreach (char letter in text)
                {
                    if (alphabet.Contains(letter))
                    {
                        if (firstTime)
                            countOfAllLetters++;
                        if (letter == alphabet[i] || letter == alphabet[i + alphabet.Length/2])
                        {
                            counterLetter++;
                        }
                    }
                }
                firstTime = false;
                array[i, 0] = counterLetter;
            }
            Console.WriteLine("\n");
            Console.WriteLine("Number of letters in text: {0}", countOfAllLetters);
        }

        

        static void Main(string[] args)
        {
            
            int countOfAllLetters; //кількість літер у тексті
            double[,] array = new double[(alphabet.Length / 2), 2];

            string path = @"I:\Google Drive\Навчання\Комп'ютерні системи\Lab1\";
            Console.WriteLine("Name of file: ");
            path = path + Console.ReadLine();

            string text = ReadFile(path);
            CountLetters(array, text, out countOfAllLetters, alphabet); // Масив з кількістю окремої літери та частотою її появи у тексті
            CountFrequency(array, countOfAllLetters); // Рахуємо частоту появи літер у тексті
            double amountOfInformation = GetInfo(array, path, countOfAllLetters); // Рахуємо eнтропію та кількість інфомації
           CompareWithSizeOfArchive(amountOfInformation, path); // Порівнюємо розміри архівів
            PrintArray(array, alphabet);

            Console.ReadLine();
        }
    }
}
