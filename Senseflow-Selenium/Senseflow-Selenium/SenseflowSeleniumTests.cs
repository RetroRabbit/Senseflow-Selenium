using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace Senseflow_Selenium
{
    [Collection("SFTest")]
    public class SenseflowSeleniumTests : IClassFixture<SenseflowBrowserFixtures>
    {
        private readonly SenseflowBrowserFixtures _fixture;
        private IConfiguration Configuration { get; }


        public SenseflowSeleniumTests(SenseflowBrowserFixtures fixture)
        {
            _fixture = fixture;
            var builder = new ConfigurationBuilder().AddUserSecrets<SenseflowSeleniumTests>();
            Configuration = builder.Build();
        }

        [Fact]
        public void SenseflowLoginTest()
        {
            _fixture.ChromeWebDriver.Navigate().GoToUrl("https://app.senseflow.ai/time");

            Thread.Sleep(1000);
            _fixture.ChromeWebDriver.FindElement(By.Name("organizationName")).SendKeys("RetroRabbit");
            _fixture.ChromeWebDriver.FindElement(By.Name("action")).Click();

            Thread.Sleep(2000);
            IWebElement searchBox = _fixture.ChromeWebDriver.FindElement(By.Id("identifierId"));
            searchBox.SendKeys(Configuration["username"]);
            searchBox.SendKeys(Keys.Enter);

            Thread.Sleep(3000);
            IWebElement passwordBox = _fixture.ChromeWebDriver.FindElement(By.Name("Passwd"));
            passwordBox.SendKeys(Configuration["password"]);
            passwordBox.SendKeys(Keys.Enter);

            Thread.Sleep(2000);
            Assert.Equal(("SenseFlow"), _fixture.ChromeWebDriver.Title);
        }
    }
}
