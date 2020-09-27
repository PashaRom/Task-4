using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Test.Framework.Logging;

namespace Test.Framework.Configuration
{
    public class ConfigurationGetter
    {         
        public ConfigurationGetter(string fileName)
        {
            var builder = new ConfigurationBuilder().AddJsonFile(fileName);
            GetConfiguration = builder.Build();            
        }
        public IConfiguration GetConfiguration { get; }
        public bool GetBooleanParam(string param)
        {
            string stringParam = null;
            bool booleanParam = false;
            Log.Info($"The param \"{param}\" is writing.");
            try
            {
                stringParam = GetConfiguration[param];
                if (String.IsNullOrEmpty(stringParam))
                    throw new FormatException($"The param \"{param}\" is empty or invalid format. \"{param}\" can have one value of false or true.");
                else
                    return booleanParam = Convert.ToBoolean(stringParam);
            }
            catch (FormatException ex)
            {
                Log.Fatal(ex, $"Invalid param \"{param}\" in {ConfigurationData.TestCofigurationFileName}. The param \"{param}\" can have value true or false");
                return booleanParam;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Unexpected error occurred during to write param {param} from {ConfigurationData.TestCofigurationFileName}.");
                return booleanParam;
            }
        }
        public string GetStringParam(string param)
        {
            string errorMessage = $"The param \"{param}\" is empty or invalid format. \"{param}\" can have one value of false or true.";
            string stringParam = null;
            Log.Info($"The param \"{param}\" is writing.");
            try
            {
                stringParam = GetConfiguration[param];
                if (String.IsNullOrEmpty(stringParam))
                    throw new Exception(errorMessage);
                else
                    return stringParam;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, errorMessage);
                return stringParam;
            }
        }
        public int GetIntParam(string param)
        {
            string stringParam = null;
            int intParam = 0;
            Log.Info($"The param \"{param}\" is writing.");
            try
            {
                stringParam = GetConfiguration[param];
                if (String.IsNullOrEmpty(stringParam))
                    throw new FormatException($"The param \"{param}\" is empty or invalid format. \"{param}\" can have integer value.");
                else
                    return intParam = Convert.ToInt32(stringParam);
            }
            catch (FormatException ex)
            {
                Log.Fatal(ex, $"Invalid param \"{param}\" in {ConfigurationData.TestCofigurationFileName}. The param \"{param}\" can have integer value.");
                return intParam;
            }
            catch (OverflowException ex)
            {
                Log.Fatal(ex, $"Invalid param \"{param}\" in {ConfigurationData.TestCofigurationFileName}. The param \"{param}\" can have integer value, but the current value has less than -2147483648 or greater than 2147483647.");
                return intParam;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Unexpected error occurred during to write param {param} from {ConfigurationData.TestCofigurationFileName}.");
                return intParam;
            }
        }
        public Queue<T> GetSectionWithArray<T>(string nameSection)
        {
            Log.Info($"Get section \"{nameSection}\" for class \"{typeof(T).ToString()}\"");
            Queue<T> ts = new Queue<T>();
            try
            {
                var valuesSection = GetConfiguration.GetSection(nameSection);
                foreach (IConfigurationSection section in valuesSection.GetChildren())
                {
                    ts.Enqueue(section.Get<T>());
                }
                return ts;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Unexpected error occurred during writing section \"{nameSection}\" for class \"{typeof(T).ToString()}\" from {ConfigurationData.TestCofigurationFileName} file.");
            }
            return ts;
        }
    }
}
