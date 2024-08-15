using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Chrome;

namespace Senseflow_Selenium
{
    [Collection("SFTest")]
    public class SenseflowSeleniumTests
    {
        WebDriver chromeDriver;
        WebDriverWait webDriverWait;
        private IConfiguration Configuration { get; }


        public SenseflowSeleniumTests()
        {
            chromeDriver = new ChromeDriver();
            webDriverWait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10));
            var builder = new ConfigurationBuilder().AddUserSecrets<SenseflowSeleniumTests>();
            Configuration = builder.Build();
        }

        [Fact]
        public void SenseflowLoginTest()
        {           
            chromeDriver.Navigate().GoToUrl("https://app.senseflow.ai/time");              
            webDriverWait.Until(d => d.Url.Contains("auth.senseflow.ai"));

            IWebElement orgTextBox = chromeDriver.FindElement(By.Name("organizationName"));
            webDriverWait.Until(d => orgTextBox.Displayed);
            orgTextBox.SendKeys("RetroRabbit");
            chromeDriver.FindElement(By.Name("action")).Click();

            webDriverWait.Until(d => d.Url.Contains("https://accounts.google.com/v3/signin/identifier"));
            IWebElement searchBox = chromeDriver.FindElement(By.Id("identifierId"));
            webDriverWait.Until(d => searchBox.Displayed);
            searchBox.SendKeys(Configuration["username"]);
            searchBox.SendKeys(Keys.Enter);

            webDriverWait.Until(d => d.Url.Contains("https://accounts.google.com/v3/signin/challenge/pwd"));
            webDriverWait.Until(d => chromeDriver.FindElement(By.Name("Passwd")).Displayed);
            IWebElement passwordBox = chromeDriver.FindElement(By.Name("Passwd"));
            passwordBox.SendKeys(Configuration["password"]);
            passwordBox.SendKeys(Keys.Enter);

            webDriverWait.Until(d => d.Url.Contains("https://app.senseflow.ai/time"));
            Assert.Equal(("SenseFlow"), chromeDriver.Title);
        }

        [Fact]
        public void Dispose()
        {
            chromeDriver.Quit();
        }
    }
}
