using System;
using Test.Framework.Logging;
using OpenQA.Selenium;
namespace Test.Framework.Forms
{
    public class BaseForm
    {
        public readonly By Locator;
        public readonly string Name;
        private IWebElement webElement;
        public WebDriver WebDriver;
        public BaseForm(By locator, string name)
        {
            Locator = locator;
            Name = name;            
            InstanceWebDriver instanceWebDriver = InstanceWebDriver.GetInstanceWebDriver();
            try {
                this.WebDriver = instanceWebDriver.GetDriver();
                webElement = WebDriver.FindElement(Locator);
                if (webElement == null)
                    throw new NullReferenceException();
            }
            catch(NullReferenceException ex)
            {
                Log.Error($"The error occurred during finding form element.The target element {Name} which has locator {Locator} has null value");
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during finding form element {Name} which has locator {Locator}.");
            }
        }
        public IWebElement FormElement
        {
            get
            {
                return webElement;
            }
        }
    }
}
