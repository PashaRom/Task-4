using OpenQA.Selenium;
namespace Test.Framework.Elements
{
    public class Input : BaseElement
    {
        public readonly string XpahtLocator;
        public Input(By locator, string name) : base(locator,name) {
            string xPath = "By.XPath: ";
            XpahtLocator = Locator.ToString().Remove(0, xPath.Length - 1);
        }        
    }    
}
