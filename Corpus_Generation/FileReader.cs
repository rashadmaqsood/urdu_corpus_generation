using PairSentences;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Corpus_Generation
{
    public static class FileReader
    {
        public static NewsDocument ReadxmlDocument(string docId)
        {
            try
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                XmlNodeList xmlnode;
                //int i = 0;
                FileStream fs = new FileStream(docId, FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                xmlnode = xmldoc.GetElementsByTagName("COUNTER_document");
                //for (i = 0; i <= xmlnode.Count - 1; i++)
                if (xmlnode.Count > 0)
                {
                    NewsDocument doc = new NewsDocument();
                    //str = xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
                    doc.HeadLine = xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
                    doc.Detail = xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
                    doc.Classification = xmlnode[0].Attributes["classification"].Value;

                    doc.Domain = xmlnode[0].Attributes["domain"].Value;
                    doc.NewsDate = xmlnode[0].Attributes["newsdate"].Value;
                    doc.NewsPaper = xmlnode[0].Attributes["newspaper"].Value;
                    return doc;
                    //MessageBox.Show(str);
                }
            }
            catch (Exception ex)
            {

                Logger.LogException(Logger.GetCurrentMethod(), ex.Message, ex.StackTrace);
            }

            return null;
        }
    }
}
