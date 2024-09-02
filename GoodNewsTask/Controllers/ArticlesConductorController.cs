using GoodNewsTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodNewsTask.Controllers
{
    public class ArticlesConductorController : Controller
    {
        [HttpGet]
        public IActionResult HandleIncomeArticles()
        {
            //читай про API https://metanit.com/sharp/aspnet5/23.1.php и https://metanit.com/sharp/aspnet6/2.11.php
            //1) собираем новости (Парсинг) в массив данных (List<Articles>)
            List<Article> gatheredArticles = new List<Article>(); // Список для хранения статей и их передачи во View()

            //2) упорядочиваем новость согласно API (этот кусок вставишь в код из ParsingSolution например для Онлайнера для View (MethodFive), где есть элементы для ArticleModel)
            foreach (var downloadedArticle in gatheredArticles)
            {
                downloadedArticle.Id = Guid.NewGuid();
                downloadedArticle.Title = "Title from API";
                downloadedArticle.Text = "Text from API";
                downloadedArticle.Source = "Link of source from API";
                downloadedArticle.PublicationDate = DateTime.Now;
                downloadedArticle.PositiveLevel = 0;

                gatheredArticles.Add(downloadedArticle);    
            }
            

            //3) переадресуем новость в контроллер ArticlesFilterController в метод Filter
            //return await Task.Run<IActionResult>(() => RedirectToAction("FilterAsync", "ArticlesFilter", downloadedArticle)); //RedirectToAction не надо так обрабатывать
            return RedirectToAction("Filter", "ArticlesFilter", gatheredArticles);
            //Можно ли куда-нибудь передавать наши данные как API для кого-то ещё?
        }
    }
}
