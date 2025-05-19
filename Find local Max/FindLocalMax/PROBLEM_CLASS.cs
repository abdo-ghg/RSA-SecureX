using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class PROBLEM_CLASS
    {
        #region YOUR CODE IS HERE 

        enum SOLUTION_TYPE { NAIVE, EFFICIENT };
        static SOLUTION_TYPE solType = SOLUTION_TYPE.EFFICIENT;

        //Your Code is Here:
        //==================
        /// <summary>
        /// short descrition about the function 
        /// </summary>
        /// <param name="param1">param description </param>
        /// <param name="param2">param description </param>
        /// <returns>return value</returns>
        public static (int, int) FindLocalMaxDivideConquer(int[,] matrix)
        {
            int row = matrix.GetLength(0); //O(1)
            int col = matrix.GetLength(1); //O(1)
            return FindLocalMax(matrix, 0, col - 1, row); //O(n log(n))

            //REMOVE THIS LINE BEFORE START CODING
            //throw new NotImplementedException();

        }
        private static (int, int) FindLocalMax(int[,] matrix, int start, int end, int totalRows)
        {
            // Base case (matrix khelset)
            if (start > end) //O(1)
                return (-1, -1); //O(1)

            int mid = (start + end) / 2; //middle column's index //O(1)
            int maxRow = 0; //O(1)
            for (int i = 1; i < totalRows; i++) //O(n)
            {
                if (matrix[i, mid] > matrix[maxRow, mid])//O(1)
                    maxRow = i; //maximum bas to start with ya3ny //O(1)
            }

            int current = matrix[maxRow, mid];  //O(1)
            int left, right; //O(1)
            //check for boundaries
            if (mid - 1 >= start) //O(1)
            {
                left = matrix[maxRow, mid - 1]; //O(1)
            }
            else
            {
                left = -1; //O(1)
            }
            if (mid + 1 <= end) //O(1)
            {
                right = matrix[maxRow, mid + 1]; //O(1)
            }
            else
            {
                right = -1; //O(1)
            }

            if (current >= left && current >= right) //O(1)
            {
                return (maxRow, mid); //found //O(1)
            }
            else if (left > current) //O(1)
            {
                return FindLocalMax(matrix, start, mid - 1, totalRows); //O(log(n))
            }
            else
            {
                return FindLocalMax(matrix, mid + 1, end, totalRows); //O(log(n))
            }
        }
        
        #endregion
    }
}
