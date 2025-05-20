using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_SecureX
{
    internal class Cryptosystem
    {
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

        public static BigInteger ExpMod(BigInteger messs, BigInteger key, BigInteger n)//total of O(log n * N^1.585)
        {
            BigInteger result = new BigInteger(1);//O(log N)
            messs = BigInteger.Mod(messs, n);//O(N log N)   
            BigInteger zero = new BigInteger(0);//O(1)
            BigInteger one = new BigInteger(1);//O(log N)

            while (key > zero)//O(log n)
            {
                if ((key & one) == one)//O(N)
                {
                    result = BigInteger.Mod(BigInteger.Multiply(result, messs), n);//O(N log N)+O(N^1.58) = O(N^1.585)    
                }

                key = key >> 1; //O(S* N log N)//divide by 2

                //Pow
                messs = BigInteger.Mod(BigInteger.Multiply(messs, messs), n);//O(N log N)+O(N^1.58) = O(N^1.585)   k^2 mod m
            }//total of O(log n * N^1.585)
            return result;//O(1)
        }


        public static BigInteger encrypt(BigInteger e, BigInteger n, BigInteger message)//O(log n * N^1.585)  M^e mod n
        {
            return ExpMod(message, e, n);// O(log n * N^1.585)
        }


        public static BigInteger decrypt(BigInteger d, BigInteger n, BigInteger cipher)// O(log n * N^1.585) C^d mod n
        {
            return ExpMod(cipher, d, n);// O(log n * N^1.585)
        }


        public static void TheCases(string[] lines)//O(L*N) //checked
        {
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

        public static void CryptoTheMassege() // O(N*L)
        {
            Console.Clear();
            ManagerFiles.ReadFile(); // O(N*L)
            result.Clear();//O(N)
            time.Clear();//O(N)
            try
            {
                int theNOTC = ManagerFiles.ReadTheFirstLine();//O(N)
                string[] lines = ManagerFiles.ReadTheLines();//O(N)

                TheCases(lines); // o (L*N) //checked
                lines = null;//O(1)
                for (int i = 0; i < theNOTC; i++)
                {
                    int startTime = Environment.TickCount;
                    BigInteger n = testCases[i].n;
                    BigInteger k = testCases[i].k;
                    BigInteger m = testCases[i].m;
                    string b = testCases[i].b;
                    if (b == "1")
                    {
                        result.Add(new KeyValuePair<string, BigInteger>("The Decrypted Massege ", decrypt(k, n, m)));// O(log n * N^1.585)
                    }
                    else
                    {
                        result.Add(new KeyValuePair<string, BigInteger>("The Encrypted Massege", encrypt(k, n, m)));// O(log n * N^1.585)
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
            Console.WriteLine("The crypto system succuss ");//O(1)
        }

        public static List<KeyValuePair<string, BigInteger>> TheOutPut()//O(1)
        {
            return result;//O(1)
        }
        public static List<int> TheTime()//O(1)
        {
            return time;//O(1)
        }
    }
}
