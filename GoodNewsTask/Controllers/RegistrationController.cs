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

        [AcceptVerbs("Get", "Post")] //ЭТО НАДО БУДЕТ СПРОСИТЬ КАК ДЕЛАТЬ ПРОВЕРКУ ПО СПИСКУ ПОЧОВЫХ ЯЩИКОВ (см. https://metanit.com/sharp/aspnetmvc/9.3.php )
        public IActionResult CheckEmail(string enteredEmail)
        {
            if (enteredEmail == "admin@mail.com")
            {
                return Json(false); //СПРОСИТЬ. Я очень сомневаюсь, что это нужно. Может надо return Context("Такой эл. ящик уже занят"); 
            }
            return Json(true);//СПРОСИТЬ. Вроде бы логично. Я не знаю, правильно ли так делать? Но мне надо вернуть из этого метода enteredEmail. 

        }

        [HttpGet]
        public IActionResult Create()
        {
            //await Task.CompletedTask; так не надо делать
            return View(); //писать return await Task.FromResult(View()); не надо 
                           //оформляй View() для регистрации нового пользователя
        }

        [HttpPost]
        public void Create(User enteredUser) //ТАК МОЖНО (void, а не IActionResult)?
        {
            _db.Users.Add(enteredUser);
            _db.SaveChanges();
        }
        #region Это старый вариант
        //public void Create(string enteredLogin, string enteredPassword, string enteredEmail, int enteredPositiveLevel)
        //{
        //    _db.Users.Add(new User
        //    {
        //        Login = enteredLogin,
        //        Password = enteredPassword,
        //        Email = enteredEmail,
        //        RegistrationDate = DateTime.Now,
        //        SelectedPositiveLevel = enteredPositiveLevel,
        //        IsBlocked = false
        //    });
        //    _db.SaveChanges();
        //}
        #endregion
    }
}
