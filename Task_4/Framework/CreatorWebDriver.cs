using System;
using System.IO;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Test.Framework.Configuration;
using Test.Framework.Configuration.Models;
using Test.Framework.Logging;

namespace Test.Framework
{
    public class CreatorWebDriver {
        private IWebDriver webDriver;
        ChromeOptions ChromeOptions;
        FirefoxOptions FirefoxOptions;
        public readonly bool IsActiveChromeDriver;
        public readonly bool IsActiveFirefoxDriver;               
        private readonly string PathDownloadFiles;
        private int timeExplicitWait;
        private ConfigurationGetter configuration;
        public string Name { get; set; }
        public const string IntlAcceptLanguages = "intl.accept_languages";
        public struct ChromeDriverOptions
        {
            public const string IsActive = "browsers:chrome:isActive";
            public const string PathToDriver = "browsers:chrome:pathToDriver";
            public const string LeaveBrowserRunning = "browsers:chrome:options:leaveBrowserRunning";
            public const string StartSizeWindow = "browsers:chrome:options:startSizeWindow";
            public const string Mode = "browsers:chrome:options:mode";
            public const string ImplicitWait = "browsers:chrome:options:implicitWait";
            public const string ExplicitWait = "browsers:chrome:options:explicitWait";
            public const string SafebrowsingDisableDownloadProtection = "safebrowsing-disable-download-protection";            
            public const string DownloadDefaultDirectory = "download.default_directory";            
            public struct Safebrowsing
            {
                public const string Name = "safebrowsing";
                public const string Value = "enabled";
            }            
            public struct DisablePopupBlocking
            {
                public const string Name = "disable-popup-blocking";
                public const string Value = "true";
            }
        }
        public struct FirefoxDriverOptions
        {
            public static string isActive = "browsers:firefox:isActive";
            public const string PathToDriver = "browsers:firefox:pathToDriver";
            public const string StartSizeWindow = "browsers:firefox:options:startSizeWindow";
            public const string ImplicitWait = "browsers:firefox:options:implicitWait";
            public const string ExplicitWait = "browsers:firefox:options:explicitWait";
            public struct BrowserDownloadFolderList
            {
                public const string name = "browser.download.folderList";
                public const int value = 2;
            }
        }
        public TimeSpan TimeForExplicitWait
        {
            get
            {
                return TimeSpan.FromSeconds(timeExplicitWait);
            }
        }
        public CreatorWebDriver()
        {
            ConfigurationManager configurationManager = ConfigurationManager.GetConfigurationManager();
            configuration = configurationManager.GetConfigeration();           
            webDriver = null;
            PathDownloadFiles = GetDownloadFullPathDirectory(configuration.GetStringParam("pathDownloadFiles"));
            IsActiveChromeDriver = configuration.GetBooleanParam(ChromeDriverOptions.IsActive);
            IsActiveFirefoxDriver = configuration.GetBooleanParam(FirefoxDriverOptions.isActive);            
        }
        public IWebDriver CreateWebDriver()
        {
            try { 
                if (IsActiveChromeDriver && IsActiveFirefoxDriver)
                    Log.Fatal($"Both params {IsActiveChromeDriver} and {IsActiveFirefoxDriver} do not have true values at the same time.");
                else if (!IsActiveChromeDriver && !IsActiveFirefoxDriver)
                    Log.Fatal($"Both params {IsActiveChromeDriver} and {IsActiveFirefoxDriver} do not have false values at the same time.");
                else if (IsActiveChromeDriver) 
                { 
                    this.ChromeOptions = InitChromeOptions();
                    webDriver = CreateChromeDriver();
                    if (webDriver == null)
                        throw new NullReferenceException("The ChromeDriver has not been created.");
                    this.Name = "CromeDriver";
                    timeExplicitWait = configuration.GetIntParam(ChromeDriverOptions.ExplicitWait);
                }
                else if (IsActiveFirefoxDriver) { 
                    this.FirefoxOptions = InitFirefoxOptions();
                    webDriver = CreateFirefoxDriver();
                    if (webDriver == null)
                        throw new NullReferenceException("The FirefoxDriver has not been created.");
                    this.Name = "FirefoxDriver";
                    timeExplicitWait = configuration.GetIntParam(FirefoxDriverOptions.ExplicitWait);
                }
            return webDriver;
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Unexpected error occurred during creating WebDriwer");
                return webDriver;
            }
        }
        public IWebDriver CreateChromeDriver() {
            IWebDriver webDriver = null;
            try
            {
                Log.Info("Create CromeDriver.");
                webDriver = new ChromeDriver(
                    configuration.GetStringParam(ChromeDriverOptions.PathToDriver),
                    this.ChromeOptions);
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(configuration.GetIntParam(ChromeDriverOptions.ImplicitWait));                
                return webDriver;
            }
            catch (Exception ex) {
                Log.Fatal(ex, "Unexpected error occurred during creation chrome driver.");
                return webDriver;
            }            
        }

