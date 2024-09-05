using HtmlAgilityPack;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

namespace GoodNewsTask.Models
{
    public class ParserForOnliner
    {
        private string ExtractImageUrl(string description)
        {
            // Регулярное выражение для поиска URL изображений (подсказка https://zzzcode.ai/answer-question?id=d02b6be9-edd0-449c-83f3-7695aceafb26 )
            string pattern = @"<img[^>]+src=""([^""]+)""";
            Match match = Regex.Match(description, pattern);
            return match.Success ? match.Groups[1].Value : string.Empty; // Возвращаем URL изображения или пустую строку
        }

        public List<Article> ParseXmlForLinks(string url)
        {
            List<Article> articles = new List<Article>(); // Список для хранения статей и их передачи во View()

            try
            {
                XmlReader reader = XmlReader.Create(url);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                if (feed != null)
                {
                    foreach (SyndicationItem item in feed.Items)
                    {
                        foreach (SyndicationLink link in item.Links)
                        {
                            //извлечение текста из сайта
                            if (link.Uri != null && !link.Uri.ToString().Contains("/go/"))//ссылка не пустая и не содержит в себе "/go/"
                            {
                                HtmlWeb web = new HtmlWeb();
                                HtmlDocument doc = web.Load(link.Uri.AbsoluteUri);
                                HtmlNodeCollection paragraphs = doc.DocumentNode.SelectNodes("//p");

                                if (paragraphs != null)
                                {
                                    string textCollector = "";
                                    foreach (HtmlNode paragraph in paragraphs)
                                    {
                                        textCollector += paragraph.InnerText;//собираем весь текст из тегов <p></p>
                                    }
                                    // Создаем объект ArticleViewModel и добавляем его в список
                                    articles.Add(new Article
                                    {
                                        Title = link.Uri.AbsoluteUri,
                                        Text = textCollector,
                                        Source = ExtractImageUrl(item.Summary.Text)// Извлечение URL изображения
                                    });
                                    // Здесь можно добавить логику для работы с рейтингом, если это необходимо, но не нужно, т.к. эта логика будет в другом контроллере
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Не удалось загрузить данные из XML.");
                }
            }
            catch (Exception? ex)
            {
                throw new Exception("Ошибка: " + ex.Message);
            }
            return articles; // Возвращаем список статей
        }
    }
}
