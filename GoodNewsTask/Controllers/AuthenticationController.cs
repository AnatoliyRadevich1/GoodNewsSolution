using Microsoft.AspNetCore.Mvc;
using GoodNewsTask.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace GoodNewsTask.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly NewsContext _db;
        bool isAuthenticate = false;
        bool _isCorrectLoginAndPassword = true;
        public AuthenticationController(NewsContext enteredContext)
        {
            _db = enteredContext;
        }


        //РАБОТАЕТ КОРРЕКТНО!!!
        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
        [AllowAnonymous]
        public IActionResult InputLoginPassword()
        {
            return View();
        }
        //РАБОТАЕТ КОРРЕКТНО!!!
        [HttpPost]
        [Route("[controller]/[action]")] //для Swagger-а
        //[AllowAnonymous]
        public IActionResult InputLoginPassword([FromForm] User enteredUser)
        {
            var queryUserFromDB = _db.Users.FirstOrDefault(user => user.Login == enteredUser.Login && user.Password == enteredUser.Password);//Поиск пользователя по логину и паролю
            
            if (queryUserFromDB == null) /*Обработка исключений при неправильном логине либо пароле*/
            {
                ViewBag.FailedAuthentificationMessage = "Пользователь заблокирован либо он написал неверную комбинацию логина и пароля";
                return View("InputLoginPassword", enteredUser); // Возвращаем представление с моделью
            }

            #region Вставка для входа админом
            ClaimsIdentity identity = null!;

            if (queryUserFromDB!.Login == "Admin" && queryUserFromDB.Password == "1000" && queryUserFromDB.IsBlocked == false)
            {
                isAuthenticate = false;
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, queryUserFromDB.Login), //Была использована библиотека using System.Security.Claims;
                    new Claim(ClaimTypes.Role,"Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                isAuthenticate = true;
                _isCorrectLoginAndPassword = true;
                var principal = new ClaimsPrincipal(identity);
                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("ShowArticlesFromDBForAdmin", "Display"/*, new { userId = queryUserFromDB.Id }*/);
            }
            #endregion
            if (queryUserFromDB != null && queryUserFromDB.Login !="Admin" && queryUserFromDB.IsBlocked == false)
            {
                #region Вставка для входа user-ом
                identity = null!;
                isAuthenticate = false;
                identity = new ClaimsIdentity(new[]
{
                    new Claim(ClaimTypes.Name, queryUserFromDB.Login!), //Была использована библиотека using System.Security.Claims;
                    new Claim(ClaimTypes.Role,"User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                isAuthenticate = true;
                _isCorrectLoginAndPassword = true;
                var principal = new ClaimsPrincipal(identity);
                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("UserId", queryUserFromDB.Id.ToString()); //СЕССИЯ, НО ЧЕРЕЗ ID ПОЛЬЗОВАТЕЛЯ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return RedirectToAction("ShowArticlesFromDBForRegisteredUsers", "Display");
                //return RedirectToAction("ShowArticlesFromDBForRegisteredUsers", "Display", new { userId = queryUserFromDB.Id }); //Если так оставить, то в браузерной строке пропишется userId
                #endregion
                //return RedirectToAction("ShowArticlesFromDBForRegisteredUsers", "Display", new { enteredPositiveLevel = queryUserFromDB.SelectedPositiveLevel });
                // подсказка с передачей переменной https://zzzcode.ai/answer-question?id=8999dede-0adf-4a8b-afcb-f0d969178b35

                #region Второй способ реализации (корректно работающий код)
                //var selectedPositiveLevelOfUser = _db.Users.Where(u => u.Login == enteredUser.Login && u.Password == enteredUser.Password)
                //                           .Select(u => u.SelectedPositiveLevel)
                //                           .FirstOrDefault();
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine($"selectedPositiveLevelOfUser = {selectedPositiveLevelOfUser}");
                //Console.ResetColor();
                //return RedirectToAction("ShowArticlesFromDBForRegisteredUsers", "Display", new { enteredPositiveLevel = queryUserFromDB.SelectedPositiveLevel });
                #endregion

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

        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
        //[Authorize(Roles = "Admin,User")] //Если это оставить, то гостей будет перекидывать на страницу 404
        public async Task<IActionResult> Logout() //рекомендуемый асинхронный метод
        {
            var userRole  = HttpContext.User;
            if (!userRole.IsInRole("Admin") && !userRole.IsInRole("User"))
            {
                return RedirectToAction("InputLoginPassword", "Authentication");
            }
            else
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear(); //ОЧИСТКА СЕССИИ (УДАЛЯЕМ ВСЕ ДАННЫЕ) !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return RedirectToAction("InputLoginPassword", "Authentication");
            }
            
        }

        //РАБОТАЕТ КОРРЕКТНО!!!
		[HttpGet]
		[Route("[controller]/[action]")] //для Swagger-а
        //[AllowAnonymous]
        public IActionResult RestorePasswordAndPositiveLevel()
		{
			return View();
		}

		[HttpPost] //Это напоминание про CRUD-операции https://metanit.com/sharp/aspnet5/23.2.php
		[Route("[controller]/[action]")] //для Swagger-а
        [AllowAnonymous]
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
