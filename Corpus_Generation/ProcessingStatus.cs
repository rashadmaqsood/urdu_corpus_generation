using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PairSentences
{
    public class ProcessingStatus
    {
        static ProcessingStatus()
        {
            TotalDocumentPairs = 600;
        }
        static int _processed = 0;
        public static DateTime StartTime { get; set; }
        public static TimeSpan ElapsedTime { get { return (DateTime.Now - StartTime); } }
        public static TimeSpan RemainingTime { get {
                int processed = ProcessedDocumentPairs;
                int remaining = RemainingDocumentPairs;

                double seconds_per_document = ElapsedTime.TotalSeconds / processed;
                int totalseconds = (int)(seconds_per_document * remaining);
                return new TimeSpan(totalseconds/3600, totalseconds/60, totalseconds%60);
            }
        }
        public static int TotalDocumentPairs { get; set; }
        public static int ProcessedDocumentPairs
        {
            get
            {
                int toReturn = 0;
                Interlocked.Exchange(ref toReturn, _processed);
                return toReturn;
            }
        }
        public static int RemainingDocumentPairs {
            get
            {
                return TotalDocumentPairs - ProcessedDocumentPairs;
            }
        }

        public static void IncrementProcessed()
        {
            Interlocked.Increment(ref _processed);
        }
    }
}
