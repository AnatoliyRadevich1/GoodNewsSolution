using GoodNewsTask.Models;
using Microsoft.AspNetCore.Mvc;


namespace GoodNewsTask.Controllers
{
    public class ArticlesFilterController : Controller
    {
        private readonly NewsContext _db; //подключение ArticlesFilterController к БД
        public ArticlesFilterController(NewsContext enteredContext)
        {
            _db = enteredContext;
        }

        [HttpGet]
        public void Filter(Article articleFromArticlesConductorController) //получаем Article из ArticlesConductorController
        {
            RatingArticles ratingArticles = new RatingArticles(); //создаём переменную модели RatingArticles для определения позитивноси новости

            articleFromArticlesConductorController.PositiveLevel = ratingArticles.Filtering(articleFromArticlesConductorController.Text!);
            //определяем уровень позитивности через фильтр слов и устанавливаем уровень позитивности статьи

            //1)определяем слова, задающие рейтинг
            //2)каждой новости на основе каждого слова задаём рейтинг через LINQ
            //3)сортируем новости по критерию Article.PositiveLevel > 0
            //4)отправляем новости в контроллер к подключением к базе данных (скорее всего HomeController)
            if (articleFromArticlesConductorController.PositiveLevel > 0)
            {
                _db.Articles.Add(articleFromArticlesConductorController);
                _db.SaveChanges();//await _db.SaveChangesAsync(); не надо
            }
        }
    }
}
