using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSA_SecureX
{
    internal class The_App
    {
        public static void menu()
        {
        bool exitRequested = false;

            while (!exitRequested)
            {
                Console.Clear();
                Console.WriteLine("=== Main Menu ===");
                Console.WriteLine("1. Crypto System");
                Console.WriteLine("2. Arithmetic Operations");
                Console.WriteLine("3. Generate File & Keys");
                Console.WriteLine("4. Cryptosystem for string");
                Console.WriteLine("5. Exit");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        CryptoSystem();
                        break;
                    case "2":
                        RunArithmeticOperations();
                        break;
                    case "3":
                        GenerateKey();
                        break;
                    case "4":
                        theString();
                        break;
                    case "5":
                        exitRequested = true;
                        Console.WriteLine("Turning off program…");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                        break;
                }

                if (!exitRequested)
                {
                    Console.WriteLine("\nPress any key to return to the main menu...");
                    Console.ReadKey();
                }
            }
        }

        static void CryptoSystem()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("--- Crypto System ---");
                Console.WriteLine();
                Cryptosystem.CryptoTheMassege();
                ManagerFiles.ChooseTheWay();
                Console.WriteLine();
                back = ret();
            }
        }
        static void RunArithmeticOperations()
        {
            Console.Clear();
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("--- Arithmetic Operations ---");
                Console.WriteLine("Enter Two Dig Integer");
                Console.Write("Enter first number: ");
                string firstInput = Console.ReadLine();
                Console.Write("Enter second number: ");
                string secondInput = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine(" --- Addition --- ");
                int startTime = Environment.TickCount;
                Console.WriteLine("The result is: " + add(new BigInteger(firstInput), new BigInteger(secondInput)));
                int endTime = Environment.TickCount;
                int duration = endTime - startTime;
                Console.WriteLine("Execution time for addition: " + duration + " ms");
                Console.WriteLine();
                Console.WriteLine(" --- Subtraction --- ");
                startTime = Environment.TickCount;
                Console.WriteLine("The result is: " + sub(new BigInteger(firstInput), new BigInteger(secondInput)));
                endTime = Environment.TickCount;
                duration = endTime - startTime;
                Console.WriteLine("Execution time for subtraction: " + duration + " ms");
                Console.WriteLine();
                Console.WriteLine(" --- Multiplication --- ");
                startTime = Environment.TickCount;
                Console.WriteLine("The result is: " + mul(new BigInteger(firstInput), new BigInteger(secondInput)));
                endTime = Environment.TickCount;
                duration = endTime - startTime;
                Console.WriteLine("Execution time for multiplication: " + duration + " ms");
                Console.WriteLine();

                back = ret();
            }
        }
        public static BigInteger add(BigInteger a, BigInteger b)
        {
            return BigInteger.Add(a,b);
        }
        public static BigInteger sub(BigInteger a, BigInteger b)
        {
            return BigInteger.sub(a, b);
        }
        public static BigInteger mul(BigInteger a, BigInteger b)
        {
            return BigInteger.Multiply(a, b);
        }
        public static bool ret() {
            Console.WriteLine();
            Console.WriteLine("Press 0 Return to Main Menu and any key to continu in this page:");
            string choice = Console.ReadLine();
            Console.WriteLine();
            if (choice == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static void GenerateKey() {
            int digits = 4;
            bool back = false;
            while (!back)
            {
                BigInteger p = GenerateKeys.GenerateLargePrime(digits);
                BigInteger q = GenerateKeys.GenerateLargePrime(digits);
                BigInteger n, e, d;
                GenerateKeys.GenerateKey(p, q, out n, out e, out d);
                Console.WriteLine("n: " + n);
                Console.WriteLine("e: " + e);
                Console.WriteLine("d: " + d);

                BigInteger message = new BigInteger("1234");

                BigInteger chipher = Cryptosystem.ExpMod(message, e, n);

                BigInteger decrpt = Cryptosystem.ExpMod(chipher, d, n);

                Console.WriteLine(decrpt);
                Console.WriteLine();
                back = ret();
            }

        }

        static void theString()
        {
            Console.Clear();
            bool back = false;
            int digits = 4;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("--- Cryptosystem for String ---");
                Console.WriteLine();
                Console.WriteLine("Enter any thing: ");
                string str= Console.ReadLine();
                BigInteger bi = string_op.StringToBigInteger(str);
                Console.WriteLine("the big integer: " + bi);
                string s = string_op.BigIntegerToString(bi);
                Console.WriteLine("the string" + s);
                Console.WriteLine();

                back = ret();
            }
        }

    }
}
