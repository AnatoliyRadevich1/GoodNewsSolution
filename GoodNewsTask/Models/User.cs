using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GoodNewsTask.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required (ErrorMessage = "Не указан Login")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 20 символов")]
        public string? Login { get; set; }

        [Required (ErrorMessage = "Не указан пароль")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 20 символов")]
        public string? Password { get; set; }

        [Required (ErrorMessage = "Не подтверждён пароль")]
        [Compare("Password", ErrorMessage = "Введённые данные не совпадают с паролем")]
        public string? ConfirmPassword { get; set; }

        //[Remote(action: "CheckEmail", controller: "Registration", ErrorMessage = "Такой адрес уже используется")] надо СПРОСИТЬ почему не работает
        [Required (ErrorMessage = "Не указан e-mail")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string? Email { get; set; }
 
        [Required (ErrorMessage = "Не указан уровень позитивности новостей")]
        [Range(0,100, ErrorMessage = "Значение должно быть больше ноля (но не более 100)")]
        public int SelectedPositiveLevel { get; set; }

		public DateTime RegistrationDate { get; set; }

		public bool IsBlocked { get; set; }

        public User()
        {
            RegistrationDate = DateTime.Now;
            IsBlocked = false;
		}
    }
}
