using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "FindLocalMax"; } }

        public override void TryMyCode()
        {
            /* WRITE 4~6 DIFFERENT CASES FOR TRACE, EACH WITH
             * 1) SMALL INPUT SIZE
             * 2) RETURNED indices
             * 4) PRINT THE CASE 
             */
            int N = 0, M = 0;
            int row, col;

            // Single Local Maxima
            N = 4;
            M = 4;
            int[,] matrix1 = { 
                                { 10, 8, 10, 10 },
                                { 14, 13, 12, 11 },
                                { 15, 9, 11, 21 },
                                { 16, 17, 19, 20 },
                            };
            (row, col) = PROBLEM_CLASS.FindLocalMaxDivideConquer(matrix1);
            PrintCase(N, M, matrix1, row, col);

            // Multiple Local Maxima in Corners
            N = 3;
            M = 3;
            int[,] matrix2 = {
                                { 5, 1, 5 },
                                { 1, 2, 1 },
                                { 5, 1, 5 }
                            };
            (row, col) = PROBLEM_CLASS.FindLocalMaxDivideConquer(matrix2);
            PrintCase(N, M, matrix2, row, col);

            // Multiple Peaks in Different Positions
            N = 3;
            M = 3;
            int[,] matrix3 = {
                                { 3, 9, 2 },
                                { 8, 5, 7 },
                                { 4, 6, 1 }
                            };
            (row, col) = PROBLEM_CLASS.FindLocalMaxDivideConquer(matrix3);
            PrintCase(N, M, matrix3, row, col);

            // Invalid Non-Empty Matrix (No Local Max)
            N = 5;
            M = 5;
            int[,] matrix4 = {
                                { 10, 12, 11, 13, 14 },
                                { 9, 15,  8, 16, 13 },
                                { 8, 14,  7, 15, 12 },
                                { 7, 13,  6, 14, 11 },
                                { 6, 12,  5, 13, 10 }
                            };
            (row, col) = PROBLEM_CLASS.FindLocalMaxDivideConquer(matrix4);
            PrintCase(N, M, matrix4, row, col);
        }

        Thread tstCaseThr;
        bool caseTimedOut ;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            /* READ THE TEST CASES FROM THE SPECIFIED FILE, FOR EACH CASE DO:
             * 1) READ ITS INPUT & EXPECTED OUTPUT
             * 2) READ ITS EXPECTED TIMEOUT LIMIT (IF ANY)
             * 3) CALL THE FUNCTION ON THE GIVEN INPUT USING THREAD WITH THE GIVEN TIMEOUT 
             * 4) CHECK THE OUTPUT WITH THE EXPECTED ONE
             */

            int testCases;
            int N = 0, M = 0;
            int[,] matrix = null;

            Stream s = new FileStream(fileName, FileMode.Open);
            BinaryReader br = new BinaryReader(s);

            testCases = br.ReadInt32();

            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            int i = 1;
            while (testCases-- > 0)
            {
                N = br.ReadInt32();
                M = br.ReadInt32();
                matrix = new int[N, M];
                for (int k = 0; k < N; k++)
                    for (int j = 0; j < M; j++)
                        matrix[k, j] = br.ReadInt32();

                //Console.WriteLine("N = {0}, Res = {1}", N, actualResult);
                int row = -1, col = -1;
                bool IsLocalMaxima = true;
                caseTimedOut = true;
                caseException = false;
                {
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            int sum = 0;
                            int numOfRep = 5;
                            Stopwatch sw = Stopwatch.StartNew();
                            for (int x = 0; x < numOfRep; x++)
                            {
                                (row, col) = PROBLEM_CLASS.FindLocalMaxDivideConquer(matrix);
                            }
                            sw.Stop();
                            // Check if the output cell contains the local maxima
                            if (row != -1 && col != -1)
                            {
                                int current = matrix[row, col];

                                // Check north neighbor
                                if (row > 0 && matrix[row - 1, col] > current)
                                    IsLocalMaxima = false;

                                // Check south neighbor
                                if (row < N - 1 && matrix[row + 1, col] > current)
                                    IsLocalMaxima = false;

                                // Check east neighbor
                                if (col < M - 1 && matrix[row, col + 1] > current)
                                    IsLocalMaxima = false;

                                // Check west neighbor
                                if (col > 0 && matrix[row, col - 1] > current)
                                    IsLocalMaxima = false;
                            }
                            //Console.WriteLine("N = {0}, time in ms = {1}", arr.Length, sw.ElapsedMilliseconds);
                        }
                        catch
                        {
                            caseException = true;
                        }
                        caseTimedOut = false;
                    });

                    if (readTimeFromFile)
                    {
                        timeOutInMillisec = br.ReadInt32();
                    }
                    /*LARGE TIMEOUT FOR SAMPLE CASES TO ENSURE CORRECTNESS ONLY*/
                    if (level == HardniessLevel.Easy)
                    {
                        timeOutInMillisec = 100; //Large Value 
                    }
                    /*=========================================================*/
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }

                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
                    tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                else if (IsLocalMaxima)    //Passed
                {
                    Console.WriteLine("Test Case {0} Passed!", i);
                    correctCases++;
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    Console.WriteLine(" your answer is wrong, please review your code");
                    wrongCases++;
                }

                i++;
            }
            s.Close();
            br.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0));
        }

        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        /// <summary>
        /// Generate a file of test cases according to the specified params
        /// </summary>
        /// <param name="level">Easy or Hard</param>
        /// <param name="numOfCases">Required number of cases</param>
        /// <param name="includeTimeInFile">specify whether to include the expected time for each case in the file or not</param>
        /// <param name="timeFactor">factor to be multiplied by the actual time</param>
        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helper Methods
        private static void PrintCase(int N, int M, int[,] matrix, int row, int col)
        {
            /* PRINT THE FOLLOWING
             * 1) INPUT
             * 2) RETURNED indices
             * 3) WHETHER IT'S CORRECT OR WRONG
             * */
            Console.WriteLine("N: {0}, M: {1}", N, M);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                    Console.Write(matrix[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine("Output for indices ({0}, {1}) = {2}", row, col, matrix[row, col]);
            bool IsLocalMaxima = true;
            if (row != -1 && col != -1)
            {
                int current = matrix[row, col];

                // Check north neighbor
                if (row > 0 && matrix[row - 1, col] > current)
                    IsLocalMaxima = false;

                // Check south neighbor
                if (row < N - 1 && matrix[row + 1, col] > current)
                    IsLocalMaxima = false;

                // Check east neighbor
                if (col < M - 1 && matrix[row, col + 1] > current)
                    IsLocalMaxima = false;

                // Check west neighbor
                if (col > 0 && matrix[row, col - 1] > current)
                    IsLocalMaxima = false;
            }
            Console.WriteLine("Your answer is: {0}", (IsLocalMaxima) ? "Correct" : "Wrong");
        }

        #endregion

    }
}
