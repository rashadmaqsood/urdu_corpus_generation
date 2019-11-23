using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PairSentences
{
    public class sentence_pair
    {
        public int id { get; set; }

        public string sentence_1 { get; set; }

        public string sentence_2 { get; set; }

        public int sentence_1_no { get; set; }

        public int sentence_2_no { get; set; }

        public int edit_distance { get; set; }

        public int sentence_1_length { get; set; }

        public int sentence_2_length { get; set; }

        public string classification { get; set; }

        public string domain { get; set; }

        public string newsdate { get; set; }

        public string news_agency { get; set; }

        public string news_paper { get; set; }

        public int edit_distance_word_level { get; set; }

        public int sentence1_words { get; set; }

        public int sentence2_words { get; set; }

        public int shared_words { get; set; }

    }
}
