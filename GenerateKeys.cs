using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_SecureX
{
    internal class GenerateKeys
    {
        // O(log(min(a,b))) Complexity
        // Function to calculate GCD using Euclidean algorithm 
        static BigInteger gcd(BigInteger a, BigInteger b)
        {
            while (b != new BigInteger("0"))
            {
                BigInteger temp = b;
                b = BigInteger.Mod(a, b);
                a = temp;
            }

            return a;
        }

        // O(phi*log(phi))  msh mota2kd
        public static void GenerateKey(BigInteger p, BigInteger q, out BigInteger n, out BigInteger e, out BigInteger d)
        {
            int startTime = Environment.TickCount;

            n = BigInteger.Multiply(p, q);
            BigInteger phi = BigInteger.Multiply(BigInteger.sub(p, new BigInteger("1")), BigInteger.sub(q, new BigInteger("1")));

            // 2. Get end time
            int endTime = Environment.TickCount;

            // 3. Calculate duration
            int duration = endTime - startTime;

            Console.WriteLine(phi);

            Console.WriteLine("Execution time for multiply: " + duration + " ms");


            //e = 65537;
            //e = 3; // Start with a small odd integer
            e = new BigInteger("3");

            startTime = Environment.TickCount;

            while (e < phi)
            {
                if (gcd(e, phi) == new BigInteger("1"))
                {
                    break;
                }
                e = BigInteger.Add(e, new BigInteger("1"));
            }

            // 2. Get end time
            endTime = Environment.TickCount;

            //3.Calculate duration
            duration = endTime - startTime;

            Console.WriteLine("Execution time for gcd while loop: " + duration + " ms");

            //d = 0;
            d = new BigInteger("0");

            startTime = Environment.TickCount;

            for (BigInteger i = new BigInteger("1"); i < phi; i = BigInteger.Add(i, new BigInteger("1")))
            {

                // e *i mod phi = 1
                if (BigInteger.Mod(BigInteger.Multiply(e, i), phi) == new BigInteger("1"))
                {
                    d = i;
                    break;
                }
            }

            // 2. Get end time
            endTime = Environment.TickCount;

            // 3. Calculate duration
            duration = endTime - startTime;

            Console.WriteLine("Execution time for gcd while loop: " + duration + " ms");

        }

        // Prime numbers generate 
        public static BigInteger GenerateLargePrime(int digits)
        {
            Random rand = new Random();
            BigInteger min = new BigInteger((int)Math.Pow(10, digits - 1));
            BigInteger max = new BigInteger((int)Math.Pow(10, digits) - 1);

            BigInteger candidate = new BigInteger();

            do
            {
                candidate = rand.NextLong(min, max);
            } while (!IsPrime(candidate));

            return candidate;
        }

        static bool IsPrime(BigInteger number)
        {
            BigInteger two = new BigInteger(2);
            BigInteger three = new BigInteger(3);
            BigInteger zero = new BigInteger(0);

            if (number < two) return false;
            if (number == two || number == three) return true;
            if (BigInteger.Mod(number, two) == zero) return false;

            BigInteger boundary = BigInteger.Sqrt(number);

            BigInteger i = new BigInteger(3);

            for (; i <= boundary; i = BigInteger.Add(i, two))
            {
                if (BigInteger.Mod(number, i) == zero)
                    return false;
            }

            return true;
        }
    }

    static class RandomExtensions
    {
        public static BigInteger NextLong(this Random rand, BigInteger minValue, BigInteger maxValue)
        {
            byte[] buffer = new byte[8];
            rand.NextBytes(buffer);
            BigInteger longRand = new BigInteger(BitConverter.ToInt32(buffer, 0) & 0x7FFFFFFF); // Ensure positive
            BigInteger one = new BigInteger(1);

            return BigInteger.Add
                (BigInteger.Mod(longRand, BigInteger.Add
                (BigInteger.sub
                (maxValue, minValue), one)), minValue); //The Same As return (longRand % (maxValue - minValue + 1)) + minValue;

        }
    }
}
}
