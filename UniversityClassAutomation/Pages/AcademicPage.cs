using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers; // Add this line

namespace UniversityClassAutomation.Pages
{
    public class AcademicPage
    {
        private IWebDriver _driver;

        public AcademicPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void NavigateToClassSearch()
        {
            _driver.FindElement(By.CssSelector(".media > .cw-tile-img")).Click();

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("SSR_CSTRMCUR_VW_DESCR$0")));
            element.Click();
        }
    }
}
