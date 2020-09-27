namespace Test.Framework
{
    class InstanceWebDriver
    {
        private static InstanceWebDriver instanceWebDriver = null;
        private WebDriver driver;
        private InstanceWebDriver()
        {            
            driver = new WebDriver();
        }
        public static InstanceWebDriver GetInstanceWebDriver()
        {
            if (instanceWebDriver == null)
            {
                instanceWebDriver = new InstanceWebDriver();
            }
            return instanceWebDriver;
        }
        public static void QuitInstanceWebDriver()
        {
            if(instanceWebDriver != null)
            {
                instanceWebDriver = null;                
            }
        }
        public WebDriver GetDriver()
        {
            return driver;
        }

    }
}
