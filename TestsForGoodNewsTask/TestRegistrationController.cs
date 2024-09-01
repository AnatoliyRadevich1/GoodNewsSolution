using GoodNewsTask.Controllers;
using GoodNewsTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace TestsForGoodNewsTask
{
    //https://learn.microsoft.com/ru-ru/visualstudio/ide/how-to-add-or-remove-references-by-using-the-reference-manager?view=vs-2022 - ������ �� ������
    public class TestRegistrationController
    {
        [Fact]//�������, ���������� ��� � ���, ��� ����� �������� ��������, ������� ��� ���� ����� � ������ �����
        //[Theory] //������������ ����� ������, ������� ��������� ���� � ��� �� ���, �� ����� ������ ������� ���������
        //[InlineData(3, (-2), "1")] //������ �������� ��� ���� ������� ������
        //[InlineData(3, 3, "6")] //������ �������� ��� ���� ������� ������
        //[InlineData(3, 0, "3")] //������ �������� ��� ���� ������� ������
        public void TestCreate()
        {
            //arrange - �������� ������
            User testUser = new User();
            testUser.Id = Guid.NewGuid();
            testUser.Login = "TestLogin";
            testUser.Password = "TestPassword";
            testUser.ConfirmPassword = "TestPassword";
            testUser.Email = "testUser@testmail.com";
            testUser.SelectedPositiveLevel = 5;
            testUser.RegistrationDate = DateTime.Now;
            testUser.IsBlocked = false;

            //act - ����������� ��������
            if (string.IsNullOrEmpty(testUser.Login))
            {
                Assert.Equal(testUser.Login, "TestLogin"); //assert - ��������� ������������
            }

            if (string.IsNullOrEmpty(testUser.Password))
            {
                Assert.Equal(testUser.Password, "TestPassword"); //assert - ��������� ������������
            }
            if (testUser.Email == "testUser@testmail.com")
            {
                Assert.Equal(testUser.Email, "testUser@testmail.com"); //assert - ��������� ������������
            }

            if (testUser.SelectedPositiveLevel > 0 || testUser.SelectedPositiveLevel < 100)
            {
                Assert.Equal(testUser.SelectedPositiveLevel, 5); //assert - ��������� ������������
            }

            if (testUser.IsBlocked == false)
            {
                Assert.Equal(testUser.IsBlocked, false); //assert - ��������� ������������
            }

            //assert - ��������� ������������
            Assert.Equal(testUser.Password, testUser.ConfirmPassword);
        }
    }
}