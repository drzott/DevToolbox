using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace RiskRSSReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Init");

            Init();

            Console.WriteLine("Loading Feed Urls");

            List<RiskSource> rs = GetFeedUrls();

            string targetTable = RiskRSSReader.Properties.Settings.Default.TargetTable;

            if (string.IsNullOrEmpty(targetTable))
                targetTable = "RSS_RISK";

            foreach (RiskSource r in rs)
            {
                try
                {
                    Console.WriteLine("Reading :" + r.Url);



                    XmlReader reader = null;


                    if (RiskRSSReader.Properties.Settings.Default.ProxyAuth)
                    {
                        Console.WriteLine("ProxyAuth");

                        WebClient client = new WebClient();
                        client.Proxy = new WebProxy(RiskRSSReader.Properties.Settings.Default.Proxy);
                        client.Proxy.Credentials = new NetworkCredential(RiskRSSReader.Properties.Settings.Default.ProxyLogin, RiskRSSReader.Properties.Settings.Default.ProxyPW);

                        string xml = client.DownloadString(r.Url);

                        StringReader rd = new StringReader(xml);

                        reader = XmlReader.Create(rd);
                    }
                    else
                    {
                        reader = XmlReader.Create(r.Url);
                    }

                    SyndicationFeed feed = SyndicationFeed.Load(reader);

                    foreach(SyndicationItem item in feed.Items)
                    {
                        string link = string.Empty;
                        string title = string.Empty;
                        string content = string.Empty;
                        DateTimeOffset lastUpdate = new DateTimeOffset(new DateTime(1901, 1, 1));

                        if(item != null && item.Links != null && item.Links.Count >= 0 && item.Links[0].Uri != null && !string.IsNullOrEmpty(item.Links[0].Uri.ToString()))
                        {
                            link = item.Links[0].Uri.ToString();

                            if(link.Length > 2000)
                            {
                                link = string.Empty;
                            }
                        }

                        if(item != null && item.Title != null && !(string.IsNullOrEmpty(item.Title.Text)))
                        {
                            title = item.Title.Text.Trim();
                            title = StripHTML(title);

                            if(title.Length > 500)
                            {
                                title = title.Substring(0,499);
                            }
                        }

                        if(item != null && item.LastUpdatedTime != null)
                        {
                            lastUpdate = item.LastUpdatedTime;
                        }

                        if(item != null && item.Content != null)
                        {
                            TextSyndicationContent c = (TextSyndicationContent)item.Content;

                            if(!string.IsNullOrEmpty(c.Text))
                            {
                                content = c.Text.Trim();
                                content = StripHTML(content);

                                if(content.Length > 4000)
                                {
                                    content = content.Substring(0, 3999);
                                }
                            }                           
                        }

                        int hash = (title + content).GetHashCode();

                        using (SqlConnection conn = new SqlConnection(RiskRSSReader.Properties.Settings.Default.ConnectionString))
                        {
                            conn.Open();



                            string sql = @"

                                    INSERT INTO [dbo]." + targetTable + @" 
                                               ([Url],[SupplierId],[FeedName],[Title],[Link],[LastUpdate],[Content],[HashValue])
                                    Values
                                               (@u, @sup, @f, @t, @l, @lu, @c, @hv)";

                            SqlCommand cmd = new SqlCommand(sql, conn);

                            cmd.Parameters.AddWithValue("@u", r.Url);
                            cmd.Parameters.AddWithValue("@sup", r.SupplierId);
                            cmd.Parameters.AddWithValue("@f", r.FeedName);
                            cmd.Parameters.AddWithValue("@t", title);
                            cmd.Parameters.AddWithValue("@lu", lastUpdate);
                            cmd.Parameters.AddWithValue("@l", link);
                            cmd.Parameters.AddWithValue("@c", content);
                            cmd.Parameters.AddWithValue("@hv", hash.ToString());

                            cmd.ExecuteNonQuery();
                        }
                    }

                }catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                }
            }
        }

        public static string StripHTML(string HTMLText, bool decode = true)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            var stripped = reg.Replace(HTMLText, "");
            return decode ? System.Net.WebUtility.HtmlDecode(stripped) : stripped;
        }


        private static List<RiskSource> GetFeedUrls()
        {
            List<RiskSource> res = new List<RiskSource>();

            try
            {
                using (SqlConnection conn = new SqlConnection(RiskRSSReader.Properties.Settings.Default.ConnectionString))
                {
                    conn.Open();

                    string sql = @"
                            SELECT [" + RiskRSSReader.Properties.Settings.Default.FeedColumn + "] as Url, [" 
                                    + RiskRSSReader.Properties.Settings.Default.FeedSupplierId + "] as SupplierId, [" 
                                    + RiskRSSReader.Properties.Settings.Default.FeedName + "] as FeedName FROM [" 
                                    + RiskRSSReader.Properties.Settings.Default.FeedTable + "]";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            RiskSource rs = new RiskSource();

                            string supId = dr["SupplierId"].ToString();
                            string url = dr["Url"].ToString();
                            string fn = dr["FeedName"].ToString();

                            if (string.IsNullOrEmpty(supId) || string.IsNullOrEmpty("url"))
                                continue;

                            rs.SupplierId = supId.Trim();
                            rs.Url = url.Trim();
                            rs.FeedName = fn.Trim();

                            Console.WriteLine(supId + " " + url);
                            res.Add(rs);
                        }
                    }
                }

            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return res;
            }

            return res;
        }

        private static void Init()
        {
            using (SqlConnection conn = new SqlConnection(RiskRSSReader.Properties.Settings.Default.ConnectionString))
            {

                string targetTable = RiskRSSReader.Properties.Settings.Default.TargetTable;

                if (string.IsNullOrEmpty(targetTable))
                    targetTable = "RSS_RISK";

                conn.Open();

                string sql = @"

                                    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + targetTable + @"')
                                    BEGIN
	
	                                    CREATE TABLE " + targetTable + @"
	                                    (
	                                    Url varchar(2000) NULL,
	                                    SupplierId varchar(500) NULL,
	                                    FeedName varchar(500) NULL,
	                                    Title varchar(500) NULL,
	                                    Link varchar(2000) NULL,
	                                    LastUpdate datetime NULL,
	                                    Content varchar(4000) NULL,
	                                    HashValue varchar(100) NULL
		                                    )

                                    END

                                    TRUNCATE TABLE " + targetTable + @"
                            ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
