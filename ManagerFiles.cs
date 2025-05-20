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

        // take the file path from the user
        public static bool ReadFileFromUser()//O(1)
        {
            Console.Write("Enter file path: ");//O(1)
            string userFilePath = Console.ReadLine()?.Trim();//O(1)

            try
            {
                if (string.IsNullOrEmpty(userFilePath))//O(1)
                {
                    Console.WriteLine("No path entered.");//O(1)
                    return false;//O(1)
                }

                if (!File.Exists(userFilePath))//O(1)
                {
                    Console.WriteLine("File not found. Try again.");//O(1)
                    return false;//O(1)
                }

                filePath = userFilePath;//O(1)
                Console.WriteLine("File found.");//O(1)
                return true;//O(1)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");//O(1)
                return false;//O(1)
            }
        }

        // read the file
        public static void ReadFile()//O(N)
        {
            bool fileSelected = false;//O(1)
            while (!fileSelected)
            {
                ReadFileFromUser();

                if (string.IsNullOrEmpty(filePath))
                {
                    Console.WriteLine("No file was selected. Please try again or press 1 to exit.");
                    continue;
                }

                fileSelected = true;//O(1)
            }
        }

        // read the line in the file
        public static int ReadTheFirstLine()//O(N) 
        {
            if (string.IsNullOrEmpty(filePath))//O(1)
            {
                throw new InvalidOperationException("File path is not set. Please ensure a file is selected before proceeding.");//O(1)
            }

            try
            {
                lines.Clear();//O(1)
                lines = File.ReadAllLines(filePath).ToList();//O(N) N is the lines.count
                if (lines.Count == 0)//O(1)
                {
                    throw new InvalidOperationException("The file is empty.");//O(1)
                }

                int numberOfTestCases;
                if (!int.TryParse(lines[0], out numberOfTestCases))//O(1)
                {
                    throw new FormatException("The first line must be an integer representing the number of test cases.");//O(1)
                }

                if (numberOfTestCases <= 0)//O(1)
                {
                    throw new InvalidOperationException("The number of test cases must be positive.");//O(1)
                }

                lines.RemoveAt(0);//O(N) N is the size
                return numberOfTestCases;//O(1)
            }
            catch (Exception ex) when (!(ex is InvalidOperationException || ex is FormatException))
            {
                throw new IOException($"Error reading file: {ex.Message}", ex);//O(1)
            }
        }

        public static string[] ReadTheLines()//O(N) N is the lines.count 
        {
            filePath = null;//O(1)
            string[] lA = lines.ToArray();
            lines.Clear();//O(1)
            return lA;
        }
        

        // save the results in the file
        public static void TheResults()
        {
            result.Clear();//O(N)
            result = Cryptosystem.TheOutPut();
        }

        public static void TheTime()
        {
            time.Clear();//O(N)
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
                result.Clear();//O(N)
                time.Clear();//O(N)
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
