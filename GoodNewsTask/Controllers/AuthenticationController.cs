using Microsoft.AspNetCore.Mvc;
using GoodNewsTask.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

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


        //РАБОТАЕТ КОРРЕКТНО!!!
        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
        public IActionResult InputLoginPassword()
        {
            return View();
        }
        //РАБОТАЕТ КОРРЕКТНО!!! НО СЮДА НАДО БУДЕТ ДОБАВИТЬ РАБОТУ С АДМИНИСТРАТОРОМ
        [HttpPost]
        [Route("[controller]/[action]")] //для Swagger-а
        public IActionResult InputLoginPassword([FromForm] User enteredUser)
        {
            var queryUserFromDB = _db.Users.FirstOrDefault(user => user.Login == enteredUser.Login && user.Password == enteredUser.Password);//Поиск пользователя по логину и паролю
            
            if (queryUserFromDB != null && queryUserFromDB.IsBlocked == false)
            {
                _isCorrectLoginAndPassword = true;
                return RedirectToAction("ShowArticlesFromDBForRegisteredUsers", "DisplayArticles", new { userId = queryUserFromDB.Id });
                //return RedirectToAction("ShowArticlesFromDBForRegisteredUsers", "DisplayArticles", new { enteredPositiveLevel = queryUserFromDB.SelectedPositiveLevel });


                // подсказка с передачей переменной https://zzzcode.ai/answer-question?id=8999dede-0adf-4a8b-afcb-f0d969178b35

                #region Второй способ реализации (корректно работающий код)
                //var selectedPositiveLevelOfUser = _db.Users.Where(u => u.Login == enteredUser.Login && u.Password == enteredUser.Password)
                //                           .Select(u => u.SelectedPositiveLevel)
                //                           .FirstOrDefault();
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine($"selectedPositiveLevelOfUser = {selectedPositiveLevelOfUser}");
                //Console.ResetColor();
                //return RedirectToAction("ShowArticlesFromDBForRegisteredUsers", "DisplayArticles", new { enteredPositiveLevel = queryUserFromDB.SelectedPositiveLevel });
                #endregion
                /*По состоянию на 03.09.2024 у тебя есть в TestSolution->TaslAuth код, где реализованы [AllowAnonymous], 
                [Authorize(Roles="Admin, User")],[Authorize(Roles="Admin")]. Вот прямо вместо этого коммментария можно оттуда добавить тот код*/
                //см. https://metanit.com/sharp/aspnetmvc/2.7.php и https://metanit.com/sharp/aspnet5/8.5.php !!!
            }
            else
            {
                ViewBag.FailedAuthentificationMessage = "Пользователь заблокирован либо он написал неверную комбинацию логина и пароля";
                return View("InputLoginPassword", enteredUser); // Возвращаем представление с моделью
            }

            //Не надо писать return await Task.Run<IActionResult>(() => RedirectToAction("InputLoginPasswordAsync", "Authentication", "Некорректно введены логин и/или пароль"));
            //https://stackoverflow.com/questions/40968182/return-redirecttoaction-in-mvc-using-async-await RedirectToAction()
        }

        //РАБОТАЕТ КОРРЕКТНО!!!
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
				queryUserFromDB.ConfirmPassword = enteredUser.ConfirmPassword;
				queryUserFromDB.SelectedPositiveLevel = enteredUser.SelectedPositiveLevel;
				//_db.Users.Update(queryUserFromDB); писать не надо, т.к. _db.SaveChanges(); самостоятельно это делает благодаря механизмам Entity Framework
				_db.SaveChanges();
                ViewBag.SuccessRegistrationMessage = "Пароль и уровень позитивности успешно обновлены.";//это просто не успевает отобразиться
        }
            else
            {
                ViewBag.SuccessRegistrationMessage = "Пользователь не найден или введены некорректные данные.";//это просто не успевает отобразиться

            }
            return RedirectToAction("InputLoginPassword", "Authentication", _isCorrectLoginAndPassword);


        }

	}
}
