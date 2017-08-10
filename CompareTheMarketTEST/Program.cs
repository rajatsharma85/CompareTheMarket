using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CompareTheMarketTEST
{

    public static class Program
    {
        static void Main(string[] args)
        {

            string inputFilePath = ConfigurationManager.AppSettings["inputFile"];
            string outputFilePath = ConfigurationManager.AppSettings["outputFile"];

            Console.WriteLine("Started processing the file....Please wait, this may take while...");

            //var wordsInBook = ProcessFileToGetWords_ReadLineByLine(inputFilePath);

            //In this case with given text, this method is performing better, 
            //so the other method has been commented out.
            var wordsInBook = ProcessFileToGetWords_ReadAllAtOnce(inputFilePath);
            
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
        /// Processes the input file to read and count the words as it reads through line by line
        /// and outputs an object with information about the word and its count in entire text
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <returns>List of object type WordInfo containing Words and count of the words in text</returns>
        public static IEnumerable<WordInfo> ProcessFileToGetWords_ReadLineByLine(string inputFilePath)
        {

            var wordsInBook = new List<WordInfo>();


            Stopwatch s = new Stopwatch();
            s.Start();

            //Use a stream reader and read line by line and do operations
            //rather than reading all of it into memory at once

            //File.Readlines() Initially calls streamreader's readline method

            //For the given example, this method is performing little slower because the file is not 
            //too big. In case of larger files though (as tested in Unit test case with larger file), 
            //this method will perform better over reading all text 
            foreach (string line in File.ReadLines(inputFilePath, Encoding.UTF8))
            {
                //Process next line if current one is blank
                if (string.IsNullOrEmpty(line))
                    continue;

                //Split the line to an array based on white space
                string[] wordsInLine = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    //Iterate through the words in the line
                    foreach (var word in wordsInLine)
                    {
                        
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

                catch (Exception ex)
                {
                    Console.WriteLine("An error occured. Message:{0} {1} Stack Trace: {2}",
                        ex.Message, Environment.NewLine, ex.StackTrace);
                }

            }
           
            //Arrange by highest count first
            return wordsInBook.OrderByDescending(x => x.Count);
        }

        /// <summary>
        /// Processes the input file to read and count the words as it reads complete file till end.
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <returns>List of object type WordInfo containing Words and count of the words in text</returns>
        public static IEnumerable<WordInfo> ProcessFileToGetWords_ReadAllAtOnce(string inputFilePath)
        {
            var wordsInBook = new List<WordInfo>();

            //Read all the text till end. This implements streamreaders ReadToEnd method internally
            //For the given example, this method is performing marginally better as the file is not 
            //too big. However, in case of larger files though  (as tested in Unit test case with larger file), 
            // the method for line by line reading will perform better
            string bookText = File.ReadAllText(inputFilePath, Encoding.UTF8);

            string[] allWords = bookText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in allWords)
            {
                
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

            //Arrange by highest count first
            return wordsInBook.OrderByDescending(x => x.Count);
        }


    }
}
