using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareTheMarketTEST
{
    public static class Helper
    { /// <summary>
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
    }
}
