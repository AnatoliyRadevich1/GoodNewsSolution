using GoodNewsTask.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GoodNewsTask.Controllers
{
    public class HomeController : Controller
    {
        #region �������� ���, ����������� �� ���������
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
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
        #region ��������� �������� ��� ����������� ������ (��������, 404)

        [HttpGet]
        [Route("~/StatusCodeError/{enteredStatusCode}")]
        public IActionResult ErrorPage(int enteredStatusCode)
        {
            if (enteredStatusCode == 404)
            {
                ViewData["Message"] = $"������ {enteredStatusCode}. ���� ������������ ������� ����� �� �������������� ��������";
                return View();
            }
            else
            {
                ViewData["Message"] = $"������ {enteredStatusCode}";
                return View();
            }
        }
        #endregion
    }
}
