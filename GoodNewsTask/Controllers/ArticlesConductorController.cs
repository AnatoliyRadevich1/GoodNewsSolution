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
            //1) собираем новости 
            //2) упорядочиваем новость согласно API

            Article downloadedArticle = new Article();
            downloadedArticle.Id = Guid.NewGuid();
            downloadedArticle.Title = "Title from API";
            downloadedArticle.Text = "Text from API";
            downloadedArticle.Source = "Link of source from API";
            downloadedArticle.PublicationDate = DateTime.Now;
            downloadedArticle.PositiveLevel = 0;
            //3) переадресуем новость в контроллер ArticlesFilterController в метод Filter
            //return await Task.Run<IActionResult>(() => RedirectToAction("FilterAsync", "ArticlesFilter", downloadedArticle)); //RedirectToAction не надо так обрабатывать
            return RedirectToAction("Filter", "ArticlesFilter", downloadedArticle);
            //Можно ли куда-нибудь передавать наши данные как API для кого-то ещё?
        }
    }
}
