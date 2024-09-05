using GoodNewsTask.Models;
using Microsoft.AspNetCore.Mvc;


namespace GoodNewsTask.Controllers
{
    //ВМЕСТО ЭТОГО КОДА ВСЁ СДЕЛАНО В ArticlesConductorFilterController.cs
    #region Старый уже ненужный код

    //public class ArticlesFilterController : Controller
    //{
    //    private readonly NewsContext _db; //подключение ArticlesFilterController к БД
    //    public ArticlesFilterController(NewsContext enteredContext)
    //    {
    //        _db = enteredContext;
    //    }

    //    [HttpGet]
    //    public void Filter(List<Article> articlesFromArticlesConductorController) //получаем Article из ArticlesConductorController
    //    {

    //        foreach (var filteringArticle in articlesFromArticlesConductorController)
    //        {
    //            RatingArticles ratingArticles = new RatingArticles(); //создаём переменную модели RatingArticles для определения позитивноси новости
    //            filteringArticle.PositiveLevel = ratingArticles.Filtering(filteringArticle.Text!);//определяем позитивнось новости в методе RatingArticles.Filtering()

    //            if (filteringArticle.PositiveLevel>0)//отбираем новости по критерию Article.PositiveLevel > 0
    //            {
    //                _db.Articles.Add(filteringArticle);//отправляем новости в контроллер к подключением к базе данных (скорее всего HomeController)
    //                _db.SaveChanges();//await _db.SaveChangesAsync(); не надо
    //            }
    //        }
    //    }
    //}
    #endregion
}
