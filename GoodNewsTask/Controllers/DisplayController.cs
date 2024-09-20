using Microsoft.AspNetCore.Mvc;
using GoodNewsTask.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Hangfire;

namespace GoodNewsTask.Controllers
{
    public class DisplayController : Controller
    {
        private readonly NewsContext _db;
        public ArticleUser _article_user_model = null!; //скорее всего надо будет удалить

        private const int PageSize = 10;//для пагинации (кол-во статей на странице)

        public DisplayController(NewsContext enteredContext)
        {
            _db = enteredContext;
        }

        #region Код для HangFire
        [HttpGet]
        [Route("[controller]/[action]")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestHangFireMethod()//при входе сюда запускается цикличное действие
        {
            RecurringJob.AddOrUpdate("myrecurringjob", () => Console.WriteLine($"Количество пользователей = {_db.Users.Count()}, количество новостных статей = {_db.Articles.Count()}"), Cron.Minutely);

            return View();
        }
        #endregion

        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
        [AllowAnonymous]
        public IActionResult ShowArticlesFromDBForGuests(int pageNumber = 1)
        {
            #region Старый работающий код без пагинации при условии, что у метода не будет параметров
            //подсказка https://zzzcode.ai/answer-question?id=2aaf2d19-b4d9-40f0-a9b0-0ebbaf119fca
            //var articles = _db.Articles.ToList();
            //return View(articles);
            #endregion
            #region Новый работающий код с пагинацией
            var totalArticles = _db.Articles.Count();
            var articles = _db.Articles.Skip((pageNumber - 1) * PageSize)
                                       .Take(PageSize)
                                       .ToList();
            ArticleListViewModel articleViewModel = new ArticleListViewModel()
            {
                Articles = articles,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalArticles / PageSize)
            };
            return View(articleViewModel);
            #endregion
        }

        
        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
        [Authorize(Roles = "User")]
        public IActionResult ShowArticlesFromDBForRegisteredUsers(/*Guid userId, */int pageNumber = 1, string searchElement = "")
        {
            var userIdString = HttpContext.Session.GetString("UserId"); //реализация из сессии id пользователя
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                ViewData["UserNotFoundMessage"] = "Нет такого пользователя";
                return RedirectToAction("InputLoginPassword", "Authentication"); //закрыл баг при отсутствующем id
            }

            var selectedUser = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (selectedUser == null)//До такого программа не пустит, а пользователь вполне себе может такое сделать
            {
                ViewData["UserNotFoundMessage"] = "Нет такого пользователя";
                return RedirectToAction("InputLoginPassword", "Authentication"); //закрыл баг при отсутствующем id
            }

            var articles = _db.Articles.Where(a => a.PositiveLevel >= selectedUser.SelectedPositiveLevel).ToList();// Получаем статьи, соответствующие уровню позитива пользователя
            if (!string.IsNullOrEmpty(searchElement))//для поиска данных по введённому пользователем тексту
            {
                articles = articles.Where(elem => elem.Title!.Contains(searchElement, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            //var totalArticles = _db.Articles.Count(); //тут общее количество ВСЕХ новостных статей из БД посчиано
            var totalArticles = articles.Count();
            var paginatedArticles = articles.Skip((pageNumber - 1) * PageSize)
                                            .Take(PageSize)
                                            .ToList();


            ArticleUserDTO articleUserDTO = new ArticleUserDTO()
            { 
                ListArticles = paginatedArticles,
                ListUsers = new List<User> { selectedUser },
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalArticles / PageSize)
                //SearchElement = searchElement
            };
            return View(articleUserDTO);
        }



        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
        [Authorize(Roles = "Admin")]
        public IActionResult ShowArticlesFromDBForAdmin()
        {
            List<Article> articles = _db.Articles.ToList(); //выбираем из БД все новости
            return View(articles);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ShowArticlesFromDBForAdmin(Guid id)//ИМЯ КАК В ASP-ROUTE-ID
        {
            //var articleFromDB = _db.Articles.FirstOrDefault(a => a.Id == articleId);
            var articleFromDB = _db.Articles.Find(id);
            if (articleFromDB != null)
            {
                _db.Articles.Remove(articleFromDB); // Удаляем выбранную статью
                _db.SaveChanges(); // Сохраняем изменения в базе данных
            }
            return RedirectToAction("ShowArticlesFromDBForAdmin", "Display");
 
        }

        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
        [Authorize(Roles = "Admin")]
        public IActionResult ShowUsersFromDBForAdmin(string searchUser = "")
        {

            List<User> users = _db.Users.ToList(); //выбираем из БД всех пользователей

            if (!string.IsNullOrEmpty(searchUser))//для поиска данных по введённому пользователем тексту
            {
                users = users.Where(elem => elem.Login!.Contains(searchUser, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!users.Any())
            {
                users = new List<User>();
            }
            return View(users);
        }

        [HttpPost]
        [Route("[controller]/[action]")] //для Swagger-а
        [Authorize(Roles = "Admin")]
        public IActionResult ShowUsersFromDBForAdmin(Guid userId)
        {

            var userFromDB = _db.Users.Find(userId);
            if (userFromDB != null)
            {
                userFromDB.IsBlocked = !userFromDB.IsBlocked; // Переключаем статус блокировки
                _db.SaveChanges(); // Сохраняем изменения в базе данных
            }
            return RedirectToAction("ShowUsersFromDBForAdmin", "Display");
        }
    }
}
