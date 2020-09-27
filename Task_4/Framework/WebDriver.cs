using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using Test.Framework.Logging;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
namespace Test.Framework
{
    public class WebDriver : IWebDriver
    {
        private IWebDriver webDriver;
        private string name;
        //private TimeSpan timeSpanExplicitWait;
        private string url;
        private bool disposed = false;
        private IntPtr handle;
        public WebDriver()
        {
            webDriver = null;
            try 
            {
                Log.Info("Create web driver.");
                webDriver = new CreatorWebDriver().CreateWebDriver();
                if (webDriver == null)
                    throw new NullReferenceException("The web driver has not been created.");
            }
            catch (Exception ex) 
            {
                Log.Fatal(ex, "The WebDriver has not been created.");
            }
        }

        public string Url
        {
            get
            {                
                return url;
            }
            set
            {
                Log.Info($"Set url - {value}");
                webDriver.Url = value;
            }
        }

        public IWebElement FindElement(By locator) 
        {
            Log.Info($"Find element {locator.ToString()}");
            try 
            { 
                return webDriver.FindElement(locator);
            }
            catch(NoSuchElementException ex)
            {
                Log.Error(ex, $"No element matches locator {locator.ToString()}");
                return null;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Error occurred during finding element {locator.ToString()}");
                return null;
            }
        }

        public ReadOnlyCollection<IWebElement> FindElements(By locator)
        {
            return webDriver.FindElements(locator);
        }

        public IWebElement ExplicitWait(By locator, TimeSpan timeSpanExplicitWaitlocator) 
        {
            WebDriverWait webDriverWait = null;
            try 
            {
                webDriverWait = new WebDriverWait(webDriver, timeSpanExplicitWaitlocator);
                IWebElement webElement = webDriverWait.Until(ExpectedConditions.ElementExists(locator));
                return webElement;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "ExplicitWait has not been created.");
                return null;
            }
            
        }

        public IWebElement FluentWait(By locator,TimeSpan timeOut, TimeSpan pollingInterval)
        {
            try
            {
                DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(webDriver);
                fluentWait.Timeout = timeOut;
                fluentWait.PollingInterval = pollingInterval;
                fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                fluentWait.Message = "Element to be searched not found";
                IWebElement webElement = fluentWait.Until(x => x.FindElement(locator));
                return webElement;
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "$Unexpected error occurred during creating fluent wait.");
                return null;
            } 
        }
        
        public void MoveToElement(IWebElement webElement) 
        {            
            Actions act = new Actions(webDriver);            
            act.MoveToElement(webElement);
            act.Build().Perform();
        }

        public void ExecuteScript(string javaScript) 
        {
            Log.Info($"Execute java script \"{javaScript}\"");
            IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
            js.ExecuteScript(javaScript);
        }

        public IWebDriver GetWebDriver() 
        {
            return webDriver;
        }

        public IOptions Manage()
        {
            return webDriver.Manage();
        }

        public INavigation Navigate()
        {
            return webDriver.Navigate();
        }

        public ITargetLocator SwitchTo()
        {
            return webDriver.SwitchTo();
        }
        public string Title {
            get
            {
                return webDriver.Title;
            } 
        }
        public string PageSource {
            get
            {
                return webDriver.PageSource;
            } 
        }
        public string CurrentWindowHandle {
            get
            {
                return webDriver.CurrentWindowHandle;
            }
        }
        public ReadOnlyCollection<string> WindowHandles {
            get
            {
                return webDriver.WindowHandles;
            }
        }
        public void Quit() 
        {            
            webDriver.Quit();
            Log.Info($"{this.name} was quit.");
        }

        public void Close() 
        {
            webDriver.Close();
            Log.Info($"{this.name} was closed.");
        }
        public void Dispose()
        {
            Dispose(true);            
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                   webDriver.Dispose();
                }
                CloseHandle(handle);
                handle = IntPtr.Zero;
                disposed = true;
            }
        }
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);
        ~WebDriver() 
        {
            Quit();
            disposed = false;
        }
    }
}
