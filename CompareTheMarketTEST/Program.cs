using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CompareTheMarketTEST
{

    public static class Program
    {
        static void Main(string[] args)
        {
            
            string inputFilePath = @"..\..\Data\Book.txt";
            string outputFilePath = @"..\..\Data\output.csv";

            var wordsInBook = ProcessFileToGetWords(inputFilePath);

            //Determine if the word count is prime
            foreach (var word in wordsInBook)
            {
                word.IsPrime = IsNumberPrime(word.Count);
            }

            //Write the output to a csv file
            WriteCSV(wordsInBook, outputFilePath);
            
        }


        /// <summary>
        /// Writes a CSV file from a IEnumerable of any given object of Type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="path"></param>
        public static void WriteCSV<T>(IEnumerable<T> items, string path)
        {
            Type itemType = typeof(T);

            //Use reflection to get properties to print the name in CSV file
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            using (var writer = new StreamWriter(path))
            {
                //Join propertynames using comma
                writer.WriteLine(string.Join(", ", props.Select(p => p.Name)));

                //Join Values using comma
                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(", ", props.Select(p => p.GetValue(item, null))));
                }
            }
        }

        /// <summary>
        /// Processes the input file to read and count the words as you read through
        /// and outputs an object with information about the word and its count in entire text
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <returns></returns>
        public static List<WordInfo> ProcessFileToGetWords(string inputFilePath)
        {
            var wordsInBook = new List<WordInfo>();
            using (FileStream fs = File.Open(inputFilePath, FileMode.Open, 
                FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //Remove after test
                    if(string.IsNullOrEmpty(line))
                    {
                        break;
                    }
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] wordsInLine = line.Split(null);
                        foreach (var word in wordsInLine)
                        {
                            if (string.IsNullOrWhiteSpace(word))
                                continue;

                            var sanitizedWord = SanitizeWord(word);
                            //Check if word already exists in the list in memory so far
                            var x = wordsInBook.Where(w => w.Word == sanitizedWord.ToLowerInvariant())
                                .ToList()
                                .SingleOrDefault();

                            //If the word appeared for the first time
                            if (x == null)
                            {
                                wordsInBook.Add(new WordInfo
                                {
                                    Word = sanitizedWord.ToLowerInvariant(),
                                    Count = 1
                                });
                            }

                            //Increase the count of that word
                            else
                            {
                                x.Count += 1;
                            }
                        }
                    }

                }
            }
            return wordsInBook;
        }

        /// <summary>
        /// Sanitizes a word to remove the punctuation marks
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Sanitized string without any punctuation marks</returns>
        public static string SanitizeWord(string input)
        {
            var sb = new StringBuilder();
            foreach (char c in input)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Checks if a number is prime
        /// </summary>
        /// <param name="number"></param>
        /// <returns>True or False</returns>
        public static bool IsNumberPrime(int number)
        {
            int i;
            for (i = 2; i <= number - 1; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            if (i == number)
            {
                return true;
            }
            return false;
        }
    }
}
