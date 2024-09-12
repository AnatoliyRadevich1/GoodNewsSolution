using GoodNewsTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GoodNewsTask.Controllers
{
    public class HomeController : Controller
    {
        #region Исходный код, прописанный по умолчанию
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        //[Route("[controller]/[action]")] //для Swagger-а не делай тут, иначе будет вход на locahost7000:Home/StatusCode/404
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("[controller]/[action]")] //для Swagger-а
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
        #region Обработка страницы для всплывающих ошибок (например, 404)

        [HttpGet]
        [Route("~/StatusCodeError/{enteredStatusCode}")]
        [AllowAnonymous]
        public IActionResult ErrorPage(int enteredStatusCode)
        {
            if (enteredStatusCode == 404)
            {
                ViewData["Message"] = $"Ошибка {enteredStatusCode}. Была осуществлена попытка входа на несуществующую страницу";
                return View();
            }
            else
            {
                ViewData["Message"] = $"Ошибка {enteredStatusCode}";
                return View();
            }
        }
        #endregion
    }
}
