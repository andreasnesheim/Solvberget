using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace Solvberget.Domain.DTO
{
    public class WebPage
    {
        WebClient client = new WebClient();

        public string Link { get; set; }
        public string Html { get; set; }

        public string GetHtml()
        {
            string html = client.DownloadString(Link);
            //html = html.Replace("\n", "");
            html = html.Replace("\\", "");
            return html;
        }

        public static string StripHtmlTags(string strHtml)
        {
            return Regex.Replace(strHtml, "<(.|\n)*?>", "");
        }

        public static HtmlNode GetDiv(string strHtml, string divName)
        {

            var expression = "//div[@class='"+divName+"']";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(strHtml);
            if (htmlDoc.DocumentNode != null)
            {
                HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode(expression);

                if (node != null)
                {
                    // Do something with bodyNode
                    return node;
                }
            }
            return null;
        }

        public static string CleanHtml(string strHtml)
        {
            var strippedTagsHtml = StripHtmlTags(strHtml);
            var cleanedHtml = strippedTagsHtml.Replace("\n", " ");

            cleanedHtml = cleanedHtml.Replace("–", "");
            cleanedHtml = cleanedHtml.Replace("å", "�");
            cleanedHtml = cleanedHtml.Replace("Å", "�");
            cleanedHtml = cleanedHtml.Replace("ø", "�");
            cleanedHtml = cleanedHtml.Replace("Ø", "�");
            cleanedHtml = cleanedHtml.Replace("é", "�");
            cleanedHtml = cleanedHtml.Replace("» ", "");
            cleanedHtml = cleanedHtml.Replace("» ", "");
            cleanedHtml = cleanedHtml.Replace("► ", "");
            cleanedHtml = cleanedHtml.Replace("►", "");
            cleanedHtml = cleanedHtml.Replace("►O", "O");
            cleanedHtml = cleanedHtml.Replace("&nbsp;", " ");
 

           
            cleanedHtml = cleanedHtml.Replace("Hvor er vi?  Se kart p� Google maps", "");
            cleanedHtml = cleanedHtml.Trim();
            return cleanedHtml;
        }

        public static List<String> GetValue(HtmlNode node, string div)
        {
            var nodes = node.Descendants().Where(n => n.Name.StartsWith(div));
           var list = nodes.Select(nodex => nodex.InnerHtml).Select(CleanHtml).ToList();
            return list;
        }

    }
}
