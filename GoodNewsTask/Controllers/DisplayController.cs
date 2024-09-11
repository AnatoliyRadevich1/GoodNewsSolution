using Microsoft.AspNetCore.Mvc;
using GoodNewsTask.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GoodNewsTask.Controllers
{
    public class DisplayController : Controller
    {
        private readonly NewsContext _db;
        public ArticleUser _article_user_model = null!; //скорее всего надо будет удалить
        public DisplayController(NewsContext enteredContext)
        {
            _db = enteredContext;
        }

        
        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
        public IActionResult ShowArticlesFromDBForGuests()
        {
            //подсказка https://zzzcode.ai/answer-question?id=2aaf2d19-b4d9-40f0-a9b0-0ebbaf119fca
            var articles = _db.Articles.ToList();
            return View(articles);
        }
        

        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
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
        public IActionResult ShowArticlesFromDBForAdmin()
        {
            List<Article> articles = _db.Articles.ToList(); //выбираем из БД все новости
            return View(articles);
        }

        [HttpPost]
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
        public IActionResult ShowUsersFromDBForAdmin()
        {
            List<User> users = _db.Users.ToList(); //выбираем из БД все новости
            return View(users);
        }

        [HttpPost]
        [Route("[controller]/[action]")] //для Swagger-а
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
