using Microsoft.AspNetCore.Mvc;
using GoodNewsTask.Models;
using Microsoft.EntityFrameworkCore;

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

        /*По состоянию на 03.09.2024 есть идея, что тут будет View() с полями "Логин", "Пароль", кнопками "Вход", "Зарегистрироваться", "Восстанновление пароля", а также с 
         полем ввода уровня позитивности.
        Кнопка "Вход" (при верных логине и пароле) будет переадресовывавать на страниу с новостями, отфильтрованными по полю уровня позиивности
        Кнопка "Зарегистрироваться" отправит на View() от RegistrationCOntroller.Create()
        Кнопка "Восстановление пароля" отправит на страницу с полями ввода "Логин", "E-mail", "Пароль","Повтор пароля". При корректных данных после нажатия кнопки отправить 
        будет в БД обновлён пароль, произойдёт переадресация назад во View() данного контроллера AuthentificationController. В противном случае поля очистятся*/

        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
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
        [Route("[controller]/[action]")] //для Swagger-а
        public IActionResult InputLoginPassword([FromForm] string entreredLogin, string enteredPassword) //[FromForm] для получения данных из параметров метода
        {
            var query = _db.Users.ToList();//Получение всех записей из таблицы
            foreach (var user in query)
            {
                if (user.Login == entreredLogin && user.Password == enteredPassword && user.IsBlocked == false)
                {
                    /*По состоянию на 03.09.2024 у тебя есть в TestSolution->TaslAuth код, где реализованы [AllowAnonymous], 
                    [Authorize(Roles="Admin, User")],[Authorize(Roles="Admin")]. Вот прямо вместо этого коммментария можно оттуда добавить тот код*/
                    return RedirectToAction("ShowArticlesFromDBForRegisteredUsers", "DisplayArticlesController", user.Login.ToString());

                    //пока что предполагаю, что страница будет выглядеть так: ~/нужныйМетод?User.Login=Логин&User.SelectedPositiveLevel=5
                    //см. https://metanit.com/sharp/aspnetmvc/2.7.php и https://metanit.com/sharp/aspnet5/8.5.php !!!
                }
                else
                {
                    ViewBag.FailedAuthentificationMessage = "Пользователь заблокирован либо он написал неверную комбинацию логина и пароля";
				}
            }
            //Не надо писать return await Task.Run<IActionResult>(() => RedirectToAction("InputLoginPasswordAsync", "Authentication", "Некорректно введены логин и/или пароль"));
            //https://stackoverflow.com/questions/40968182/return-redirecttoaction-in-mvc-using-async-await RedirectToAction()
            _isCorrectLoginAndPassword = false; //"Некорректно введены логин и/или пароль"
            return RedirectToAction("InputLoginPassword", "Authentication", _isCorrectLoginAndPassword);
        }


		[HttpGet]
		[Route("[controller]/[action]")] //для Swagger-а
		public IActionResult RestorePasswordAndPositiveLevel()
		{
			return View();
		}

		[HttpPost] //Это напоминание про CRUD-операции https://metanit.com/sharp/aspnet5/23.2.php
		[Route("[controller]/[action]")] //для Swagger-а
		public IActionResult RestorePasswordAndPositiveLevel([FromForm] User enteredUser)
		{
            var queryUserFromDB = _db.Users.FirstOrDefault(user => user.Login == enteredUser.Login && user.Email == enteredUser.Email);//Поиск пользователя по логину и email
            if (queryUserFromDB != null)
            {
                queryUserFromDB.Password = enteredUser.Password;
                queryUserFromDB.SelectedPositiveLevel = enteredUser.SelectedPositiveLevel;
				//_db.Users.Update(queryUserFromDB); писать не надо, т.к. _db.SaveChanges(); самостоятельно это делает благодаря механизмам Entity Framework
				_db.SaveChanges();
                ViewBag.SuccessRegistrationMessage = "Пароль и уровень позитивности успешно обновлены.";
        }
            else
            {
                ViewBag.SuccessRegistrationMessage = "Пользователь не найден или введены некорректные данные.";
            }
            return RedirectToAction("InputLoginPassword", "Authentication", _isCorrectLoginAndPassword);


        }

	}
}
