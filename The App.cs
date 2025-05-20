using System;
using System.Collections.Generic;
using System.Linq;
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
                Console.WriteLine("4. Exit");
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
                Cryptosystem.CryptoTheMassege();
                ManagerFiles.ChooseTheWay();
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
                Console.WriteLine(" --- Addition --- ");
                int startTime = Environment.TickCount;
                Console.WriteLine("The result is: " + add(new BigInteger(firstInput), new BigInteger(secondInput)));
                int endTime = Environment.TickCount;
                int duration = endTime - startTime;
                Console.WriteLine("Execution time for addition: " + duration + " ms");
                Console.WriteLine(" --- Subtraction --- ");
                startTime = Environment.TickCount;
                Console.WriteLine("The result is: " + sub(new BigInteger(firstInput), new BigInteger(secondInput)));
                endTime = Environment.TickCount;
                duration = endTime - startTime;
                Console.WriteLine("Execution time for subtraction: " + duration + " ms");
                Console.WriteLine(" --- Multiplication --- ");
                startTime = Environment.TickCount;
                Console.WriteLine("The result is: " + mul(new BigInteger(firstInput), new BigInteger(secondInput)));
                endTime = Environment.TickCount;
                duration = endTime - startTime;
                Console.WriteLine("Execution time for multiplication: " + duration + " ms");
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
            int digits = 3;
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
                back = ret();
            }

        }

    }
}
