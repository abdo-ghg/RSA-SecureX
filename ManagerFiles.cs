using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RSA_SecureX
{
    internal class ManagerFiles
    {
        public static string filePath;
        private static List<string> lines = new List<string>();
        private static List<KeyValuePair<string, BigInteger>> result = new List<KeyValuePair<string, BigInteger>>();
        private static List<int> time = new List<int>();

        // read the file from the pc 

        // take the file path from the user
        public static bool ReadFileFromUser()
        {
            Console.Write("Enter file path: ");
            string userFilePath = Console.ReadLine()?.Trim();

            try
            {
                if (string.IsNullOrEmpty(userFilePath))
                {
                    Console.WriteLine("No path entered.");
                    return false;
                }

                if (!File.Exists(userFilePath))
                {
                    Console.WriteLine("File not found. Try again.");
                    return false;
                }

                filePath = userFilePath;
                Console.WriteLine("File found.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        // read the file
        public static void ReadFile()
        {
            int choice;//O(1)
            do
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1 - Enter file path manually");
                Console.WriteLine("2 - Exit");
                Console.Write("Your choice: ");

                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        if (ReadFileFromUser())
                            break;
                        break;
                    case 2:
                        return;
                        break;
                    default:
                        Console.WriteLine("Invalid option! Try again.");
                        break;
                }
            } while (choice > 2 ||choice > 0);
        }

        // read the line in the file
        public static int ReadTheFirstLine()
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("File path is not set. Please ensure a file is selected before proceeding.");
            }

            try
            {
                lines = File.ReadAllLines(filePath).ToList();
                if (lines.Count == 0)
                {
                    throw new InvalidOperationException("The file is empty.");
                }

                int numberOfTestCases;
                if (!int.TryParse(lines[0], out numberOfTestCases))
                {
                    throw new FormatException("The first line must be an integer representing the number of test cases.");
                }

                if (numberOfTestCases <= 0)
                {
                    throw new InvalidOperationException("The number of test cases must be positive.");
                }

                lines.RemoveAt(0);
                return numberOfTestCases;
            }
            catch (Exception ex) when (!(ex is InvalidOperationException || ex is FormatException))
            {
                throw new IOException($"Error reading file: {ex.Message}", ex);
            }
        }

        public static string[] ReadTheLines()
        {
            string[] lA = lines.ToArray();
            return lA;
        }

        // save the results in the file
        public static void TheResults()
        {
            result = Cryptosystem.TheOutPut();
        }

        public static void TheTime()
        {
            time = Cryptosystem.TheTime();
        }

        public static void SaveResultsToFileManually()
        {
            while (true)
            {
                Console.WriteLine("Enter the directory path where you want to save the file (or 'exit' to cancel):");
                string directoryPath = Console.ReadLine()?.Trim();

                if (string.Equals(directoryPath, "exit", StringComparison.OrdinalIgnoreCase))
                    return;

                Console.WriteLine("Enter the file name (without extension):");
                string fileName = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(directoryPath) || string.IsNullOrEmpty(fileName))
                {
                    Console.WriteLine("Invalid path or file name. Please try again.");
                    continue;
                }

                string fullPath = Path.Combine(directoryPath, fileName + ".txt");

                if (WriteInTheFile(directoryPath, fullPath))
                    break;
            }
        }

        public static void SaveInThePc()
        {
            while (true)
            {
                string folderPath = SFolder();
                if (string.IsNullOrEmpty(folderPath))
                {
                    Console.WriteLine("No folder selected. Try again? (Y/N)");
                    if (Console.ReadLine().Trim().Equals("N", StringComparison.OrdinalIgnoreCase))
                        return;
                    continue;
                }

                Console.WriteLine("\nEnter file name (without extension):");
                string fileName = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(fileName))
                {
                    Console.WriteLine("Invalid file name. Please try again.");
                    continue;
                }

                string fullPath = Path.Combine(folderPath, fileName + ".txt");

                if (WriteInTheFile(folderPath, fullPath))
                    break;
            }
        }

        static string SFolder()
        {
            Console.WriteLine("\nPress any key to open folder dialog...");
            Console.ReadKey();

            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select folder to save the file";
                folderDialog.ShowNewFolderButton = true;
                return folderDialog.ShowDialog() == DialogResult.OK ? folderDialog.SelectedPath : null;
            }
        }

        public static bool WriteInTheFile(string directoryPath, string fullPath)
        {
            try
            {
                Directory.CreateDirectory(directoryPath);

                if (File.Exists(fullPath))
                {
                    Console.WriteLine($"File already exists at {fullPath}. Overwrite? (Y/N)");
                    if (!Console.ReadLine().Trim().Equals("Y", StringComparison.OrdinalIgnoreCase))
                        return false;
                }

                // Make sure we get the latest results and times before writing
                TheResults();
                TheTime();

                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    int itemCount = Math.Min(result.Count, time.Count);

                    for (int i = 0; i < itemCount; i++)
                    {
                        var res = result[i];
                        writer.WriteLine($"{res.Key}: {res.Value}");
                        writer.WriteLine($"Execution time: {time[i]} ms");
                        writer.WriteLine();
                    }

                    if (result.Count != time.Count)
                    {
                        writer.WriteLine($"Warning: Results/Time count mismatch - {result.Count} results vs {time.Count} time entries");
                    }
                }

                FileInfo fileInfo = new FileInfo(fullPath);
                Console.WriteLine($"\nFile saved successfully at: {fullPath}");
                Console.WriteLine($"File size: {fileInfo.Length} bytes");
                Console.WriteLine($"Entries saved: {Math.Min(result.Count, time.Count)}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError saving file: {ex.Message}");
                Console.WriteLine("Please try again with different path/name.");
                return false;
            }
        }
        public static void ChooseTheWay()
        {
            int choice;
            do
            {
                Console.WriteLine("Choose the way to save the file:");
                Console.WriteLine("1. Save in the PC");
                Console.WriteLine("2. Save manually");
                Console.WriteLine("3. Exit");

                string input = Console.ReadLine();
                if (!int.TryParse(input, out choice))
                {
                    Console.WriteLine("Invalid input! Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        SaveInThePc();
                        break;
                    case 2:
                        SaveResultsToFileManually();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Invalid option! Try again.");
                        break;
                }
            } while (choice < 1 || choice > 3);
        }
    }
}
