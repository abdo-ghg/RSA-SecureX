using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_SecureX
{
    internal class Cryptosystem
    {
        private static List<BigInteger> theResults = new List<BigInteger>(0);
        private static List<KeyValuePair<string, BigInteger>> result = new List<KeyValuePair<string, BigInteger>>();
        private static List<int> time = new List<int>();
        public struct TestCase
        {
            public BigInteger n;
            public BigInteger k;
            public BigInteger m;
            public string b;
        }
        private static List<TestCase> testCases = new List<TestCase>();

        public static BigInteger ExpMod(BigInteger k, BigInteger n, BigInteger m)
        {
            BigInteger result = new BigInteger(1);
            k = BigInteger.Mod(k, m);
            BigInteger zero = new BigInteger(0);
            BigInteger one = new BigInteger(1);

            while (n > zero)
            {
                if ((n & one) == one)
                {
                    result = BigInteger.Mod(BigInteger.Multiply(result, k), m);
                }

                n = n >> 1; //divide by 2

                //Pow
                k = BigInteger.Mod(BigInteger.Multiply(k, k), m);
            }
            return result;
        }


        public static BigInteger encrypt(BigInteger e, BigInteger n, BigInteger message)
        {
            return ExpMod(message, e, n);
        }


        public static BigInteger decrypt(BigInteger d, BigInteger n, BigInteger cipher)
        {
            return ExpMod(cipher, d, n);
        }


        public static void TheCases(string[] lines)
        {
            testCases.Clear();
            int j = 0;

            while (j + 3 < lines.Length)
            {
                TestCase t = new TestCase
                {
                    n = new BigInteger(lines[j]),
                    k = new BigInteger(lines[j + 1]),
                    m = new BigInteger(lines[j + 2]),
                    b = (lines[j + 3])
                };
                testCases.Add(t);
                j += 4;
            }
        }

        public static void CryptoTheMassege()
        {
            result.Clear();
            time.Clear();
            theResults.Clear();

            bool fileSelected = false;
            while (!fileSelected)
            {
                ManagerFiles.ReadFile();

                if (string.IsNullOrEmpty(ManagerFiles.filePath))
                {
                    Console.WriteLine("No file was selected. Please try again or press Ctrl+C to exit.");
                    continue;
                }

                fileSelected = true;
            }

            try
            {
                int theNOTC = ManagerFiles.ReadTheFirstLine();
                string[] lines = ManagerFiles.ReadTheLines();

                TheCases(lines);
                for (int i = 0; i < theNOTC; i++)
                {
                    int startTime = Environment.TickCount;
                    BigInteger n = testCases[i].n;
                    BigInteger k = testCases[i].k;
                    BigInteger m = testCases[i].m;
                    string b = testCases[i].b;
                    if (b == "1")
                    {
                        result.Add(new KeyValuePair<string, BigInteger>("The Decrypted Massege ", decrypt(k, n, m)));
                        theResults.Add(decrypt(k, n, m));
                    }
                    else
                    {
                        result.Add(new KeyValuePair<string, BigInteger>("The Encrypted Massege", encrypt(k, n, m)));
                        theResults.Add(encrypt(k, n, m));
                    }
                    int endTime = Environment.TickCount;
                    time.Add(endTime - startTime);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
                Console.WriteLine("Please try again with a valid file.");
            }
        }

        public static List<KeyValuePair<string, BigInteger>> TheOutPut()
        {
            return result;
        }
        public static List<int> TheTime()
        {
            return time;
        }
        public static void DisplayAllTestInformation()
        {
            int maxCount = Math.Max(Math.Max(testCases.Count, time.Count), theResults.Count);

            for (int i = 0; i < maxCount; i++)
            {
                Console.WriteLine($"\nTest Case {i + 1}:");
                Console.WriteLine("-----------------");

                if (i < testCases.Count)
                {
                    Console.WriteLine($"n = {testCases[i].n}");
                    Console.WriteLine($"k = {testCases[i].k}");
                    Console.WriteLine($"m = {testCases[i].m}");
                    Console.WriteLine($"s = {testCases[i].b}");
                }
                else
                {
                    Console.WriteLine("Test case information not available");
                }


                if (i < time.Count)
                {
                    Console.WriteLine($"Execution time: {time[i]} ms");
                }
                else
                {
                    Console.WriteLine("Execution time not available");
                }

                if (i < theResults.Count)
                {
                    Console.WriteLine($"Result: {theResults[i]}");
                }
                else
                {
                    Console.WriteLine("Result not available");
                }

                Console.WriteLine("=================");
            }

            if (testCases.Count != time.Count || testCases.Count != theResults.Count || time.Count != theResults.Count)
            {
                Console.WriteLine($"\nWarning: Data count mismatch - Test Cases: {testCases.Count}, " +
                                 $"Execution Times: {time.Count}, Results: {theResults.Count}");
            }
        }
    }
}
