using Microsoft.AspNetCore.Mvc;
using GoodNewsTask.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authorization;

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

        #region Тестовый код с пагинацией
        [Route("[controller]/[action]")] //для Swagger-а
        [AllowAnonymous]
        public IActionResult ShowArticlesFromDBForGuestsWithPagination(int pageNumber = 1)
        {
            var totalArticles = _db.Articles.Count();
            var articles = _db.Articles.Skip((pageNumber - 1)*PageSize)
                                       .Take(PageSize)
                                       .ToList();
            ArticleListViewModel articleViewModel = new ArticleListViewModel()
            {
                Articles = articles,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalArticles / PageSize)
            };
            return View(articleViewModel);
        }
        #endregion



        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
        [AllowAnonymous]
        public IActionResult ShowArticlesFromDBForGuests()
        {
            //подсказка https://zzzcode.ai/answer-question?id=2aaf2d19-b4d9-40f0-a9b0-0ebbaf119fca
            var articles = _db.Articles.ToList();
            return View(articles);
        }

        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
        [Authorize(Roles = "User")]
        public IActionResult ShowArticlesFromDBForRegisteredUsers(Guid userId) //User registeredUserInfo
        {
            //см. https://metanit.com/sharp/aspnetmvc/2.7.php и https://metanit.com/sharp/aspnet5/8.5.php !!!
            //так делай https://zzzcode.ai/answer-question?id=fbc336bc-f05c-4c47-aca7-279f95749e70 !!!
            var selectedUser = _db.Users.FirstOrDefault(u => u.Id == userId);
            
            if (selectedUser == null)//До такого программа не пустит
            {
                ViewData["UserNotFoundMessage"] = "Нет такого пользователя";
                return View();
            }
            
            var articles = _db.Articles.Where(a => a.PositiveLevel >= selectedUser.SelectedPositiveLevel).ToList();// Получаем статьи, соответствующие уровню позитива пользователя

            ArticleUser articleUserModel = new ArticleUser // Создаем модель для передачи в представление
            {
                ListArticles = articles,
                ListUsers = new List<User> { selectedUser } // Передаем только текущего пользователя
            };
            return View(articleUserModel);

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
        public IActionResult ShowUsersFromDBForAdmin()
        {
            List<User> users = _db.Users.ToList(); //выбираем из БД все новости
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
