using System;
using Test.Forms;
using Test.Framework;
using Test.Framework.Configuration;
using Test.Framework.Logging;
using OpenQA.Selenium;
namespace Test.PageObjects
{
    class HomePage
    {        
        public string Name = "Home page";
        public const string SearchFormLocator = "//form[contains(@class,'--home')]";
        public const string FormName = "Search form";
        private WebDriver webDriver;
        public HomePage() 
        {
            InstanceWebDriver instanceWebDriver = InstanceWebDriver.GetInstanceWebDriver();
            webDriver = instanceWebDriver.GetDriver();
        }
        public SearchForm SearchForm;        
        public void LoadPage()
        {
            Log.Info($"Load \"{Name}\".");
            try
            {                
                ConfigurationManager configurationManager = ConfigurationManager.GetConfigurationManager();
                webDriver.Url = configurationManager.GetConfigeration().GetStringParam("url");
                Log.Info($"Get {FormName} on {Name}");
                SearchForm = new SearchForm(By.XPath(SearchFormLocator), FormName);

            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred while loading the {Name}");
            }
        }
        public void RefreshPage()
        {
            Log.Info($"Refresh \"{Name}\"");
            try 
            {                
                webDriver.Navigate().Refresh();
                SearchForm = new SearchForm(By.XPath(SearchFormLocator), FormName);
            }
            catch(Exception ex) 
            {
                Log.Error(ex, $"Unexpected error occurred while refreshing \"{Name}\"");
            }
        }
        public bool IsLoadPage()
        {
            Log.Info($"Check to load {Name}.");
            try 
            {
                IWebElement themeSwichCheckbox = webDriver.FluentWait(By.XPath("//span[@class='_theme_switch_checkbox_30psx']"), TimeSpan.FromSeconds(30), TimeSpan.FromMilliseconds(500));
                if (themeSwichCheckbox != null)
                {
                    Log.Info($"{Name} has been loaded.");
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred while checking to load \"{Name}\".");
                return false;
            }
        }
    }
}
