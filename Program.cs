using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_SecureX
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            int startTime5 = Environment.TickCount;
            //Cryptosystem.CryptoTheMassege();
            //ManagerFiles.ChooseTheWay();

            string original = "Hello Sir George!";
            int digits = 4;

            //string n1 = "5886655887";
            //string n2 = "147999999999";
            //BigInteger num1 = new BigInteger("17");
            //BigInteger num2 = new BigInteger("5");

            // Test 5: Modulo
            BigInteger p = GenerateKeys.GenerateLargePrime(digits);
            BigInteger q = GenerateKeys.GenerateLargePrime(digits);
            Console.WriteLine("p: " + p);
            Console.WriteLine("q: " + q);

            //BigInteger t = BigInteger.Mod(num1, num2);
            BigInteger e, d, n;
            GenerateKeys.GenerateKey(p, q, out n, out e, out d);

            BigInteger encrypted = Cryptosystem.StringToBigInteger(original);
            string decrypted = Cryptosystem.BigIntegerToString(encrypted);

            Console.WriteLine("Original : " + original);
            Console.WriteLine("Decrypted: " + decrypted);

            int endtim5 = Environment.TickCount;
            Console.WriteLine("Execution time for modulo: " + (endtim5 - startTime5) + " ms");

        }

        //void testBigInteger()
        //{
        //    string n1 = new string('9', 10);
        //    string n2 = new string('3', 9000);
        //    BigInteger num1 = new BigInteger(n1);
        //    BigInteger num2 = new BigInteger(n2);

        //    // Test 1: Addition
        //    int startTime1 = Environment.TickCount;
        //    BigInteger sum = BigInteger.Add(num1, num2);
        //    Console.WriteLine($"{num1} + {num2} = {sum}");
        //    int endtim1 = Environment.TickCount;
        //    Console.WriteLine("Execution time for addition: " + (endtim1 - startTime1) + " ms");

        //    // Test 2: Subtraction
        //    int startTime2 = Environment.TickCount;
        //    BigInteger diff = BigInteger.sub(num2, num1);
        //    Console.WriteLine($"{num2} - {num1} = {diff}");
        //    int endtim2 = Environment.TickCount;
        //    Console.WriteLine("Execution time for subtraction: " + (endtim2 - startTime2) + " ms");

        //    // Test 3: Multiplication
        //    int startTime3 = Environment.TickCount;
        //    BigInteger product = BigInteger.Multiply(num1, num2);
        //    Console.WriteLine($"{num1} * {num2} = {product}");
        //    int endtim3 = Environment.TickCount;
        //    Console.WriteLine("Execution time for multiplication: " + (endtim3 - startTime3) + " ms");

        //    // Test 4: Division
        //    int startTime4 = Environment.TickCount;
        //    BigInteger[] divResult = BigInteger.Div(num2, num1);
        //    Console.WriteLine($"{num2} / {num1} = {divResult[0]} (Quotient), {divResult[1]} (Remainder)");
        //    int endtim4 = Environment.TickCount;
        //    Console.WriteLine("Execution time for division: " + (endtim4 - startTime4) + " ms");

        //    // Test 5: Modulo
        //    int startTime5 = Environment.TickCount;
        //    BigInteger mod = BigInteger.Mod(num2, num1);
        //    Console.WriteLine($"{num2} % {num1} = {mod}");
        //    int endtim5 = Environment.TickCount;
        //    Console.WriteLine("Execution time for modulo: " + (endtim5 - startTime5) + " ms");

        //    // Test 6: Square Root
        //    int startTime6 = Environment.TickCount;
        //    BigInteger num3 = new BigInteger("144");
        //    BigInteger sqrt = BigInteger.Sqrt(num3);
        //    Console.WriteLine($"Sqrt({num3}) = {sqrt}");
        //    int endtim6 = Environment.TickCount;
        //    Console.WriteLine("Execution time for square root: " + (endtim6 - startTime6) + " ms");

        //    // Test 7: Shift left and right
        //    int startTime7 = Environment.TickCount;
        //    BigInteger shiftedLeft = num1 << 2;
        //    BigInteger shiftedRight = num1 >> 2;
        //    Console.WriteLine($"{num1} << 2 = {shiftedLeft}");
        //    Console.WriteLine($"{num1} >> 2 = {shiftedRight}");
        //    int endtim7 = Environment.TickCount;
        //    Console.WriteLine("Execution time for shift operations: " + (endtim7 - startTime7) + " ms");

        //    // Test 8: Comparison
        //    int startTime8 = Environment.TickCount;
        //    Console.WriteLine($"{num1} > {num2} : {num1 > num2}");
        //    Console.WriteLine($"{num1} < {num2} : {num1 < num2}");
        //    Console.WriteLine($"{num1} == {num1} : {num1 == num1}");
        //    int endtim8 = Environment.TickCount;
        //    Console.WriteLine("Execution time for comparison: " + (endtim8 - startTime8) + " ms");
        //}
        //void testCrytptoSysten(string k, string n, string m)
        //{
        //    int startTime1 = Environment.TickCount;
        //    BigInteger k1 = new BigInteger(k);
        //    BigInteger n1 = new BigInteger(n);
        //    BigInteger m1 = new BigInteger(m);
        //    BigInteger enm = Cryptosystem.ExpMod(k1, n1, m1); // Encrypt the message
        //    Console.WriteLine("The message: " + enm);
        //    int endTime1 = Environment.TickCount;
        //    Console.WriteLine("Execution time for crypto system: " + (endTime1 - startTime1) + " ms");
        //}
        //void testfiles()
        //{
        //    int startTime1 = Environment.TickCount;
        //    ManagerFiles.ReadFileFromUser();
        //    ManagerFiles.ReadFileFrompc();
        //    int endTime1 = Environment.TickCount;
        //    Console.WriteLine("Execution time for file: " + (endTime1 - startTime1) + " ms");
        //}

        //void testTheSystem() { }
        //void test() { }

    }
}
