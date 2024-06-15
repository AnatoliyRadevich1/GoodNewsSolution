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

        public void Filter(Article articleFromArticlesConductorController) //public async Task FilterAsync(Article articleFromArticlesConductorController) не надо
        {
            Article selectedArticle = new Article();
            //1)определяем слова, задающие рейтинг
            #region Набор подстрок позитивного значения
            string positiveSubstring01 = "мир";
            string positiveSubstring02 = "салют";
            string positiveSubstring03 = "радост";
            string positiveSubstring04 = "забав";
            string positiveSubstring05 = "торжеств";
            string positiveSubstring06 = "праздник";
            string positiveSubstring07 = "спас";
            string positiveSubstring08 = "родил";
            string positiveSubstring09 = "жив";
            string positiveSubstring10 = "масленица";
            string positiveSubstring11 = "гуляни";
            string positiveSubstring12 = "парк";
            string positiveSubstring13 = "великолепн";
            string positiveSubstring14 = "зоопарк";
            string positiveSubstring15 = "цирк";
            #endregion
            #region Набор подстрок негативного значения
            string negativeSubstring01 = "смерт";
            string negativeSubstring02 = "погиб";
            string negativeSubstring03 = "ранен";
            string negativeSubstring04 = "бедств";
            string negativeSubstring05 = "коррупц";
            string negativeSubstring06 = "скандал";
            string negativeSubstring07 = "взятк";
            string negativeSubstring08 = "мошен";
            string negativeSubstring09 = "доставле";
            string negativeSubstring10 = "больниц";
            string negativeSubstring11 = "кровь";
            string negativeSubstring12 = "тяжелом";
            string negativeSubstring13 = "милиц";
            string negativeSubstring14 = "полиц";
            string negativeSubstring15 = "пожар";
            #endregion
            //2)каждой новости на основе каждого слова задаём рейтинг через LINQ
            #region Это лишнее
            //логическое ИЛИ нельзя использовать в методе Contains, поэтому приходится так делать
            //var positiveQuery01 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring01)).ToString();
            //var positiveQuery02 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring02)).ToString();
            //var positiveQuery03 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring03)).ToString();
            //var positiveQuery04 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring04)).ToString();
            //var positiveQuery05 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring05)).ToString();
            //var positiveQuery06 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring06)).ToString();
            //var positiveQuery07 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring07)).ToString();
            //var positiveQuery08 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring08)).ToString();
            //var positiveQuery09 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring09)).ToString();
            //var positiveQuery10 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring10)).ToString();
            //var positiveQuery11 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring11)).ToString();
            //var positiveQuery12 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring12)).ToString();
            //var positiveQuery13 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring13)).ToString();
            //var positiveQuery14 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring14)).ToString();
            //var positiveQuery15 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(positiveSubstring15)).ToString();

            //var negativeQuery01 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring01)).ToString();
            //var negativeQuery02 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring02)).ToString();
            //var negativeQuery03 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring03)).ToString();
            //var negativeQuery04 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring04)).ToString();
            //var negativeQuery05 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring05)).ToString();
            //var negativeQuery06 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring06)).ToString();
            //var negativeQuery07 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring07)).ToString();
            //var negativeQuery08 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring08)).ToString();
            //var negativeQuery09 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring09)).ToString();
            //var negativeQuery10 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring10)).ToString();
            //var negativeQuery11 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring11)).ToString();
            //var negativeQuery12 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring12)).ToString();
            //var negativeQuery13 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring13)).ToString();
            //var negativeQuery14 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring14)).ToString();
            //var negativeQuery15 = articleFromArticlesConductorController.Text.Split(' ').Where(word => word.Contains(negativeSubstring15)).ToString();
            #endregion
            foreach (var word in articleFromArticlesConductorController.Text!.Split(' '))
            {
                if (word == positiveSubstring01 || word == positiveSubstring02 || word == positiveSubstring03 || word == positiveSubstring04 || word == positiveSubstring05 ||
                    word == positiveSubstring06 || word == positiveSubstring07 || word == positiveSubstring08 || word == positiveSubstring09 || word == positiveSubstring10 ||
                    word == positiveSubstring11 || word == positiveSubstring12 || word == positiveSubstring13 || word == positiveSubstring14 || word == positiveSubstring15)
                {
                    articleFromArticlesConductorController.PositiveLevel++;
                }
                if (word == negativeSubstring01 || word == negativeSubstring02 || word == negativeSubstring03 || word == negativeSubstring04 || word == negativeSubstring05 ||
                    word == negativeSubstring06 || word == negativeSubstring07 || word == negativeSubstring08 || word == negativeSubstring09 || word == negativeSubstring10 ||
                    word == negativeSubstring11 || word == negativeSubstring12 || word == negativeSubstring13 || word == negativeSubstring14 || word == negativeSubstring15)
                {
                    articleFromArticlesConductorController.PositiveLevel--;
                }
            }

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
