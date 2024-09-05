using HtmlAgilityPack;
using System.Xml;

namespace GoodNewsTask.Models
{
    public class ParserForLenta
    {
        //подсказка https://zzzcode.ai/answer-question?id=b92d93d6-a3ff-400e-b143-10ade8ab8994
        //подсказка https://zzzcode.ai/answer-question?id=113e08cc-c55c-49e8-849c-9bdac0321787 (с устаревшим new System.Net.WebClient())
        //подсказка https://zzzcode.ai/answer-question?id=4bac0fff-1ef0-432f-9322-171074dfda5c

        private static readonly HttpClient httpClient = new HttpClient();
        private string DownloadString(string url)
        {
            // Используем HttpClient для загрузки содержимого
            var response = httpClient.GetAsync(url).Result; // Синхронный вызов
            response.EnsureSuccessStatusCode(); // Проверяем успешность ответа
            return response.Content.ReadAsStringAsync().Result; // Синхронное чтение содержимого
        }

        private string GetTextFromArticle(string articleUrl)
        {
            // Загружаем HTML-код статьи
            string htmlContent = DownloadString(articleUrl);

            // Создаем HtmlDocument для парсинга HTML
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // Извлекаем текст из тегов <p>
            var paragraphs = htmlDoc.DocumentNode.SelectNodes("//p");
            if (paragraphs != null)
            {
                return string.Join(" ", paragraphs.Select(p => p.InnerText.Trim()));
            }

            return string.Empty; // Возвращаем пустую строку, если нет тегов <p>
        }
        public List<Article> ParseXmlForLinks(string url)
        {
            var articles = new List<Article>();
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(url);

            foreach (XmlNode item in xmlDoc.SelectNodes("//item")!)
            {
                var article = new Article
                {
                    Title = item["link"]!.InnerText,
                    Text = GetTextFromArticle(item["link"]!.InnerText),
                    Source = item["enclosure"]?.Attributes["url"]?.Value,//тэг <enclosure> с параметром url
                };
                articles.Add(article);
            }

            return articles;
        }

    }
}
