using PairSentences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corpus_Generation
{
    public static class ClassificationFilters
    {
        #region IsFiltersSatisfied
        static bool IsFilter1Satisfied(sentence_pair pair)
        {
            if (
                (pair.sentence_1_no <= 2 && pair.sentence_2_no <= 2)
                ||
                (
                    pair.edit_distance_word_level >= 1 && pair.edit_distance_word_level <= 20
                    &&
                    (
                        (pair.sentence_1_length < pair.sentence_2_length && pair.sentence_1_length * 2 >= pair.sentence_2_length)
                        ||
                        (pair.sentence_2_length < pair.sentence_1_length && pair.sentence_2_length * 2 >= pair.sentence_1_length)
                    )
                )
                )
                return true;
            return false;
        }
        static bool IsFilter2Satisfied(sentence_pair pair)
        {
            if (
                pair.sentence1_words >= 5 && pair.sentence1_words <= 40 &&
                pair.sentence2_words >= 5 && pair.sentence2_words <= 40
                )
                return true;
            return false;
        }
        static bool IsFilter3Satisfied(sentence_pair pair)
        {
            if (
                pair.shared_words >= 5
                )
                return true;
            return false;
        }
        public static bool IsFiltersSatisfied(sentence_pair pair)
        {
            if (
                IsFilter1Satisfied(pair) && IsFilter2Satisfied(pair) && IsFilter3Satisfied(pair)
                )
                return true;
            return false;
        }
        #endregion
    }
}
