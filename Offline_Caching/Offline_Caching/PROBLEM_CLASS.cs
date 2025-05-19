using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MS.Internal;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************

    public static class PROBLEM_CLASS
    {
        #region YOUR CODE IS HERE 


        //Your Code is Here:
        //==================
        /// <summary>
        /// Implements the Off-line Caching Algorithm to minimize cache misses.
        /// The function processes a sequence of memory requests while managing a cache of limited size.
        /// </summary>
        /// <param name="cacheSize">The maximum number of elements that can be stored in the cache.</param>
        /// <param name="requests">An array representing the sequence of memory requests.</param>
        /// <returns>The Min number of cache misses after processing the entire request sequence.</returns>
        static public int OfflineCaching(int cacheSize, string[] requests)
        {
            // Your solution should be less than O(K * N^2)
            // where N is the requests sequence size, and K is the Cache Size

            // Remove this line before you start coding
            //throw new NotImplementedException();

            

            // bStore amaken tekrar kol request
            Dictionary<string, List<int>> nextUse = new Dictionary<string, List<int>>();
            string request;
            for (int i = 0; i < requests.Length; i++)
            {
                 request = requests[i];
                if (!nextUse.ContainsKey(request))
                    nextUse[request] = new List<int>();

                nextUse[request].Add(i);
            }

            HashSet<string> cache = new HashSet<string>();
            int missCount = 0;
            string currentRequest = "";

            for (int i = 0; i < requests.Length; i++)
            {
                 currentRequest = requests[i];

                // lw el request mawgood msh bdefo
                if (cache.Contains(currentRequest)) continue;

                missCount++;

                // lw elcash feh makan bdef 3latol
                if (cache.Count < cacheSize)
                {
                    cache.Add(currentRequest);
                    continue;
                }

                // lw elcash malean bdwer 3la A7sen evictCandidate
                string evictCandidate = null;
                int farthestNextUse = -1;

                foreach (string elem in cache)
                {
                    // Find the next occurrence of 'elem' after index 'i'
                    List<int> occurrences = nextUse[elem];
                    int nextOccurrenceIndex = SearchOnTheOccuranceList(occurrences, i);

                    if (nextOccurrenceIndex == -1)
                    {
                        // M3naha mesh mawgood so hekon a7san evictCandidate
                        evictCandidate = elem;
                        break;
                    }
                    else if (nextOccurrenceIndex > farthestNextUse)
                    {
                        farthestNextUse = nextOccurrenceIndex;
                        evictCandidate = elem;
                    }
                }

                // Eviction 
                cache.Remove(evictCandidate);
                cache.Add(currentRequest);
            }

            return missCount;
        }

        private static int SearchOnTheOccuranceList(List<int> occurrences, int currentIndex)
        {
            int left = 0;
            int right = occurrences.Count - 1;
            int result = -1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (occurrences[mid] > currentIndex)
                {
                    result = occurrences[mid];
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            return result;
        }

        #endregion
    }
}
