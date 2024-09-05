using HtmlAgilityPack;
using System.Xml;

namespace GoodNewsTask.Models
{
    public class ParserForSputnik
    {
        //подсказка https://zzzcode.ai/answer-question?id=613163c0-c085-4583-920f-de0e42267c54
        //подсказка https://zzzcode.ai/answer-question?id=3007524c-6708-4989-84e9-0df992dcb670
        //подсказка https://zzzcode.ai/answer-question?id=0cbf133a-e292-4b11-a3e2-64641d4584fd
        private string GetTextFromArticle(string url)
        {
            var web = new HtmlWeb();
            var document = web.Load(url);
            var textNodes = document.DocumentNode.SelectNodes("//div[@class='article__text']");

            if (textNodes != null)
            {
                return string.Join(Environment.NewLine, textNodes.Select(node => node.InnerText.Trim()));
            }
            return string.Empty;
        }

        private string GetPictureFromArticle(string url)
        {
            var web = new HtmlWeb();
            var document = web.Load(url);
            var imageNode = document.DocumentNode.SelectSingleNode("//img[@media-type='ar16x9']");
            return imageNode != null ? imageNode.GetAttributeValue("src", string.Empty) : string.Empty;
        }
        public List<Article> ParseXmlForLinks(string url)
        {
            var articles = new List<Article>();
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(url);

            // Определяем пространство имен
            var namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("ns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            namespaceManager.AddNamespace("news", "http://www.google.com/schemas/sitemap-news/0.9");

            // Извлекаем узлы с учетом пространства имен
            foreach (XmlNode item in xmlDoc.SelectNodes("//ns:url", namespaceManager)!)
            {
                var article = new Article
                {
                    Title = item.SelectSingleNode("ns:loc", namespaceManager)?.InnerText,
                    Text = GetTextFromArticle(item.SelectSingleNode("ns:loc", namespaceManager)?.InnerText!),
                    Source = GetPictureFromArticle(item.SelectSingleNode("ns:loc", namespaceManager)?.InnerText!)
                };
                articles.Add(article);
            }
            return articles;
        }
    }
}
