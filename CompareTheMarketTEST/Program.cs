using System;
using System.Collections.Generic;
using System.Configuration;
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

            string inputFilePath = ConfigurationManager.AppSettings["inputFile"];
            string outputFilePath = ConfigurationManager.AppSettings["outputFile"];

            var wordsInBook = ProcessFileToGetWords(inputFilePath);

            //Determine if the word count is prime
            foreach (var word in wordsInBook)
            {
                word.IsPrime = Helper.IsNumberPrime(word.Count);
            }

            //Write the output to a csv file
            Helper.WriteCSV(wordsInBook, outputFilePath);
            
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
                    //Process next line if current one is blank
                    if (string.IsNullOrEmpty(line))
                        continue;
                    
                       //Split the line to an array based on white space
                        string[] wordsInLine = line.Split(null);

                    try
                    {
                        //Iterate through the words in the line
                        foreach (var word in wordsInLine)
                        {
                            //Ignore blank words
                            if (string.IsNullOrWhiteSpace(word))
                                continue;

                            //Remove punctuations
                            var sanitizedWord = Helper.SanitizeWord(word);

                            //Check if a word already exists in the list in memory so far
                            var x = wordsInBook.Where(w => w.Word == sanitizedWord.ToLowerInvariant())
                                .ToList()
                                .SingleOrDefault();

                            //If the word appeared for the first time, add it to the list object
                            if (x == null)
                            {
                                wordsInBook.Add(new WordInfo
                                {
                                    Word = sanitizedWord.ToLowerInvariant(),
                                    Count = 1
                                });
                            }

                            //If word already exists, just increase the count of that word
                            else
                            {
                                x.Count += 1;
                            }
                        }
                    }

                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message, ex.StackTrace);
                    }
                    

                }
            }
            return wordsInBook;
        }

        

       
    }
}
