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

        public static BigInteger ExpMod(BigInteger k, BigInteger n, BigInteger m)//total of O(log n * N^1.585)
        {
            BigInteger result = new BigInteger(1);//O(1)
            k = BigInteger.Mod(k, m);//O(N^1.585)
            BigInteger zero = new BigInteger(0);//O(1)
            BigInteger one = new BigInteger(1);//O(log N)

            while (n > zero)//O(log n)
            {
                if ((n & one) == one)//O(N)
                {
                    result = BigInteger.Mod(BigInteger.Multiply(result, k), m);//O(N^1.58)+O(N^1.58) = O(N^1.585)
                }

                n = n >> 1; //O(S* N log N)//divide by 2

                //Pow
                k = BigInteger.Mod(BigInteger.Multiply(k, k), m);//O(N^1.58)+O(N^1.58) = O(N^1.585)
            }//total of O(log n * N^1.585)
            return result;//O(1)
        }


        public static BigInteger encrypt(BigInteger e, BigInteger n, BigInteger message)//O(log n * N^1.585)
        {
            return ExpMod(message, e, n);// O(log n * N^1.585)
        }


        public static BigInteger decrypt(BigInteger d, BigInteger n, BigInteger cipher)// O(log n * N^1.585)
        {
            return ExpMod(cipher, d, n);// O(log n * N^1.585)
        }


        public static void TheCases(string[] lines)//O(L*N) //checked
        {
            testCases.Clear();//O(N)
            int j = 0;//O(1)

            while (j + 3 < lines.Length)//L/4 = O(L)
            {
                TestCase t = new TestCase//O(1)
                {
                    n = new BigInteger(lines[j]),
                    k = new BigInteger(lines[j + 1]),
                    m = new BigInteger(lines[j + 2]),
                    b = (lines[j + 3])
                }; //O(N)
                testCases.Add(t);//O(1)
                j += 4;//O(1)
            }//O(L)*O(N) = O(L*N)
        }

        public static void CryptoTheMassege()
        {
            result.Clear();//O(N)
            time.Clear();//O(N)
            theResults.Clear();//O(N)

            bool fileSelected = false;//O(1)
            while (!fileSelected)
            {
                ManagerFiles.ReadFile();

                if (string.IsNullOrEmpty(ManagerFiles.filePath))
                {
                    Console.WriteLine("No file was selected. Please try again or press Ctrl+C to exit.");
                    continue;
                }

                fileSelected = true;//O(1)
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
                    int endTime = Environment.TickCount;//O(1)
                    time.Add(endTime - startTime);//O(1)
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");//O(1)
                Console.WriteLine("Please try again with a valid file.");//O(1)
            }
        }

        public static List<KeyValuePair<string, BigInteger>> TheOutPut()//O(1)
        {
            return result;//O(1)
        }
        public static List<int> TheTime()//O(1)
        {
            return time;//O(1)
        }
        public static void DisplayAllTestInformation()//O(N)
        {
            int maxCount = Math.Max(Math.Max(testCases.Count, time.Count), theResults.Count);//O(1)

            for (int i = 0; i < maxCount; i++)//O(N)
            {
                Console.WriteLine($"\nTest Case {i + 1}:");//O(1)
                Console.WriteLine("-----------------");//O(1)

                if (i < testCases.Count)//O(1)
                {
                    Console.WriteLine($"n = {testCases[i].n}");//O(1)
                    Console.WriteLine($"k = {testCases[i].k}");//O(1)
                    Console.WriteLine($"m = {testCases[i].m}");//O(1)
                    Console.WriteLine($"s = {testCases[i].b}");//O(1)
                }
                else
                {
                    Console.WriteLine("Test case information not available");//O(1)
                }


                if (i < time.Count)//O(1)
                {
                    Console.WriteLine($"Execution time: {time[i]} ms");//O(1)
                }
                else
                {
                    Console.WriteLine("Execution time not available");//O(1)
                }

                if (i < theResults.Count)//O(1)
                {
                    Console.WriteLine($"Result: {theResults[i]}");//O(1)
                }
                else
                {
                    Console.WriteLine("Result not available");//O(1)
                }

                Console.WriteLine("=================");//O(1)
            }

            if (testCases.Count != time.Count || testCases.Count != theResults.Count || time.Count != theResults.Count)//O(1)
            {
                Console.WriteLine($"\nWarning: Data count mismatch - Test Cases: {testCases.Count}, " +
                                 $"Execution Times: {time.Count}, Results: {theResults.Count}");//O(1)
            }
        }
    }
}
