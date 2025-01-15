using OpenQA.Selenium;

namespace UniversityClassAutomation.Pages
{
    public class ClassSearchPage
    {
        private IWebDriver _driver;

        public ClassSearchPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void SearchForClasses(string keyword)
        {
            _driver.FindElement(By.Id("CW_CLSRCH_WRK2_PTUN_KEYWORD")).SendKeys(keyword);
            _driver.FindElement(By.Id("CW_CLSRCH_WRK_SSR_PB_SEARCH$IMG")).Click();
        }

        public void DownloadExcel()
        {
            int attempts = 0;
            while (attempts < 3)
            {
                try
                {
                    // Re-locate the element to avoid StaleElementReferenceException
                    var excelButton = _driver.FindElement(By.Id("CW_CLSRCH_WRK2_TC_EXCEL_BTN"));
                    excelButton.Click();
                    break; // Exit the loop if the click is successful
                }
                catch (OpenQA.Selenium.StaleElementReferenceException)
                {
                    attempts++;
                    // Optionally, add a small delay before retrying
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

    }
}
