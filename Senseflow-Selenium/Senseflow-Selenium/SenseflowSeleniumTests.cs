using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System.Threading;
using Xunit.Abstractions;
using Xunit.Sdk;
public class AlphabeticalOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
        IEnumerable<TTestCase> testCases) where TTestCase : ITestCase =>
        testCases.OrderBy(testCase => testCase.TestMethod.Method.Name);
}
namespace Senseflow_Selenium
{
    [Collection("SFTest")]
    [TestCaseOrderer(
    ordererTypeName: "AlphabeticalOrderer",
    ordererAssemblyName: "Senseflow-Selenium")]
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
        public void aSenseflowLoginTest()
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
            Thread.Sleep(2000);
        }

        [Fact]
        public void bSenseflowOpenTeamsTest()
        {
            Thread.Sleep(6000);
            _fixture.ChromeWebDriver.FindElement(By.LinkText("Teams")).Click();
            Thread.Sleep(6000);

        }

        [Fact]
        public void cSenseflowOpenProjectsTest()
        {
            Thread.Sleep(6000);
            _fixture.ChromeWebDriver.FindElement(By.LinkText("Projects")).Click();
            Thread.Sleep(6000);

        }
    }
}
