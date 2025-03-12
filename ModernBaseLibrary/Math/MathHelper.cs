namespace ModernBaseLibrary.Math
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class MathHelper
    {
        /// <summary>
        /// Prüft, ob die übergebene Zahl eine Primezahl ist
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPrime(int number)
        {

            if (number == 1)
            {
                return false;
            }

            // 
            if (number == 2)
            {
                return true;
            }

            // If the number is even, it is not prime
            if (number % 2 == 0)
            {
                return false;
            }

            // Check every odd number till the sqrt of the number
            for (int i = 3; i * i <= number; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get the prime factors of a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static List<int> PrimeFactors(int number)
        {
            List<int> result = new List<int>();
            int n = number;

            for (int i = 2; i <= n; i++)
            {
                while (n % i == 0)
                {
                    result.Add(i);
                    n /= i;
                }
            }

            return result;
        }

        /// <summary>
        /// Checks if a number is a square by taking the root of it and checking whether it's an int
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsSquare(int number)
        {

            double root = System.Math.Sqrt(number);

            return root % 1 == 0.0;
        }

        /// <summary>
        /// Creates a rectangle which has an area which equals the given count. 
        /// This rectangle is trying to be as quadratic as possible. 
        /// Doesn't work for prime numbers since they only have their 2 divisors obviously.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Tuple<int, int> EqualSquare(int count)
        {
            if (count == 0)
            {
                return new Tuple<int, int>(0, 0);
            }

            var parts = new List<Tuple<int, int>>();

            // Get all division pairs
            for (int i = 1; i <= System.Math.Ceiling(count / 2.0); i++)
            {

                if ((double)count / i % 1 == 0)
                {
                    parts.Add(new Tuple<int, int>(i, count / i));
                }
            }


            // Get pair with smallest distance
            var result = parts[0];
            foreach (Tuple<int, int> tup in parts)
            {
                if (System.Math.Abs(tup.Item1 - tup.Item2) < System.Math.Abs(result.Item1 - result.Item2))
                {
                    result = tup;
                }
            }

            return result;
        }
    }
}
