using System;
using System.Collections.Generic;

namespace Snake.Cli
{
    public static class Parse
    {
        private static char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static char[] doubleDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' ,',','.'};
        public static int ToInt(string input, int defaultNumber = 0)
        {
            int i;
            if (!int.TryParse(input, out i))
            {
                input = RemoveLetters(input, defaultNumber);
                return int.Parse(input);
            }
            return i;
        }
        public static long ToLong(string input, int defaultNumber = 0)
        {
            long i;
            if (!long.TryParse(input, out i))
            {
                input = RemoveLetters(input, defaultNumber);
                return long.Parse(input);
            }
            return i;
        }
        public static short ToShort(string input, int defaultNumber = 0)
        {
            short i;
            if (!short.TryParse(input, out i))
            {
                input = RemoveLetters(input,defaultNumber);
                return short.Parse(input);
            }
            return i;
        }
        public static byte ToByte(string input, int defaultNumber = 0)
        {
            byte i;
            if (!byte.TryParse(input, out i))
            {
                input = RemoveLetters(input,defaultNumber);
                return byte.Parse(input);
            }
            return i;
        }
        public static double ToDouble(string input, int defaultNumber = 0)
        {
            double i;
            if (!double.TryParse(input, out i))
            {
                input = ToDoubleForm(input, defaultNumber);
                return Convert.ToDouble(input);
            }
            return i;
        }

        private static string ToDoubleForm(string input, int defaultNumber)
        {
            List<int> digitPosition = new List<int>();
            for (int testfor = 0; input.IndexOfAny(doubleDigits, testfor) != -1; testfor = input.IndexOfAny(doubleDigits, testfor) + 1)
            {
                digitPosition.Add(input.IndexOfAny(doubleDigits, testfor));
            }
            string output = "";
            for (int i = 0; i < digitPosition.Count; i++)
            {
                output += input[digitPosition[i]];
            }
            if (output.Equals(""))
            {
                output = defaultNumber.ToString();
            }
            return output;
        }
        private static string RemoveLetters(string input, int defaultNumber)
        {
            List<int> digitPosition = new List<int>();
            for (int testfor = 0; input.IndexOfAny(digits, testfor) != -1; testfor = input.IndexOfAny(digits, testfor) + 1)
            {
                digitPosition.Add(input.IndexOfAny(digits, testfor));
            }
            string output = "";
            for (int i = 0; i < digitPosition.Count; i++)
            {
                output += input[digitPosition[i]];
            }
            if (output.Equals(""))
            {
                output = defaultNumber.ToString();
            }
            return output;
        }
    }
}
