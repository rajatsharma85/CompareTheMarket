using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareTheMarketTEST
{
    public class WordInfo
    {
       
        public string Word { get; set; }
        public int Count { get; set; }
        public bool IsPrime { get; set; }

        /// <summary>
        /// Checks if a number is prime
        /// </summary>
        /// <param name="number"></param>
        /// <returns>True or False</returns>
        public bool IsNumberPrime(int number)
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
