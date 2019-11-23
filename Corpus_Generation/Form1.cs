using Corpus_Generation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PairSentences
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         }

        private void PairSentences(string _directory)
        {
            if (Directory.Exists(_directory))
            {
                string[] files = Directory.GetFiles(_directory);
                EditDistanceCalculator common = new EditDistanceCalculator();
                foreach(string article in files)
                {
                    if (article.EndsWith(".txt"))
                    {
                        string[] main_article_sentences = File.ReadAllLines(article);
                        foreach (string sub_article in files)
                        {
                            if (article.EndsWith(".txt") && article != sub_article)
                            {
                                string[] sub_article_sentences = File.ReadAllLines(sub_article);

                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Specified Directory Does Not Exist.");
            }
        }


        ProcessSentences processSentences = new ProcessSentences();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                CheckForIllegalCrossThreadCalls = false;
                ProcessingStatus.StartTime = DateTime.Now;

                Task.Run(()=> processSentences.ProcessDocuments());
                Task.Run(() => UpdateGUI());


                //MessageBox.Show("Completed");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {

            }

        }
        object lblLock = new object();
        private void UpdateSentence(string s)
        {
            try
            {
                //lock(lblLock)
                {

                if (lblStatus.InvokeRequired)
                {
                    lblStatus.Invoke(new MethodInvoker(() => { lblStatus.Text = s; }));
                }
                else
                {
                    lblStatus.Text = s;
                }
                Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(Logger.GetCurrentMethod(), ex.Message, ex.StackTrace);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private  async void UpdateGUI()
        {
            while (ProcessSentences.IsProcessing)
            {
                int total = ProcessingStatus.TotalDocumentPairs;
                int processed = ProcessingStatus.ProcessedDocumentPairs;
                int remaining = ProcessingStatus.RemainingDocumentPairs;

                string elapsed = ProcessingStatus.ElapsedTime.ToString();
                string remainigTime = ProcessingStatus.RemainingTime.ToString();

                if (this.InvokeRequired)
                {
                    new MethodInvoker(() =>
                    {
                        lblTimeElapsed.Text = elapsed;
                        lblTimeRemaining.Text = remainigTime;

                        lblTotalDocuments.Text = total.ToString();
                        lblProcessedDocuments.Text = processed.ToString();
                        lblRemainingDocuments.Text = remaining.ToString();
                    }).Invoke();

                }
                else
                {
                    lblTimeElapsed.Text = elapsed;
                    lblTimeRemaining.Text = remainigTime;

                    lblTotalDocuments.Text = total.ToString();
                    lblProcessedDocuments.Text = processed.ToString();
                    lblRemainingDocuments.Text = remaining.ToString();
                }
                Application.DoEvents();
                await Task.Delay(800); 
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //SaveToFile.Test();
        }
    }
}
