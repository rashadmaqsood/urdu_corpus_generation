using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairSentences
{
    class EditDistanceCalculator
    {

        // Maximum 2-D array coloumn size 
        const int maximum = 1000;

        // Utility function to find minimum of three numbers 
        int min(int x, int y, int z)
        {
            return min(min(x, y), z);
        }
        int min(int x, int y)
        {
            return x < y ? x : y;
        }

        public int levenshteinDistance(string s1,string s2)
        {

            int m = s1.Length;
            int n = s2.Length;
            int[,] vs = new int[s1.Length, s2.Length];
            initArray(vs, m, n);
            return editDist(s1, s2, m, n, vs);
        }

        public int levenshteinDistanceWordLevel (string s1, string s2)
        {
            char[] splitter = new char[] { ' ' };
            string[] s1list = s1.Split(splitter);
            string[] s2list = s2.Split(splitter);
            int m = s1list.Length;
            int n = s2list.Length;
            int[,] vs = new int[m, n];
            initArray(vs, m, n);
            return editDist(s1list, s2list, m, n, vs);
        }

        int editDist(string str1, string str2, int m, int n, int[,] dp)
        {
            // If first string is empty, the only option is to 
            // insert all characters of second string into first 
            if (m == 0)
                return n;

            // If second string is empty, the only option is to 
            // remove all characters of first string 
            if (n == 0)
                return m;

            // if the recursive call has been 
            // called previously, then return 
            // the stored value that was calculated 
            // previously 
            if (dp[m - 1,n - 1] != -1)
                return dp[m - 1,n - 1];

            // If last characters of two strings are same, nothing 
            // much to do. Ignore last characters and get count for 
            // remaining strings. 

            // Store the returned value at dp[m-1][n-1] 
            // considering 1-based indexing 
            if (str1[m - 1] == str2[n - 1])
                return dp[m - 1,n - 1] = editDist(str1, str2, m - 1, n - 1, dp);

            // If last characters are not same, consider all three 
            // operations on last character of first string, recursively 
            // compute minimum cost for all three operations and take 
            // minimum of three values. 

            // Store the returned value at dp[m-1][n-1] 
            // considering 1-based indexing 
            return dp[m - 1,n - 1] = 1 + min(editDist(str1, str2, m, n - 1, dp), // Insert 
                                            editDist(str1, str2, m - 1, n, dp), // Remove 
                                            editDist(str1, str2, m - 1, n - 1, dp) // Replace 
                                            );
        }

        int editDist(string[] str1, string[] str2, int m, int n, int[,] dp)
        {
            // If first string is empty, the only option is to 
            // insert all characters of second string into first 
            if (m == 0)
                return n;

            // If second string is empty, the only option is to 
            // remove all characters of first string 
            if (n == 0)
                return m;

            // if the recursive call has been 
            // called previously, then return 
            // the stored value that was calculated 
            // previously 
            if (dp[m - 1, n - 1] != -1)
                return dp[m - 1, n - 1];

            // If last characters of two strings are same, nothing 
            // much to do. Ignore last characters and get count for 
            // remaining strings. 

            // Store the returned value at dp[m-1][n-1] 
            // considering 1-based indexing 
            if (str1[m - 1].Equals(str2[n - 1]))
                return dp[m - 1, n - 1] = editDist(str1, str2, m - 1, n - 1, dp);

            // If last characters are not same, consider all three 
            // operations on last character of first string, recursively 
            // compute minimum cost for all three operations and take 
            // minimum of three values. 

            // Store the returned value at dp[m-1][n-1] 
            // considering 1-based indexing 
            return dp[m - 1, n - 1] = 1 + min(editDist(str1, str2, m, n - 1, dp), // Insert 
                                            editDist(str1, str2, m - 1, n, dp), // Remove 
                                            editDist(str1, str2, m - 1, n - 1, dp) // Replace 
                                            );
        }


        // Driver Code  

        void initArray(int[,] db,int m,int n)
        {
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    db[i,j] = -1;
        }

    }
}
