using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace UniversityClassAutomation.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void Login(string username, string password)
        {
            _driver.Navigate().GoToUrl("https://sis.case.edu/psp/P92SCWR/?cmd=login");
            _driver.Manage().Window.Maximize();

            // Handle initial page with "CWRU Users Login" button if it appears
            try
            {
                var ssoLoginButton = _driver.FindElement(By.Name("SSO_Signin"));
                if (ssoLoginButton.Displayed)
                {
                    ssoLoginButton.Click();
                }
            }
            catch (NoSuchElementException)
            {
                // SSO button not present, proceed directly to login
            }

            // Enter credentials on the login page
            _driver.FindElement(By.Id("username")).SendKeys(username);
            _driver.FindElement(By.Id("password")).SendKeys(password);
            _driver.FindElement(By.Id("login-submit")).Click();

            // Wait for Duo authentication to complete (manual step: approve on mobile)
            WaitForDuoApproval();

            // Click "Yes, this is my device" button to proceed
            ClickTrustBrowserButton();
        }

        private void WaitForDuoApproval()
        {
            Console.WriteLine("Waiting for Duo approval. Please approve the login on your device...");
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromMinutes(2));
            wait.Until(driver => driver.FindElement(By.Id("trust-browser-button")).Displayed);
        }

        private void ClickTrustBrowserButton()
        {
            var trustButton = _driver.FindElement(By.Id("trust-browser-button"));
            trustButton.Click();
        }
    }
}

