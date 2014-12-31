using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetCrawler
{
    public class CrawlTask
    {
        public Dictionary<string, string> Header;
        public Uri Url;
        public DateTime CreateAt = DateTime.Now;
        public int Retryed = 0;
        public string Method = "get";
        
        public CrawlTask(string url):this(url,"get",null)
        {
            
        }

        public CrawlTask(string url, string method):this(url, method, null)
        {
            
        }

        public CrawlTask(string url, string method, Dictionary<string, string> header)
        {
            method = method.ToLower();
            var methodReg = new Regex(@"^((post)|(get)|(delete)|(put))$", RegexOptions.ECMAScript);
            Url = new Uri(url);
            Header = new Dictionary<string, string>
            {
                {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"},
                {"Accept-Encoding", "gzip, deflate, sdch"},
                {"User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36"}
            };

            Method = methodReg.IsMatch(method) ? method : "get";
            if (method == "post")
            {
                Header.Add("Content-type", "application/x-www-form-urlencoded");
            }

            if (header != null)
            {
                foreach (var pair in header)
                {
                    if (!Header.ContainsKey(pair.Key))
                    {
                        Header.Add(pair.Key, pair.Value);
                    }
                    else
                    {
                        Header[pair.Key] = pair.Value;
                    }
                }
            }

        }
    }
}
