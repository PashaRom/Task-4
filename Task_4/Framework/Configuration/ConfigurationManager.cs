namespace Test.Framework.Configuration
{
    public class ConfigurationManager
    {
        private static ConfigurationManager instanceConfiguration = null;
        private ConfigurationGetter configuration;
        private ConfigurationGetter testData;
        private ConfigurationManager() {
            configuration = new ConfigurationGetter(ConfigurationData.TestCofigurationFileName);
            testData = new ConfigurationGetter(ConfigurationData.TestDataFileName);
        }
        public static ConfigurationManager GetConfigurationManager()
        {
            if(instanceConfiguration == null)
            {
                instanceConfiguration = new ConfigurationManager();
            }
            return instanceConfiguration;
        }
        public ConfigurationGetter GetConfigeration()
        {
            return configuration;
        }
        public ConfigurationGetter GetData()
        {
            return testData;
        }
    }   
}
