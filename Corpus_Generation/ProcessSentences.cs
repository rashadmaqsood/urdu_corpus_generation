using Corpus_Generation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PairSentences
{
    public class ProcessSentences
    {
        static bool isProcessing = false;
        private const string path = ".\\COUNTER\\";
        EditDistanceCalculator Distance = new EditDistanceCalculator();
        public Action<string> UpdateStatus = delegate { };
        public static bool IsProcessing { get { return isProcessing; } }
        /// <summary>
        /// Read all documents from COUNTER folder placed in executeable directory
        /// Processes each document and store results in files in same directory
        /// </summary>
        public void ProcessDocuments()
        {
            try
            {
                isProcessing = true;
                Parallel.For(1, ProcessingStatus.TotalDocumentPairs + 1, i =>
                         {
                             string file2Name = "000" + i + "p.xml";
                             string file1Name = "000" + i + ".xml";
                             if (i > 99)
                             {
                                 file2Name = "0" + i + "p.xml";
                                 file1Name = "0" + i + ".xml";
                             }
                             else if (i > 9)
                             {
                                 file2Name = "00" + i + "p.xml";
                                 file1Name = "00" + i + ".xml";
                             }
                             Console.WriteLine(file1Name);
                             NewsDocument document1 = FileReader.ReadxmlDocument(path + file1Name);
                             NewsDocument document2 = FileReader.ReadxmlDocument(path + file2Name);
                             if (document1 == null || document2 == null)
                                 return;
                             List<string> document1SentencesList = ExtractSentencesFromText(document1.Detail);
                             List<string> document2SentencesList = ExtractSentencesFromText(document2.Detail);
                             document1SentencesList.Insert(0, document1.HeadLine);
                             document2SentencesList.Insert(0, document2.HeadLine);
                             PairSentencesAndSave(document1SentencesList, document2SentencesList, document1.Classification, document1.Domain, document1.NewsDate, document1.NewsPaper, document2.NewsPaper);

                             ProcessingStatus.IncrementProcessed();
                             if (ProcessingStatus.TotalDocumentPairs == ProcessingStatus.TotalDocumentPairs - 1)
                                 ProcessingStatus.IncrementProcessed();
                             if (ProcessingStatus.ProcessedDocumentPairs >= ProcessingStatus.TotalDocumentPairs)
                             {
                                 isProcessing = false;
                             }
                           //await Task.Delay(100);
                       });
            }
            catch (Exception ex)
            {
                isProcessing = false;
                Logger.LogException(Logger.GetCurrentMethod(), ex.Message, ex.StackTrace);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Extracts Sentences from the text passed
        /// </summary>
        /// <param name="text">text to be splittes into sentences</param>
        /// <returns></returns>
        private List<string> ExtractSentencesFromText(string text)
        {
            char[] splitter = new char[] { '۔' };
            List<string> sentences = new List<string>(text.Split(splitter));
            return sentences;
        }
        /// <summary>
        /// Pairs sentences from both lists of sentences , Checks if All Three filters are satisfied 
        /// then save the pair into respective file.
        /// </summary>
        /// <param name="set1">List of Sentence from source document</param>
        /// <param name="set2">List of Sentecnes from derived documents</param>
        /// <param name="classification">category of sentence pair in COUNTER corpus</param>
        /// <param name="domain">domain of news i.e supports news , showbiz news etc</param>
        /// <param name="newsDate">date of news published</param>
        /// <param name="agency">agency from which news was origionaly released</param>
        /// <param name="newsPaper">Name of newspaper in which news article was published</param>
        private void PairSentencesAndSave(List<string> set1, List<string> set2, string classification,
            string domain, string newsDate, string agency, string newsPaper)
        {
            try
            {
                if (set1 == null || set2 == null)
                    return;
                char[] splitter = new char[] { ' ' };

                Parallel.For(0, set1.Count, i =>
                {
                    Parallel.For(0, set2.Count, j =>
                    {
                        try
                        {
                            string s1 = set1[i];
                            string s2 = set2[j];
                            string[] s1list = s1.Split(splitter);
                            string[] s2list = s2.Split(splitter);
                            sentence_pair _Pair = new sentence_pair()
                            {
                                sentence_1 = s1,
                                sentence_2 = s2,
                                sentence_1_no = i,
                                sentence_2_no = j,
                                sentence_1_length = s1.Length,
                                sentence_2_length = s2.Length,
                                classification = classification,
                                domain = domain,
                                newsdate = newsDate,
                                news_agency = agency,
                                news_paper = newsPaper,
                                sentence1_words = s1.Split(splitter).Length,
                                sentence2_words = s2.Split(splitter).Length,
                                shared_words = CommonWordCalculator.getSahredWordsCount(s1list, s2list),
                                edit_distance_word_level = Distance.levenshteinDistanceWordLevel(s1, s2),
                                edit_distance = Distance.levenshteinDistance(s1, s2)
                            };
                            if (_Pair.sentence_1_length > 0 &&
                               _Pair.sentence_2_length > 0 &&
                               _Pair.edit_distance_word_level > 1)
                            {
                                if (ClassificationFilters.IsFiltersSatisfied(_Pair))
                                {
                                    FileWriter.SaveToFilteredSentence(_Pair);
                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            foreach (var errInner in err.InnerExceptions)
                            {
                                Logger.LogException(Logger.GetCurrentMethod(), errInner.Message, "Inner");
                            }
                        }
                        catch (Exception ex)
                        {


                            Logger.LogException(Logger.GetCurrentMethod(), ex.Message, ex.StackTrace);
                        }

                    });
                });
            }
            catch (Exception ex)
            {

                Logger.LogException(Logger.GetCurrentMethod(), ex.Message, ex.StackTrace);
            }
        }
    }
}
