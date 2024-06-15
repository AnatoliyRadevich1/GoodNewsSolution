using Microsoft.AspNetCore.Mvc;
using GoodNewsTask.Models;

namespace GoodNewsTask.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly NewsContext _db;
        bool _isCorrectLoginAndPassword = true;
        public AuthenticationController(NewsContext enteredContext)
        {
            _db = enteredContext;
        }

        [HttpGet]
        public IActionResult InputLoginPassword()
        {
            return View();
        }
        #region Это лишнее, т.к. [FromForm] однозначно определяет источник параметро метода
        //[HttpPost]
        //public IActionResult InputLoginPassword([FromQuery] string entreredLogin, string enteredPassword)
        //{
        //    return RedirectToAction("~/Home/ErrorPage.cshtml");//если пользователь решил через браузерную строку подбирать логины и пароли, то его отправляем в 404
        //}
        #endregion
        [HttpPost]
        public IActionResult InputLoginPassword([FromForm] string entreredLogin, string enteredPassword) //[FromForm] для получения данных из параметров метода
        {
            var query = _db.Users.ToList();//Получение всех записей из таблицы
            foreach (var user in query)
            {
                if (user.Login == entreredLogin && user.Password == enteredPassword)
                {
                    return RedirectToAction("ShowArticlesFromDBForRegisteredUsers", "DisplayArticlesController", user.Login.ToString());

                    //пока что предполагаю, что страница будет выглядеть так: ~/нужныйМетод?User.Login=Логин&User.SelectedPositiveLevel=5
                    //см. https://metanit.com/sharp/aspnetmvc/2.7.php и https://metanit.com/sharp/aspnet5/8.5.php !!!
                }
            }
            //Не надо писать return await Task.Run<IActionResult>(() => RedirectToAction("InputLoginPasswordAsync", "Authentication", "Некорректно введены логин и/или пароль"));
            //https://stackoverflow.com/questions/40968182/return-redirecttoaction-in-mvc-using-async-await RedirectToAction()
            _isCorrectLoginAndPassword = false; //"Некорректно введены логин и/или пароль"
            return RedirectToAction("InputLoginPassword", "Authentication", _isCorrectLoginAndPassword);
        }
    }
}
