using Newtonsoft.Json.Bson;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace MySeleniumTests
{
    public class Tests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void Login()
        {
           driver.Navigate().GoToUrl("https://www.saucedemo.com/");
           IWebElement userName = driver.FindElement(By.Id("user-name"));
           userName.Clear();
           userName.SendKeys("standard_user");
           Thread.Sleep(2000);
           IWebElement password = driver.FindElement(By.Id("password"));
           userName.Clear();
           password.SendKeys("secret_sauce");
           Thread.Sleep(2000);
           IWebElement loginBtn = driver.FindElement(By.Name("login-button"));
           loginBtn.Click();
        }
        [Test]
        public void DropDownInteractions()
        {
           driver.Navigate().GoToUrl("https://www.selenium.dev/");
           IWebElement aboutDropDown = driver.FindElement(By.Id("navbarDropdown"));
           aboutDropDown.Click();
           IWebElement option = driver.FindElement(By.LinkText("About Selenium"));
           option.Click();
           Thread.Sleep(2000);

           driver.Navigate().GoToUrl("https://practice.expandtesting.com/dropdown");
           IWebElement dropdownElement = driver.FindElement(By.Id("dropdown"));
           SelectElement dropdown = new SelectElement(dropdownElement);
           dropdown.SelectByValue("1");
           IWebElement dateOB = driver.FindElement(By.Id("elementsPerPageSelect"));
           SelectElement dOBElement = new SelectElement(dateOB);
           dOBElement.SelectByText("10");
           IWebElement country = driver.FindElement(By.Id("country"));
           SelectElement countryElement = new SelectElement(country);
           countryElement.SelectByValue("AD");
        }
        [Test]
        public void RadioButtons_CheckBoxes()
        {
           driver.Navigate().GoToUrl("https://practice.expandtesting.com/radio-buttons");
           IWebElement colour = driver.FindElement(By.Id("red"));
           if (!colour.Selected)
           {
               colour.Click();
           }
           IWebElement sport = driver.FindElement(By.Id("basketball"));
           if (!sport.Selected)
           {
               sport.Click();
           }
           Thread.Sleep(3000);
           driver.Navigate().GoToUrl("https://practice.expandtesting.com/checkboxes");
           IWebElement checkbox = driver.FindElement(By.XPath("//*[@id=\"core\"]/div/div/div[3]"));
           if (!checkbox.Selected)
           {
               checkbox.Click();
           }
           Thread.Sleep(3000);
        }

        [Test]
        public void MultipleWindows()
        {
           driver.Navigate().GoToUrl("https://www.hyrtutorials.com/p/window-handles-practice.html");

           string mainWindow = driver.CurrentWindowHandle;
           driver.FindElement(By.Id("newWindowBtn")).Click();
           var allWindows = driver.WindowHandles;

           string newWindow = allWindows.FirstOrDefault(win => win != mainWindow);

           if (newWindow != null)
           {
               driver.SwitchTo().Window(newWindow);
               Thread.Sleep(3000);
               driver.Close();
               driver.SwitchTo().Window(mainWindow);
           }
           else
           {
               throw new Exception("No new window was opened.");
           }
        }

        [Test]
        public void FileUpload()
        {
           driver.Navigate().GoToUrl("https://practice.expandtesting.com/upload");
           IWebElement file = driver.FindElement(By.Id("fileInput"));
           string filePath = @"C:\Users\lenovo\OneDrive\Desktop\Selenium_N-Unit\test.txt";
           file.SendKeys(filePath);
           Thread.Sleep(1000);
           driver.FindElement(By.XPath("//*[@id=\"fileSubmit\"]")).Click();
           WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
           IWebElement successMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"core\"]/div/div/h1")));
           Assert.That(successMessage.Text, Does.Contain("File Uploaded!"));
        }

        [Test]
        public void DragandDrop()
        {
           driver.Navigate().GoToUrl("https://practice.expandtesting.com/drag-and-drop");
           IWebElement source = driver.FindElement(By.Id("column-a"));
           IWebElement target = driver.FindElement(By.Id("column-b"));
           Actions action = new Actions(driver);
           action.DragAndDrop(source, target).Perform();
           //reverse
           action.ClickAndHold(source).MoveToElement(target).Release().Perform();
           Thread.Sleep(3000);

        }
        [Test]
        public void TakeScreenshot()
        {
           driver.Navigate().GoToUrl("https://practice.expandtesting.com/");
           //full page
           ITakesScreenshot takesScreenshot = (ITakesScreenshot)driver;
           Screenshot screenshot = takesScreenshot.GetScreenshot();
           string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
           string fileName = $"screenshot_{timestamp}.png";
           string filePath = Path.Combine(Directory.GetCurrentDirectory(), "screenshots", fileName);
           Directory.CreateDirectory(Path.GetDirectoryName(filePath));
           screenshot.SaveAsFile(filePath);
           Console.WriteLine($"Screenshot saved: {filePath}");
        }
        [Test]
        public void WorkingWithIframesAndWait()
        {
           driver.Navigate().GoToUrl("https://practice.expandtesting.com/iframe");
           driver.SwitchTo().Frame("email-subscribe");
           IWebElement email = driver.FindElement(By.Id("email"));
           email.SendKeys("example@gmail.com");
           //wait until clickable
           WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
           IWebElement clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btn-subscribe\"]")));
           clickableElement.Click();
           driver.SwitchTo().DefaultContent();
           // other wait methods: ElementExists, ElementIsVisible, TextToBePresentInElement, TextToBePresentInElement, TitleIs

        }

        [Test]
        public void Alerts()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");

            IWebElement confirmButton = driver.FindElement(By.XPath("//button[text()='Click for JS Confirm']"));
            confirmButton.Click();
            var alert = driver.SwitchTo().Alert();
            string alertText = alert.Text;
            alert.Accept(); // or .Dismiss()
            Thread.Sleep(5000);
            IWebElement alertButton = driver.FindElement(By.XPath("//*[@id=\"content\"]/div/ul/li[1]/button"));
            alertButton.Click();
            var _alert = driver.SwitchTo().Alert();
            _alert.Accept();


        }

        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }
    }
}
