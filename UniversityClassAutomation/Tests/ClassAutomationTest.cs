using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using UniversityClassAutomation.Pages;
using UniversityClassAutomation.Utilities;
using System;

namespace UniversityClassAutomation.Tests
{
    public class ClassAutomationTest
    {
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Dispose();
        }

        [Test]
        public void AutomateClassSearch()
        {
            var username = ConfigReader.GetValue("Credentials", "Username");
            var password = ConfigReader.GetValue("Credentials", "Password");

            var loginPage = new LoginPage(_driver);
            loginPage.Login(username, password);

            var academicPage = new AcademicPage(_driver);
            academicPage.NavigateToClassSearch();

            var classSearchPage = new ClassSearchPage(_driver);
            classSearchPage.SearchForClasses("csds 4");
            classSearchPage.DownloadExcel();

            string downloadPath = @"C:\Users\Bharghava\Downloads";
            string downloadedFile = FileDownloader.WaitForDynamicFile(downloadPath, "bxp351");

            var excelUtility = new ExcelUtility();
            var openClasses = excelUtility.GetOpenClasses(downloadedFile);

            foreach (var openClass in openClasses)
            {
                Console.WriteLine($"Open Class: {openClass}");
            }
        }
    }
}
