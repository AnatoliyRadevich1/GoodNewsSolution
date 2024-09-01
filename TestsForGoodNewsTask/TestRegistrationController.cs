using GoodNewsTask.Controllers;
using GoodNewsTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace TestsForGoodNewsTask
{
    //https://learn.microsoft.com/ru-ru/visualstudio/ide/how-to-add-or-remove-references-by-using-the-reference-manager?view=vs-2022 - ссылка на проект
    public class TestRegistrationController
    {
        [Fact]//атрибут, сообщающий нам о том, что метод €вл€етс€ тестовым, который нам пока нужен в данном тесте
        //[Theory] //представл€ет набор тестов, которые выполн€ют один и тот же код, но имеют разные входные аргументы
        //[InlineData(3, (-2), "1")] //задает значени€ дл€ этих входных данных
        //[InlineData(3, 3, "6")] //задает значени€ дл€ этих входных данных
        //[InlineData(3, 0, "3")] //задает значени€ дл€ этих входных данных
        public void TestCreate()
        {
            //arrange - исходные данные
            User testUser = new User();
            testUser.Id = Guid.NewGuid();
            testUser.Login = "TestLogin";
            testUser.Password = "TestPassword";
            testUser.ConfirmPassword = "TestPassword";
            testUser.Email = "testUser@testmail.com";
            testUser.SelectedPositiveLevel = 5;
            testUser.RegistrationDate = DateTime.Now;
            testUser.IsBlocked = false;

            //act - запускаемые действи€
            if (string.IsNullOrEmpty(testUser.Login))
            {
                Assert.Equal(testUser.Login, "TestLogin"); //assert - результат тестировани€
            }

            if (string.IsNullOrEmpty(testUser.Password))
            {
                Assert.Equal(testUser.Password, "TestPassword"); //assert - результат тестировани€
            }
            if (testUser.Email == "testUser@testmail.com")
            {
                Assert.Equal(testUser.Email, "testUser@testmail.com"); //assert - результат тестировани€
            }

            if (testUser.SelectedPositiveLevel > 0 || testUser.SelectedPositiveLevel < 100)
            {
                Assert.Equal(testUser.SelectedPositiveLevel, 5); //assert - результат тестировани€
            }

            if (testUser.IsBlocked == false)
            {
                Assert.Equal(testUser.IsBlocked, false); //assert - результат тестировани€
            }

            //assert - результат тестировани€
            Assert.Equal(testUser.Password, testUser.ConfirmPassword);
        }
    }
}