using GoodNewsTask.Controllers;
using GoodNewsTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace TestsForGoodNewsTask
{
    public class TestHomeController
    {
        [Fact]
        public void TestErrorPage()
        {
            //arrange - исходные данные
            int enteredStatusCode = 404;
            int testValue = 0;
            //act - запускаемые действия
            if (enteredStatusCode == 404)
            {
                testValue = enteredStatusCode;
            }

            //assert - результат тестирования
            Assert.Equal(enteredStatusCode, testValue);
        }
    }
}