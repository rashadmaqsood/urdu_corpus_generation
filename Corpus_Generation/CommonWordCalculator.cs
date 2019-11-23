// A C# program to print union and 
// intersection of two unsorted arrays 
using PairSentences;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

class CommonWordCalculator
{
    #region Get Shared Words
    /// <summary>
    /// Calculates and returns shared words between two sentences
    /// </summary>
    /// <param name="arr1"></param>
    /// <param name="arr2"></param>
    /// <returns></returns>
    public static int getSahredWordsCount(string[] arr1,
                                string[] arr2)
    {
        int m = arr1.Length;
        int n = arr2.Length;
        int count = 0;
        // Before finding intersection, 
        // make sure arr1[0..m-1] is smaller 
        if (m > n)
        {
            string[] tempp = arr1;
            arr1 = arr2;
            arr2 = tempp;

            int temp = m;
            m = n;
            n = temp;
        }

        // Now arr1[] is smaller 
        // Sort smaller array arr1[0..m-1] 
        Array.Sort(arr1);

        // Search every element of bigger array in 
        // smaller array and print the element if found 
        for (int i = 0; i < n; i++)
        {
            if (binarySearch(arr1, 0, m - 1, arr2[i]) != -1)
                count++;
            //Console.Write(arr2[i] + " ");
        }
        return count;
    }

    // A recursive binary search function. 
    // It returns location of x in given 
    // array arr[l..r] is present, otherwise -1 
    static int binarySearch(string[] arr, int l,
                            int r, string x)
    {
        if (r >= l)
        {
            int mid = l + (r - l) / 2;

            // If the element is present at 
            // the middle itself 
            if (arr[mid] == x)
                return mid;

            // If element is smaller than mid, then it 
            // can only be present in left subarray 
            if (arr[mid].CompareTo(x) > 0)
                return binarySearch(arr, l, mid - 1, x);

            // Else the element can only be 
            // present in right subarray 
            return binarySearch(arr, mid + 1, r, x);
        }

        // We reach here when element is 
        // not present in array 
        return -1;
    }
    #endregion
   
}


