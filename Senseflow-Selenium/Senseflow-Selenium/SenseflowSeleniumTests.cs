using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Senseflow_Selenium
{
    [Collection("SFTest")]
    public class SenseflowSeleniumTests : IClassFixture<SenseflowBrowserFixtures>
    {
        private readonly SenseflowBrowserFixtures _fixture;
        private WebDriverWait _wait;

        public SenseflowSeleniumTests(SenseflowBrowserFixtures fixture) 
        {
            _fixture = fixture;
        }
    
        [Fact]
        public void SenseflowLoginTest()
        {
          
        }
    }
}
