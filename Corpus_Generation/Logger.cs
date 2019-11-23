using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Corpus_Generation
{
    public static class Logger
    {
        #region Exception Log
        static object fileLock = new object();
        public static void LogException(string methodName, string error, string detail)
        {
            string line = "=============================================================================";
            string log = DateTime.Now.ToString() + " => " + methodName + Environment.NewLine +
                error + Environment.NewLine + detail + Environment.NewLine + line + Environment.NewLine;
            try
            {
                lock (fileLock)
                {

                    File.AppendAllText("error.txt", log);
                }
            }
            catch (Exception)
            {

                Console.Write(log);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
        #endregion
    }
}
