using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Drawing;
using System.Collections.ObjectModel;
using Test.Framework.Logging;

namespace Test.Framework.Elements
{
    public abstract class BaseElement : IWebElement
    {
        protected WebDriver webDriver;
        private IWebElement webElement;
        public readonly By Locator;
        public readonly string Name;
        
        public IWebElement WebElement
        {
            get
            {
                return this.webElement;
            }
            set
            {
                this.webElement = value;
            }
        }
        public BaseElement(By locator, string name)
        {
            Locator = locator;
            Name = name;
            InstanceWebDriver instanceWebDriver = InstanceWebDriver.GetInstanceWebDriver();
            webDriver = instanceWebDriver.GetDriver();
        }        
        public bool IsNull
        {
            get
            {
                return webElement == null ? true : false;
            }
        }        
        public void Click()
        {
            try
            {
                Log.Info($"Click the element {Name} which has locator {Locator}.");
                if (!this.IsNull)
                {
                    Actions action = new Actions(webDriver.GetWebDriver());
                    action.MoveToElement(webElement);
                    action.Build().Perform();
                    webElement.Click();
                }
                else
                    throw new Exception($"Error occurred during clicking. The target element {Name} which has locator {Locator} do not click because it has null value.");
            }
            catch (ElementNotVisibleException ex) {
                Log.Error(ex,$"Error occurred during clicking. The target element {Name} which has locator {Locator} is not visible");            
            }
            catch (StaleElementReferenceException ex)
            {
                Log.Error(ex, $"Error occurred during clicking. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during clicking the target element {Name} which has locator {Locator}.");
            }
        }

