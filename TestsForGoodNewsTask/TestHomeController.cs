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
            //arrange - �������� ������
            int enteredStatusCode = 404;
            int testValue = 0;
            //act - ����������� ��������
            if (enteredStatusCode == 404)
            {
                testValue = enteredStatusCode;
            }

            //assert - ��������� ������������
            Assert.Equal(enteredStatusCode, testValue);
        }
    }
}