using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace RSA_SecureX
{
    internal class GenerateKeys
    {
        // O(log(min(a,b))) Complexity
        // Function to calculate GCD using Euclidean algorithm 
        static BigInteger gcd(BigInteger a, BigInteger b)//O(N log N * log(min(a,b)))
        {
            while (b != new BigInteger("0"))//O(log(min(a,b))) euclidean algorithm
            {
                BigInteger temp = b;//O(1)
                b = BigInteger.Mod(a, b);//O(N log N)  b = a % b
                a = temp;//O(1)
            }

            return a;//O(1)
        }

        // O(phi*log(phi))  msh mota2kd
        public static void GenerateKey(BigInteger p, BigInteger q, out BigInteger n, out BigInteger e, out BigInteger d)//O(log(min(e,phi)) * N^1.585 *phi)
        {
            int startTime = Environment.TickCount;//O(1)
            n = BigInteger.Multiply(p, q);//O(N^1.585)
            BigInteger phi = BigInteger.Multiply(BigInteger.sub(p, new BigInteger("1")), BigInteger.sub(q, new BigInteger("1")));//O(N^1.585)+O(N)+O(N)+O(log N)= O(N^1.585)

            //e = 65537;
            //e = 3; // Start with a small odd integer
            e = new BigInteger("3");//O(N)

            while (e < phi)//O(phi)
            {
                if (gcd(e, phi) == new BigInteger("1"))//O(log(min(e,phi)) * N log N)
                {
                    break;
                }
                e = BigInteger.Add(e, new BigInteger("1"));//O(N)
            }//total while loop O(phi * log(min(e,phi)) * N log N)

            d = new BigInteger("0");//O(N)

            for (BigInteger i = new BigInteger("1"); i < phi; i = BigInteger.Add(i, new BigInteger("1")))// O(phi)
            {

                // e *i mod phi = 1
                if (BigInteger.Mod(BigInteger.Multiply(e, i), phi) == new BigInteger("1"))// O(N log N + N^1.585) = O(N^1.585) per iteration
                {
                    d = i;//O(1)
                    break;
                }
            }//total for loop O(phi * N^1.585)

            int  endTime = Environment.TickCount;//O(1)

            int duration = endTime - startTime;//O(1)

            Console.WriteLine("Execution time for generate key function: " + duration + " ms");//O(1)

        }



        public static BigInteger GenerateLargePrime(int digits)//O(sqrt(N) * N^2)
        {
            Random rand = new Random();
            BigInteger min = new BigInteger((int)Math.Pow(10, digits - 1));//O(log N)
            BigInteger max = new BigInteger((int)Math.Pow(10, digits) - 1);//O(log N)

            BigInteger candidate = new BigInteger();//O(1)

            do
            {
                candidate = rand.NextLong(min, max);//O(N^1.585)
            } while (!IsPrime(candidate));//O(sqrt(N) * N^1.585)

            return candidate;//O(1)
        }

        static bool IsPrime(BigInteger number)//O(sqrt(N) * N^1.585) //checked
        {
            BigInteger two = new BigInteger(2);//O(log N)
            BigInteger three = new BigInteger(3);//O(log N)
            BigInteger zero = new BigInteger(0);//O(log N)

            if (number < two) return false;//O(N)
            if (number == two || number == three) return true;//O(N)
            if (BigInteger.Check(number) == "Even") return false; //O(N ^ 1.585) Even

            BigInteger boundary = BigInteger.Sqrt(number);//O(log(N) * N^1.585)

            BigInteger i = new BigInteger(3);//O(log N)

            for (; i <= boundary; i = BigInteger.Add(i, two))// (sqrt(N) - 1)/2
            {
                if (BigInteger.Mod(number, i) == zero)//O(N log N)
                    return false;//O(1)
            }//total for loop O(sqrt(N) * N log N)

            return true;//O(1)
        }
    }

    static class RandomExtensions
    {
        public static BigInteger NextLong(this Random rand, BigInteger minValue, BigInteger maxValue)//O(N^1.585)
        {
            byte[] buffer = new byte[8];//O(1)
            rand.NextBytes(buffer);//O(N)
            BigInteger longRand = new BigInteger(BitConverter.ToInt32(buffer, 0) & 0x7FFFFFFF);//O(log N) // Ensure positive
            BigInteger one = new BigInteger(1);//O(log N)

            return BigInteger.Add
                (BigInteger.Mod(longRand, BigInteger.Add
                (BigInteger.sub
                (maxValue, minValue), one)), minValue); //O(N)+O(N)+O(N log N)+O(N^1.585)=O(N^1.585) //The Same As return (longRand % (maxValue - minValue + 1)) + minValue;

        }
    }
}

