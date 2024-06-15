using GoodNewsTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodNewsTask.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly NewsContext _db;
        public RegistrationController(NewsContext enteredContext)
        {
            _db = enteredContext;
        }
        [HttpGet]
        public IActionResult Registration()
        {
            //await Task.CompletedTask; так не надо делать
            return View(); //писать return await Task.FromResult(View()); не надо 
                           //оформляй View() для регистрации нового пользователя
        }

        [HttpPost]
        public void Registration(string enteredLogin, string enteredPassword, string enteredEmail, int enteredPositiveLevel)
        {
            _db.Users.Add(new User
            {
                Login = enteredLogin,
                Password = enteredPassword,
                Email = enteredEmail,
                RegistrationDate = DateTime.Now,
                SelectedPositiveLevel = enteredPositiveLevel,
                IsBlocked = false
            });
            _db.SaveChanges();
        }
    }
}
