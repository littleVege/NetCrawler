using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NetCrawler
{
    public class Crawler:WebClient
    {
        public TaskQueue Tasks;
        private List<string> _urlStringList = new List<string>(); 
        public Crawler()
        {
            this.Encoding = Encoding.UTF8;
        }
        public void Start()
        {
            while (Tasks.Any())
            {
                var task = Tasks.Dequeue();
                try
                {
                    var data = this.DownloadString(task.Url);
                    if (task.Callback != null)
                    {
                        task.Callback(data, task.Url, this);
                    }
                    
                }
                catch (WebException e)
                {
                    /*TODO:process exceptions*/
                    switch (e.Status)
                    {
                        case WebExceptionStatus.ConnectFailure:
                        case WebExceptionStatus.Timeout:
                        case WebExceptionStatus.ConnectionClosed:
                            _doRetry(task);
                            break;
                    }

                }
            }

        }

        private void _doRetry(CrawlTask task)
        {
            task.Retryed += 1;
            Tasks.Enqueue(task);
        }
    }

    public delegate XmlDocument OnResponse(string data, Uri url, WebClient client);
}
