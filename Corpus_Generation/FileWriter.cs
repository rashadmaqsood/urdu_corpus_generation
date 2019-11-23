using PairSentences;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Corpus_Generation
{
    public static class FileWriter
    {
        #region Class Members

        readonly static SaveInfo FilteredWhollyDerivedSentences = new SaveInfo("filtered_wholly_derived_sentence_pairs",768);
        readonly static SaveInfo FilteredPartiallyDerivedSentences = new SaveInfo("filtered_partially_derived__sentence_pairs",1474);
        readonly static SaveInfo FilteredNonDerviedSentences = new SaveInfo("filtered_non_derived_sentence_pairs",848);

        #endregion

        #region Public Methods

        public static void SaveToFilteredSentence(sentence_pair pair)
        {
            switch (pair.classification)
            {
                case "PD": SavePairToFile(pair, FilteredPartiallyDerivedSentences); break;
                case "WD": SavePairToFile(pair, FilteredWhollyDerivedSentences); break;
                case "ND": SavePairToFile(pair, FilteredNonDerviedSentences); break;
            }
        }


        #endregion

        #region Private Methods
        static void SavePairToFile(sentence_pair pair, SaveInfo info)
        {

            lock (info.lock_obj)
            {
                if (info.Counter <= info.MaxSentencePairs)
                {
                    Save(pair, info);
                    info.Increment();
                }
            }
        }
        static void Save(sentence_pair pair, SaveInfo info)
        {
            string fileName = info.FileName + ".txt";
            if (!File.Exists(fileName))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine("<?xml version = \"1.0\" encoding = \"utf - 8\" ?>");
                    sw.WriteLine("<sentence_pairs>");
                }
            }


            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine($"<sentence_pair classification = \"\" edit_distance = \"{pair.edit_distance}\" shared_words = \"{pair.shared_words}\" pair_no = \"{info.Counter}\" >");

                sw.WriteLine($"<sentence_1>{pair.sentence_1}</sentence_1>");

                sw.WriteLine($"<sentence_2>{pair.sentence_2}</sentence_2>");

                sw.WriteLine($"</sentence_pair>");

            }
        }
        #endregion
    }

    class SaveInfo
    {
        int _counter = 1;
        public string FileName { get; private set; }
        public object lock_obj { get; private set; }
        public int Counter { get { int counterToReturn = 0; Interlocked.Exchange(ref counterToReturn, _counter); return counterToReturn; } }
        public int MaxSentencePairs { get; private set; }
        public SaveInfo(string fileName,int maxSentences)
        {
            FileName = fileName;
            lock_obj = new object();
            MaxSentencePairs = maxSentences;
        }

        public void Increment()
        {
            Interlocked.Increment(ref _counter);
        }
    }
}
