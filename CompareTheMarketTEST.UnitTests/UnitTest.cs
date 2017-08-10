using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompareTheMarketTEST;
using System.Collections.Generic;
using System.IO;

namespace CompareTheMarketTEST.UnitTests
{
    [TestClass]
    public class UnitTest
    {

        [TestMethod]
        public void Check_Whether_Is_Number_Prime()
        {
            //Arrange
            Random rnd = new Random();
            int number = rnd.Next(1, 1001);

            //Act
            var isNumberPrime = new WordInfo().IsNumberPrime(number);

            //Assert
            Assert.IsTrue(isNumberPrime, "The number is not prime");
        }

        [TestMethod]
        public void Test_Word_Sanitization()
        {
            //Arrange
            string word = "This!is.a,testfor;punctuations";

            //Act
            var output = Helper.SanitizeWord(word);

            //Assert
            Assert.AreEqual("Thisisatestforpunctuations", output, "The word is not sanitized properly.");
        }

        
        [TestMethod]
        public void Check_List_Object_Writes_To_CSV()
        {
            string outputPath = @"..\..\outputTestFile" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv";

            //Arrange
            var wordList = new List<WordInfo> {
                new WordInfo { Word = "the", Count = 31, IsPrime = true },
                new WordInfo { Word = "a", Count = 73, IsPrime = true },
                new WordInfo { Word = "for", Count = 6, IsPrime = false }
            };

            //Act
            Helper.WriteCSV(wordList, outputPath);

            //Assert
            Assert.IsTrue(File.Exists(outputPath), "Could not write to csv file");
        }

        [TestMethod]
        public void Process_Smaller_Text_File_Read_All_At_Once()
        {
            //Arrange
            string inputPath = @"..\..\InputTextFile.txt";

            //Act
            var result = Program.ProcessFileToGetWords_ReadAllAtOnce(inputPath);

            //Assert
            Assert.IsNotNull(result, "Could not process the text file.");
        }

        [TestMethod]
        public void Process_Smaller_Text_File_Read_Line_By_Line()
        {
            //Arrange
            string inputPath = @"..\..\InputTextFile.txt";

            //Act
            var result = Program.ProcessFileToGetWords_ReadLineByLine(inputPath);

            //Assert
            Assert.IsNotNull(result, "Could not process the text file.");
        }

        [TestMethod]
        public void Process_Large_Text_File_Read_All_At_Once()
        {
            //Arrange
            string inputPath = @"..\..\InputTextFileLarge.txt";

            //Act
            var result = Program.ProcessFileToGetWords_ReadAllAtOnce(inputPath);

            //Assert
            Assert.IsNotNull(result, "Could not process the text file.");
        }

        [TestMethod]
        public void Process_Large_Text_File_Read_Line_By_Line()
        {
            //Arrange
            string inputPath = @"..\..\InputTextFileLarge.txt";

            //Act
            var result = Program.ProcessFileToGetWords_ReadLineByLine(inputPath);

            //Assert
            Assert.IsNotNull(result, "Could not process the text file.");
        }
    }
}