        public void Clear()
        {
            try
            {
                webElement.Clear();
            }
            catch(StaleElementReferenceException ex)
            {
                Log.Error(ex, $"Error occurred during clearing.The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during clearing on the target element {Name} which has locator {Locator}");
            }
        }
        public void SendKeys(string text)
        {
            Log.Info($"Send key \"{text}\" to the target element {Name} which has locator {Locator}.");
            try
            {
                webElement.SendKeys(text);
            }
            catch (ElementNotVisibleException ex)
            {
                Log.Error(ex, $"Error occurred during SendKey. The target element {Name} which has locator {Locator} is not visible.");
            }
            catch (InvalidElementStateException ex)
            {
                Log.Error(ex, $"Error occurred during SendKey. The target element {Name} which has locator {Locator} is not enabled.");
            }
            catch (StaleElementReferenceException ex)
            {
                Log.Error(ex,$"Error occurred during SendKey. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
            }
            catch(NullReferenceException ex)
            {
                Log.Error(ex,$"The error occurred during SendKey.The target element {Name} which has locator {Locator} has null value");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during SendKey to the target element {Name} which has locator {Locator}");
            }
        }
        public void Submit()
        {
            Log.Info($"Submit to the target element {Name} which has locator {Locator}.");
            try { 
                webElement.Submit();
            }
            catch (StaleElementReferenceException ex)
            {
                Log.Error(ex,$"Error occurred during Submit. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during Submit to the target element {Name} which has locator {Locator}");
            }
        }

        public string GetAttribute(string attributeName)
        {
            Log.Info($"Get atribute {attributeName} from the target element {Name} which has locator {Locator}.");
            try { 
                return webElement.GetAttribute(attributeName);
            }
            catch (StaleElementReferenceException ex)
            {
                Log.Error(ex, $"Error occurred during GetAttribute. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
                return null;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during GetAttribute to the target element {Name} which has locator {Locator}");
                return null;
            }
        }
        public string GetProperty(string propertyName)
        {
            Log.Info($"GetProperty {propertyName} from the target element {Name} which has locator {Locator}.");
            try { 
                return webElement.GetProperty(propertyName);
            }
            catch (StaleElementReferenceException ex)
            {
                Log.Error(ex, $"Error occurred during GetProperty. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during GetProperty to the target element {Name} which has locator {Locator}");
                return null;
            }
        }
        public string GetCssValue(string propertyName)
        {
            Log.Info($"GetCssValue {propertyName} from the target element {Name} which has locator {Locator}.");
            try { 
                return webElement.GetCssValue(propertyName);
            }
            catch (StaleElementReferenceException ex)
            {
                Log.Error(ex, $"Error occurred during GetCssValue. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during GetCssValue from the target element {Name} which has locator {Locator}");
                return null;
            }
        }
        public string TagName
        {
            get
            {
                Log.Info($"Get teg name the target element {Name} which has locator {Locator}.");
                try { 
                    return webElement.TagName;
                }
                catch (StaleElementReferenceException ex)
                {
                    Log.Error(ex, $"Error occurred during getting tag name. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
                    return null;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Unexpected error occurred during getting tag name the target element {Name} which has locator {Locator}.");
                    return null;
                }
            }
        }
        public string Text
        {
            get
            {
                Log.Info($"Get text from the target element {Name} which has locator {Locator}.");
                try { 
                    return webElement.Text;
                }
                catch (StaleElementReferenceException ex)
                {
                    Log.Error(ex, $"Error occurred during getting text. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
                    return null;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Unexpected error occurred during getting text from the target element {Name} which has locator {Locator}.");
                    return null;
                }
            }
        }
        public bool Enabled
        {
            get
            {
                Log.Info($"Is enabled the target element {Name} which has locator {Locator}.");
                try { 
                    return webElement.Enabled;
                }
                catch (StaleElementReferenceException ex)
                {
                    Log.Error(ex, $"Error occurred during was enabled. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
                    return false;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Unexpected error occurred during was enabled the target element {Name} which has locator {Locator}.");
                    return false;
                }
            }
        }
        public bool Selected
        {
            get
            {
                Log.Info($"Is Selected the target element {Name} which has locator {Locator}.");
                try 
                { 
                    return webElement.Selected;
                }
                catch (StaleElementReferenceException ex)
                {
                    Log.Error(ex, $"Error occurred during was selected. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
                    return false;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Unexpected error occurred during was selected the target element {Name} which has locator {Locator}.");
                    return false;
                }
            }
        }
        public Point Location
        {
            get
            {
                Log.Info($"Get location the target element {Name} which has locator {Locator}.");
                try { 
                    return webElement.Location;
                }
                catch (StaleElementReferenceException ex)
                {
                    Log.Error(ex, $"Error occurred during getting location. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
                    return new Point { X = 0, Y=0 };
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Unexpected error occurred during getting location the target element {Name} which has locator {Locator}.");
                    return new Point { X = 0, Y = 0 }; ;
                }
            }
        }
        public Size Size
        {
            get
            {
                Log.Info($"Get size the target element {Name} which has locator {Locator}.");
                try { 
                    return webElement.Size;
                }
                catch (StaleElementReferenceException ex)
                {
                    Log.Error(ex, $"Error occurred during getting size. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
                    return new Size {Width = -1, Height = -1 };
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Unexpected error occurred during getting size the target element {Name} which has locator {Locator}.");
                    return new Size { Width = -1, Height = -1 };
                }
            }
        }
        public bool Displayed
        {
            get
            {
                Log.Info($"Is displayed the target element {Name} which has locator {Locator} is searched.");
                try
                {
                    return webElement.Displayed;
                }
                catch (StaleElementReferenceException ex)
                {
                    Log.Error(ex, $"Error occurred during Displayed. The target element {Name} which has locator {Locator} is no longer valid in the document DOM");
                    return false;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Unexpected error occurred during was displayed the target element {Name} which has locator {Locator}.");
                    return false;
                }

            }
        }
        public IWebElement FindElement(By by)
        {
            Log.Info($"Find the target element {by} inside the current element {Name} which has locator {Locator} is searched.");
            try { 
                return webElement.FindElement(by);
            }
            catch(NoSuchElementException ex)
            {
                Log.Error(ex, $"Error occurred during finding element {by} inside the current element {Name} which has locator {Locator}.");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during finding element {by} inside the current element {Name} which has locator {Locator} is searched.");
                return null;
            }
        }
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            Log.Info($"Find the target elements {by} inside the current element {Name} which has locator {Locator} is searched.");
            try { 
                return webElement.FindElements(by);
            }
            catch (NoSuchElementException ex)
            {
                Log.Error(ex, $"Error occurred during finding elements {by} inside the current element {Name} which has locator {Locator}.");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during finding elements {by} inside the current element {Name} which has locator {Locator} .");
                return null;
            }
        }
    }
}
