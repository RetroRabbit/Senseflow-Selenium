using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

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
            webDriverWait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(20));
            webDriverWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            var builder = new ConfigurationBuilder().AddUserSecrets<SenseflowSeleniumTests>();
            Configuration = builder.Build();
        }

        [Fact]
        public void SenseflowLoginTest()
        {
            LogIn();
            Assert.Equal(("SenseFlow"), chromeDriver.Title);
            chromeDriver.Quit();
        }

        [Fact]
        public void NavigateToTeams()
        {
            LogIn();
            webDriverWait.Until(d => chromeDriver.FindElement(By.LinkText("Teams")).Displayed);
            IWebElement teamsButton = chromeDriver.FindElement(By.LinkText("Teams"));
            teamsButton.Click();
            webDriverWait.Until(d => d.Url.Contains("https://app.senseflow.ai/teams"));
            Assert.Equal(("https://app.senseflow.ai/teams"), chromeDriver.Url);
            chromeDriver.Quit();
        }

        [Fact]
        public void NavigateToProjects()
        {
            LogIn();
            webDriverWait.Until(d => chromeDriver.FindElement(By.LinkText("Projects")).Displayed);
            IWebElement projectButton = chromeDriver.FindElement(By.LinkText("Projects"));
            projectButton.Click();
            webDriverWait.Until(d => d.Url.Contains("https://app.senseflow.ai/projects"));
            Assert.Equal(("https://app.senseflow.ai/projects"), chromeDriver.Url);
            chromeDriver.Quit();
        }

        [Fact]
        public void MentoringViewPastMeetingsTest()
        {
            LogIn();
            webDriverWait.Until(d => d.Url.Contains("https://app.senseflow.ai/time"));
            webDriverWait.Until(d => chromeDriver.FindElement(By.LinkText("Mentoring")).Displayed);
            IWebElement mentoringTab = chromeDriver.FindElement(By.LinkText("Mentoring"));
            mentoringTab.Click();
            webDriverWait.Until(d => d.Url.Contains("https://app.senseflow.ai/mentoring"));            

            webDriverWait.Until(d => chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/div/div[2]/div/app-dashboard/div/div/app-my-meetings-dashboard/div[3]/div/h3[3]")).Displayed);
            IWebElement pastMeetings = chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/div/div[2]/div/app-dashboard/div/div/app-my-meetings-dashboard/div[3]/div/h3[3]"));
            pastMeetings.Click();

            string expectedUrl = "https://app.senseflow.ai/mentoring";
            Assert.Equal(expectedUrl, chromeDriver.Url);
            Assert.NotNull(chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/div/div[2]/div/app-dashboard/div/div/app-my-meetings-dashboard/div[3]/p-table")));
            chromeDriver.Quit();
        }

        [Fact]
        public void SenseflowCaptureTaskTest()
        {
            LogIn();
            Actions actions = new Actions(chromeDriver);
            //open Capture Task
            Thread.Sleep(5000);
            webDriverWait.Until(d => chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/div/div[1]/div[2]/div[2]/button[2]/span")).Displayed);
            chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/div/div[1]/div[2]/div[2]/button[2]/span")).Click();

            //Select an Allocation
            chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/app-journal-dialog/app-base-form/p-dialog/div/div/div[3]/form/div/form/div/div[2]/div/div/div[1]/div/div/div[2]/label")).Click();
            //Fill the Task input
            IWebElement subject = chromeDriver.FindElement(By.Id("subject"));
            subject.SendKeys("Running tests");
            //Fill the Description input
            IWebElement description = chromeDriver.FindElement(By.Id("body"));
            description.SendKeys("Running tests in Selenium for Senseflow");

            //Enter a location
            IWebElement location = chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/app-journal-dialog/app-base-form/p-dialog/div/div/div[3]/form/div/form/div/div[5]/p-autocomplete/div/ul/li/input"));
            location.SendKeys("Home");
            chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/app-journal-dialog/app-base-form/p-dialog/div/div/div[3]/form/div/form/div/div[5]/p-autocomplete/div/ul/li/input")).SendKeys(Keys.Enter);
            //Enter a skill
            IWebElement skills = chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/app-journal-dialog/app-base-form/p-dialog/div/div/div[3]/form/div/form/div/div[6]/p-autocomplete/div/ul/li/input"));
            location.SendKeys("Selenium");
            chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/app-journal-dialog/app-base-form/p-dialog/div/div/div[3]/form/div/form/div/div[6]/p-autocomplete/div/ul/li/input")).SendKeys(Keys.Enter);
            //Assert
            string expectedUrl = "https://app.senseflow.ai/time";
            Assert.Equal(expectedUrl, chromeDriver.Url);

            //Close the modal
            var element = chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/app-journal-dialog/app-base-form/p-dialog/div/div/div[3]/form/div/div[2]/button[1]/span"));
            actions.MoveToElement(element);
            webDriverWait.Until(d => chromeDriver.FindElement(By.XPath("/html/body/app-root/div/div[2]/app-main/app-journal-dialog/app-base-form/p-dialog/div/div/div[3]/form/div/div[2]/button[1]/span")).Displayed);
            actions.Perform();
            element.Click();

            chromeDriver.Quit();
        }

        private void LogIn()
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
        }
    }
}
