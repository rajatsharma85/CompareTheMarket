using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompareTheMarketTEST;

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
            var isNumberPrime = Program.IsNumberPrime(number);

            //Assert
            Assert.IsTrue(isNumberPrime, "The number is not prime");
        }


    }
}
