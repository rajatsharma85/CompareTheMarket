using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace CompareTheMarketTEST
{

    public static class Program
    {
        static void Main(string[] args)
        {
            
            string inputFilePath = ConfigurationManager.AppSettings["inputFile"];
            string outputFilePath = ConfigurationManager.AppSettings["outputFile"];

            Console.WriteLine("Started processing the file....Please wait, this may take while...");
            var wordsInBook = ProcessFileToGetWords(inputFilePath);
            Console.WriteLine("Word count Complete!{0}", Environment.NewLine);

            Console.WriteLine("Determining if the count of the word is prime...");
            //Determine if the word count is prime
            foreach (var word in wordsInBook)
            {
                word.IsPrime = word.IsNumberPrime(word.Count);
            }
            Console.WriteLine("Finished checking if the count of the word is prime!{0}", Environment.NewLine);
            

            Console.WriteLine("Started writing to output csv file...");
            //Write the output to a csv file
            Helper.WriteCSV(wordsInBook, outputFilePath);
            Console.WriteLine("Finished writing to file!{0}", Environment.NewLine);

            Console.WriteLine("Press enter to continue. The output file will be in the \"Data\" folder.");

            Console.Read();
        }

        

        /// <summary>
        /// Processes the input file to read and count the words as you read through
        /// and outputs an object with information about the word and its count in entire text
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <returns></returns>
        public static IEnumerable<WordInfo> ProcessFileToGetWords(string inputFilePath)
        {
            
            var wordsInBook = new List<WordInfo>();

            //Use a stream reader and read line by line and do operations
            //rather than reading all of it into memory at once
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
                            var wordRef = wordsInBook.Where(w => w.Word == sanitizedWord.ToLowerInvariant())
                                .SingleOrDefault();

                            //If the word appeared for the first time, add it to the list object
                            if (wordRef == null)
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
                                wordRef.Count += 1;
                            }
                        }
                    }

                    catch(Exception ex)
                    {
                        Console.WriteLine("An error occured. Message:{0} {1} Stack Trace: {2}", 
                            ex.Message, Environment.NewLine, ex.StackTrace);
                    }
                    

                }
            }

            //Arrange by highest count first
            return wordsInBook.OrderByDescending(x => x.Count);
        }

        

       
    }
}