        public IWebDriver CreateFirefoxDriver()
        {
            IWebDriver webDriver = null;
            try 
            {
                Log.Info("Create FirefoxDriver.");
                FirefoxOptions firefoxOptions = new FirefoxOptions();                                    
                webDriver = new FirefoxDriver(
                    configuration.GetStringParam(FirefoxDriverOptions.PathToDriver),
                    this.FirefoxOptions);
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(configuration.GetIntParam(FirefoxDriverOptions.ImplicitWait));                                
                return webDriver;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "While creating firefox driver was error.");
                return webDriver;
            }            
        }
        private ChromeOptions InitChromeOptions()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            Log.Info("Initialization ChromeDriver options.");
            try 
            {
                chromeOptions.LeaveBrowserRunning = configuration.GetBooleanParam(ChromeDriverOptions.LeaveBrowserRunning);
                chromeOptions.AddArgument(configuration.GetStringParam(ChromeDriverOptions.StartSizeWindow));
                chromeOptions.AddArgument(configuration.GetStringParam(ChromeDriverOptions.Mode));
                chromeOptions.AddArgument(ChromeDriverOptions.SafebrowsingDisableDownloadProtection);
                chromeOptions.AddUserProfilePreference(ChromeDriverOptions.Safebrowsing.Name, ChromeDriverOptions.Safebrowsing.Value);
                chromeOptions.AddUserProfilePreference(ChromeDriverOptions.DownloadDefaultDirectory, PathDownloadFiles);
                chromeOptions.AddUserProfilePreference(ChromeDriverOptions.DisablePopupBlocking.Name, ChromeDriverOptions.DisablePopupBlocking.Value);
                chromeOptions.AddUserProfilePreference(IntlAcceptLanguages, GetLocalization().Name);
                return chromeOptions;
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, $"Unexpected error occurred during initialization CromeDriver options.");
                return chromeOptions;
            }
        }
        private FirefoxOptions InitFirefoxOptions()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            Log.Info("Initialization FirefoxDriver options.");
            try
            {
                firefoxOptions.AddArgument(FirefoxDriverOptions.StartSizeWindow);
                firefoxOptions.SetPreference("browser.download.folderList", 2);
                firefoxOptions.SetPreference("browser.download.manager.showWhenStarting", false);
                firefoxOptions.SetPreference("browser.download.dir", PathDownloadFiles);
                firefoxOptions.SetPreference("browser.download.downloadDir", PathDownloadFiles);
                firefoxOptions.SetPreference("browser.download.defaultFolder", PathDownloadFiles);
                firefoxOptions.SetPreference("browser.download.useDownloadDir", true);
                firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/octet-stream");
                firefoxOptions.SetPreference(IntlAcceptLanguages, GetLocalization().Name);
                return firefoxOptions;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Unexpected error occurred during initialization FirefoxDriver options.");
                return firefoxOptions;
            }
        }
        private string GetDownloadFullPathDirectory(string path)
        {
            string pathDownload = null;
            try
            {
                if (path == null) 
                {
                    pathDownload = Directory.GetCurrentDirectory();
                    Log.Info($"The directory for download files has been set {pathDownload}");
                    return pathDownload;
                }
                pathDownload = Path.GetFullPath(path);
                Log.Info($"The directory for download files has been set {pathDownload}");
                return pathDownload;
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Fatal(ex, $"The caller does not have the required permission to path {path}.");
                return pathDownload;
            }            
            catch (NotSupportedException ex)
            {
                Log.Fatal(ex, $"The operating system is Windows CE, which does not have current directory functionality.");
                return pathDownload;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Unexpected error occurred during to get path download {path}.");
                return pathDownload;
            }
        }

        private Language GetLocalization() 
        {
            Queue<Language> languages = new Queue<Language>();
            Language language = null;
            try 
            { 
                languages = configuration.GetSectionWithArray<Language>("localization");
                foreach(Language lang in languages)
                {
                    if (lang.isActive) { 
                        language = lang;
                        break;
                    }                
                }
                return language;
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, $"Unexpected error occurred during getting localization param from {ConfigurationData.TestCofigurationFileName} file.");
                return language;
            }
        }
    }
}
