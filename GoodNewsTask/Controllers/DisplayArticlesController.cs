using Microsoft.AspNetCore.Mvc;
using GoodNewsTask.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GoodNewsTask.Controllers
{
    public class DisplayArticlesController : Controller
    {
        private readonly NewsContext _db;
        public ArticleUser _article_user_model = null!; //скорее всего надо будет удалить
        public DisplayArticlesController(NewsContext enteredContext)
        {
            _db = enteredContext;
        }

        
        [HttpGet]
        public IActionResult ShowArticlesFromDBForGuests()
        {
            //подсказка https://zzzcode.ai/answer-question?id=2aaf2d19-b4d9-40f0-a9b0-0ebbaf119fca
            var articles = _db.Articles.ToList();
            return View(articles);
        }
        

        [HttpGet]
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
        public IActionResult ShowArticlesFromDBForAdmin()
        {
            //логика для администратора:дополнительный вывод таблицы с пользователями. блокировка пользователей
            //return await Task.FromResult(View(await _db.Articles.ToListAsync())); не надо так писать
            //return View(_db.Users.ToList()); //return View(await _db.Articles.ToListAsync()); не надо
            _article_user_model.ListUsers = _db.Users.ToList();
            _article_user_model.ListArticles = _db.Articles.ToList();
            return View(_article_user_model); //создано поле _article_user_model со списками User-ов и Article-ов

        }
    }
}
