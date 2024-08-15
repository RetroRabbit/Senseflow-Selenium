using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Senseflow_Selenium;

public class SenseflowBrowserFixtures : IDisposable
{
    public IWebDriver ChromeWebDriver { get; set; }

    public SenseflowBrowserFixtures()
    {
        ChromeWebDriver = new ChromeDriver();
    }

    public void Dispose()
    {
        ChromeWebDriver.Quit();
    }
}
