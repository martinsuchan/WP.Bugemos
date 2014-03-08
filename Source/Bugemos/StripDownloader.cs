using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Bugemos
{
    public class StripDownloader
    {
        public async static Task LoadRSSAsync(string feedUri)
        {
            StripModel.Instance.Downloading = true;

            WebClient webClient = new WebClient();
            string html;
            try
            {
                html = await webClient.DownloadStringTaskAsync(new Uri(feedUri));
            }
            catch (Exception e)
            {
                StripModel.Instance.Message = "Chyba při stahování seznamu stripů";
                StripModel.Instance.Downloading = false;
                return;
            }

            // Load the feed into a SyndicationFeed instance
            StringReader stringReader = new StringReader(html);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            SyndicationFeed feed = SyndicationFeed.Load(xmlReader);

            ObservableCollection<Strip> list = Settings.Strips.Value;
            List<int> invalidStrips = Settings.InvalidStrips.Value;

            foreach (SyndicationItem item in feed.Items)
            {
                string link = item.Links.First().Uri.ToString();
                int id = GetId(link);

                // if we have already processed item with this id, skip it
                if (list.Any(s => s.Id == id) || invalidStrips.Contains(id)) continue;

                Strip strip = new Strip
                {
                    Id = id,
                    Title = item.Title.Text,
                    Date = item.PublishDate.DateTime,
                    Link = link,
                };

                string img;
                try
                {
                    img = await GetImageAddressAsync(webClient, strip);
                }
                catch (Exception e)
                {
                    StripModel.Instance.Message = "Chyba při stahování stripu";
                    StripModel.Instance.Downloading = false;
                    return;
                }

                if (img != null)
                {
                    strip.Image = img;
                    strip.LocalImage = strip.Id + Path.GetExtension(img);
                    try
                    {
                        await DownloadFileAsync(webClient, strip);
                    }
                    catch (Exception e)
                    {
                        StripModel.Instance.Message = "Chyba při stahování stripu";
                        StripModel.Instance.Downloading = false;
                        return;
                    }
                    InsertImage(list, strip);
                }
                else
                {
                    invalidStrips.Add(id);
                }
                Settings.LastStrip.Value = Math.Max(Settings.LastStrip.Value, strip.Id);
            }

            StripModel.Instance.Downloading = false;
        }

        private static void InsertImage(ObservableCollection<Strip> list, Strip strip)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Id < strip.Id)
                {
                    list.Insert(i, strip);
                    return;
                }
            }

            list.Add(strip);
        }

        private static async Task<string> GetImageAddressAsync(WebClient webClient, Strip strip)
        {
            string html = await webClient.DownloadStringTaskAsync(new Uri(strip.Link));
            string img = GetImage(html);
            if (img != null)
            {
                return App.BugemosHome + img;
            }

            if (strip.Title.StartsWith("Trustport", StringComparison.OrdinalIgnoreCase))
            {
                Match content = Regex.Match(html, @"(?<=<div class=""content"">)([\s\S]*)(?=</div>)", RegexOptions.IgnoreCase);
                if (!content.Success) return null;

                MatchCollection mc = Regex.Matches(content.Value, "href\\s*=\\s*(?:\"(?<1>http://[^\"]*)\")", RegexOptions.IgnoreCase);
                foreach (Match match in mc)
                {
                    string href = match.Groups[1].Value;
                    if (href.StartsWith("http://www.trustport.com/blog/2"))
                    {
                        string trustport = await webClient.DownloadStringTaskAsync(new Uri(href));
                        img = GetTrustportImage(trustport);
                        return img;
                    }
                }
            }

            return null;
        }

        private static int GetId(string html)
        {
            Match m = Regex.Match(html, @"(?<=node/)[\s\S]*", RegexOptions.IgnoreCase);
            int id;
            return int.TryParse(m.Value, out id) ? id : 0;
        }

        private static string GetImage(string html)
        {
            Match m = Regex.Match(html, @"(?<=<div class=""content"">)[\s\S]*?(?=</div>)", RegexOptions.IgnoreCase);
            if (!m.Success) return null;

            Match m2 = Regex.Match(m.Value, @"(?<=<img src="")[\s\S]*?(?="")", RegexOptions.IgnoreCase);
            return m2.Success ? m2.Value : null;
        }

        private static string GetTrustportImage(string html)
        {
            Match m = Regex.Match(html, @"(?<=<div class=""entry-content"">)[\s\S]*?(?=</div>)", RegexOptions.IgnoreCase);
            if (!m.Success) return null;

            Match m2 = Regex.Match(m.Value, @"(?<=<img src="")[\s\S]*?(?="")", RegexOptions.IgnoreCase);
            return m2.Success ? m2.Value : null;
        }

        private static async Task DownloadFileAsync(WebClient webClient, Strip strip)
        {
            string imageUri = strip.Image;

            Stream str = await webClient.OpenReadTaskAsync(new Uri(imageUri));

            using (IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myStore.FileExists(strip.LocalImage))
                {
                    myStore.DeleteFile(strip.LocalImage);
                }

                byte[] buffer = new byte[4096];
                using (IsolatedStorageFileStream isoStorStr = myStore.OpenFile(strip.LocalImage, FileMode.CreateNew))
                {
                    int bytesRead;
                    while ((bytesRead = str.Read(buffer, 0, 4096)) > 0)
                    {
                        isoStorStr.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
        /*
        <?xml version="1.0" encoding="utf-8"?>
        <rss version="2.0" xml:base="http://www.bugemos.com"  xmlns:dc="http://purl.org/dc/elements/1.1/">
        <channel>
         <title>Buridanův geneticky modifikovaný osel</title>
         <link>http://www.bugemos.com</link>
         <description>Po více či méně úspěšných působeních na serverech student.cvut.cz a root.cz jsme se rozhodli vydat cestou ideově i časově nespoutané komiksové tvorby. Na tomto webu nečekejte tématický komiks, a už vůbec ne pravidelné publikování. Vše záleží na naší libovůli a náklonnosti zemních elementálů, ale snad se Vám tu bude líbit.
        Jojin&amp;HedgeHog</description>
         <language>cs</language>
        <item>
         <title>Superhrdinský</title>
         <link>http://www.bugemos.com/?q=node/385</link>
         <description>&lt;p&gt;Dnešní těžkej byz po dlouhé době doplňujeme jednoobrázkovým kouskem, kterým se zároveň vyjadřujeme k &lt;a href=&quot;http://www.supervaclav.cz&quot;&gt;aktuálnímu společenskému tématu&lt;/a&gt; maskovaných mstitelů...&lt;/p&gt;
        &lt;p&gt;&lt;a href=&quot;http://www.bugemos.com/?q=node/385&quot; target=&quot;_blank&quot;&gt;číst dál&lt;/a&gt;&lt;/p&gt;</description>
         <comments>http://www.bugemos.com/?q=node/385#comments</comments>
         <wfw:commentRss xmlns:wfw="http://wellformedweb.org/CommentAPI/">http://www.bugemos.com/?q=crss/node/385</wfw:commentRss>
         <category domain="http://www.bugemos.com/?q=taxonomy/term/63">1obrázkový</category>
         <category domain="http://www.bugemos.com/?q=taxonomy/term/44">Aktuální</category>
         <category domain="http://www.bugemos.com/?q=taxonomy/term/12">Různé</category>
         <category domain="http://www.bugemos.com/?q=taxonomy/term/173">Superman</category>
         <category domain="http://www.bugemos.com/?q=taxonomy/term/238">Virál</category>
         <pubDate>Fri, 21 Oct 2011 18:44:25 +0000</pubDate>
         <dc:creator>Jojin</dc:creator>
         <guid isPermaLink="false">385 at http://www.bugemos.com</guid>
        </item>

                public async static Task<List<Strip>> ProcessAsyncd()
                {
                    List<Strip> list = new List<Strip>();
                    WebClient web = new WebClient();

                    List<int> ii = new List<int>();
                    for (int j = 0; j < 350; j++)
                    {
                        ii.Add(j);
                    }

                    Strip[] strips = await TaskEx.WhenAll(ii.Select(DownloadItemAsync));

                    return strips.ToList();
                }

                private static async Task<Strip> DownloadItemAsync(int n)
                {
                    Uri uri = new Uri(string.Format(App.BugemosHome, n));
                    string html = await new WebClient().DownloadStringTaskAsync(uri);

                    if (!PageExists(html)) return null;

                    Strip p = new Strip
                    {
                        Link = uri.ToString(),
                        Title = GetTitle(html),
                        Date = GetDate(html),
                        Image = GetImage(html),
                        Description = GetDescription(html),
                        Serie = null,
                        Tags = null,
                    };

                    return p;
                }

                private const string errorMsg = "Stránka nenalezena";
                private static bool PageExists(string html)
                {
                    bool errorTitle = string.Compare(GetTitle(html), errorMsg, StringComparison.OrdinalIgnoreCase) == 0;
                    return !errorTitle;
                }

                private static string GetTitle(string html)
                {
                    Match m = Regex.Match(html, @"(?<=<h1 class=""title"">)([\s\S]*)(?=</h1>)", RegexOptions.IgnoreCase);
                    return m.Value;
                }

                private static DateTime GetDate(string html)
                {
                    Match m = Regex.Match(html, @"(?<=<span class=""submitted"">)([\s\S]*)(?=</span>)", RegexOptions.IgnoreCase);

                    DateTime dt;
                    if (!DateTime.TryParse(m.Value, out dt))
                    {
                        dt = DateTime.Now;
                    }
                    return dt;
                }

                private static string GetDescription(string html)
                {
                    Match m = Regex.Match(html, @"(?<=<div class=""content"">)([\s\S]*)(?=</div>)", RegexOptions.IgnoreCase);
                    if (!m.Success) return string.Empty;

                    Match m2 = Regex.Match(m.Value, @"(?<=<p>)([\s\S]*)(?=</p>)", RegexOptions.IgnoreCase);
                    return m2.Success ? m2.Value : string.Empty;
                }
        */
    }
}
