using GoodNewsTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        public IActionResult Create(User enteredUser)
		{
            if (string.IsNullOrEmpty(enteredUser.Login)) 
            {
				ModelState.AddModelError("Login", "Не введён Login");
			}
            foreach (var user in _db.Users)
            {
				if (user.Login == enteredUser.Login)
				{
					ModelState.AddModelError("Login", "Такое имя уже кем-то используется");
				}
			}

			if (string.IsNullOrEmpty(enteredUser.Password))
			{
				ModelState.AddModelError("Password", "Не введён Password");
			}
            if (enteredUser.Email == "admin@mail.com")
            {
                ModelState.AddModelError("Email", "Такой e-mail нельзя использовать");
			}
            foreach (var user in _db.Users) 
            {

                if (user.Email == enteredUser.Email)
                {
					ModelState.AddModelError("Email", "Такой e-mail уже кем-то используется");
				}
            }
			if (string.IsNullOrEmpty(enteredUser.Email))
			{
				ModelState.AddModelError("Email", "Не введён Email");
			}
			if (enteredUser.SelectedPositiveLevel <0 || enteredUser.SelectedPositiveLevel >100)
			{
				ModelState.AddModelError("SelectedPositiveLevel", "Значение должно быть больше ноля (но не более 100)");
			}

			if (ModelState.IsValid)
            {
                _db.Users.Add(enteredUser);
                _db.SaveChanges();
				ViewBag.SuccessRegistrationMessage = "Пользователь зарегистрирован";
			}
            if (ModelState.IsValid ==false)
            {
				ViewBag.SuccessRegistrationMessage = "Пользователь НЕ зарегистрирован";
			}
            return View(enteredUser);
        }
    }
}
