using Microsoft.AspNetCore.Mvc;
using GoodNewsTask.Models;

namespace GoodNewsTask.Controllers
{
    public class DisplayArticlesController : Controller
    {
        private readonly NewsContext _db;
        public ArticleUser _article_user_model = null!;
        public DisplayArticlesController(NewsContext enteredContext)
        {
            _db = enteredContext;
        }

        [HttpGet]
        public IActionResult ShowArticlesFromDBForGuests()
        {
            _article_user_model.ListUsers = _db.Users.ToList();
            _article_user_model.ListArticles = _db.Articles.ToList();
            return View(_article_user_model);
        }

        [HttpGet]
        public IActionResult ShowArticlesFromDBForRegisteredUsers(string enteredlogin) //User registeredUserInfo
        {
            //пока что предполагаю, что страница будет выглядеть так: ~/нужныйМетод?User.Login=Логин&User.SelectedPositiveLevel=5
            //см. https://metanit.com/sharp/aspnetmvc/2.7.php и https://metanit.com/sharp/aspnet5/8.5.php !!!
            User registeredUserInfo = null!;

            var queryLogin = _db.Users.Where(anyUser => anyUser.Login == enteredlogin);
            foreach (var anyUser in queryLogin)
            {
                if (anyUser.Login == anyUser.Login)
                {
                    registeredUserInfo = anyUser;
                }
            }

            _article_user_model.ListUsers = null!;
            _article_user_model.ListUsers.Add(registeredUserInfo);
            _article_user_model.ListUsers.ToList();
            _article_user_model.ListArticles = _db.Articles.ToList();
            return View(_article_user_model);
            //return Content($"login:{user.Login}  SelectedPositiveLevel: {user.SelectedPositiveLevel}");//переадресация на персонализированную страницу (НАДО ПРОВЕРИТЬ РАБОТОСПОСОБНОСТЬ)
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
