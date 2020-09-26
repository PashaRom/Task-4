using System;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using Test.Models;
using Test.Framework;
using Test.Framework.Logging;
using OpenQA.Selenium;
namespace Test.PageObjects
{
    public class TicketsPage
    {
        public WebDriver WebDriver;
        public string Name = "Ticket page";
        private const string productListItem = "//div[contains(@class,'product-list__item') and contains(@class,'fade-enter-done')]";
        public TicketsPage()
        {
            InstanceWebDriver instanceWebDriver = InstanceWebDriver.GetInstanceWebDriver();
            WebDriver = instanceWebDriver.GetDriver();
        }

        public void LoadPage()
        {
            Log.Info("Loading Ticket page.");
            try {
                IWebElement checkLoadPageElement = WebDriver.FluentWait(By.XPath("//div[@class='prediction-advice__icon']"), TimeSpan.FromSeconds(60), TimeSpan.FromMilliseconds(500));
                Log.Info("Result of searching tickets have been loaded.");

            }
            catch(Exception ex)
            {
                Log.Error(ex,$"Unexpected error occurred while loading the {Name}");
            }
        }
        public bool IsShowTicketsPriceList()
        {
            Log.Info("Show tickets price list.");
            try
            {
                ReadOnlyCollection<IWebElement> webElements = WebDriver.FindElements(By.XPath(productListItem));

                if (webElements.Count > 0) 
                {
                    Log.Info($"Tickets price list has been loaded on page {Name}");
                    return true;
                }
                else
                {
                    Log.Info($"Tickets price list has not been loaded on page {Name}");
                    return false;
                }                    
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred while showing ticket price list on {Name}.");
                return false;
            }
        }
        public Flight CompareBetweenSearchedOptionsAndFoundTickets(Flight flight)
        {
            Flight resaultFlight = new Flight();
            try
            {
                ReadOnlyCollection<IWebElement> webElements = WebDriver.FindElements(By.XPath(productListItem));
                foreach(IWebElement web in webElements)
                {                    
                    resaultFlight.DeparteCity = web.FindElement(By.XPath(".//div[contains(@class,'segment-route__endpoint') and contains(@class,'origin')]/div[@class='segment-route__city']")).Text;
                    string departeDate = web.FindElement(By.XPath(".//div[contains(@class,'segment-route__endpoint') and contains(@class,'origin')]/div[@class='segment-route__date']")).Text;
                    resaultFlight.ArrivalCity = web.FindElement(By.XPath(".//div[contains(@class,'segment-route__endpoint') and contains(@class,'destination')]/div[@class='segment-route__city']")).Text;
                    string[] splitDeparteDate = departeDate.Split(',');
                    resaultFlight.DeparteDateTime = DateTime.Parse(splitDeparteDate[0]);
                    if(
                        resaultFlight.DeparteCity.ToLower().Trim().Equals(flight.DeparteCity.ToLower())
                        && resaultFlight.ArrivalCity.ToLower().Trim().Equals(flight.ArrivalCity.ToLower().Trim())
                        && resaultFlight.DeparteDateTime.Date.Equals(flight.DeparteDateTime.Date)
                        )
                    {
                        Log.Info($"Data departe city \"{flight.DeparteCity}\" arrival city \"{flight.ArrivalCity}\" departe date {flight.DeparteDateTime.Date.ToString()} were equivalent searching resualt departe city \"{resaultFlight.DeparteCity}\" arrival city \"{resaultFlight.ArrivalCity}\" departe date {resaultFlight.DeparteDateTime.Date.ToString()}");
                        return resaultFlight;
                    }
                }
                return resaultFlight;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred while comparing loaded result on {Name}.");
                return resaultFlight;
            }
        }
        public (int CheapestPrice, int FirstPrice) GetCheapestPriceFirst()
        {
            ReadOnlyCollection<IWebElement> webElements = WebDriver.FindElements(By.XPath(productListItem));
            Queue<int> prices = new Queue<int>();
            int cheapestPrice = 0, counter = 0;
            try 
            {
                Log.Info($"Look for the cheapest price into list prise item.");
                foreach (IWebElement web in webElements)
                {
                    string stringPrice = web.FindElement(By.XPath(".//span[@class='buy-button__price']/span[@data-testid='price-with-logic']")).Text;                    
                    int price = Convert.ToInt32(Regex.Replace(stringPrice, @"[^\d]+", ""));
                    if(counter == 0)
                    {
                        cheapestPrice = price;
                        counter++;
                    }
                    if (cheapestPrice > price)
                        cheapestPrice = price;
                    prices.Enqueue(price);                    
                }
                if (prices.Peek() == cheapestPrice) 
                {
                    Log.Info($"The cheepest price is {cheapestPrice}. The first price in list price item {prices.Peek()}.");
                    return (cheapestPrice, prices.Peek());
                }
                else
                {
                    Log.Info($"The cheepest price is {cheapestPrice}. The first price in list price item {prices.Peek()}.");
                    return (cheapestPrice, prices.Peek());
                }
                    
            }
            catch(Exception ex)
            {
                Log.Error(ex,$"Unexpected error occurred while looking for the cheapest price into list price items.");
                return (0,1);
            }
        }
        public (int CheapestPrice, int LastPrice) GetCheapestPriceLast()
        {
            ReadOnlyCollection<IWebElement> headerButtons = WebDriver.FindElements(By.XPath("//div[@class='product-container__header-item']/div[@class='sorting']/ul[@class='sorting__tabs']/li"));           
            foreach(IWebElement button in headerButtons)
            {
                string t = button.Text;
                if (button.Text.ToLower().Trim().Contains("прямой")) {
                    Log.Info($"Click the direct button on {Name}.");
                    button.Click();
                    break;
                }
            }            
            for(int i = 0; i< 5; i++)
            {
                IWebElement directButton = WebDriver.FluentWait(By.XPath("//div[@class='product-container__header-item']/div[@class='sorting']/ul[@class='sorting__tabs']/li[contains(@class,'sorting__tab') and contains(@class,'is-active')]/span"), TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(500));
                if (directButton.Text.ToLower().Trim().Equals("прямой"))
                    break;
            }
            ReadOnlyCollection<IWebElement> webElements = WebDriver.FindElements(By.XPath(productListItem));
            Stack<int> prices = new Stack<int>();
            int cheapestPrice = 0, counter = 0;
            try
            {
                Log.Info($"Look for the cheapest price into list prise item.");
                foreach (IWebElement web in webElements)
                {
                    string stringPrice = web.FindElement(By.XPath(".//span[@class='buy-button__price']/span[@data-testid='price-with-logic']")).Text;                    
                    int price = Convert.ToInt32(Regex.Replace(stringPrice, @"[^\d]+", ""));
                    if (counter == 0)
                    {
                        cheapestPrice = price;
                        counter++;
                    }
                    if (cheapestPrice > price)
                        cheapestPrice = price;
                    prices.Push(price);
                }
                if (prices.Peek() == cheapestPrice)
                {
                    Log.Info($"The cheepest price is {cheapestPrice}. The first price in list price item {prices.Peek()}.");
                    return (cheapestPrice, prices.Peek());
                }
                else 
                {
                    Log.Info($"The cheepest price is {cheapestPrice}. The first price in list price item {prices.Peek()}.");
                    return (cheapestPrice, prices.Peek());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred while looking for the cheapest price into list price items.");
                return (0, 1);
            }
        }
        public (int NumberFlights, int NumberHandbag, int numberBaggage, bool EqualFligftBaggage) CheckAllFlightsHaveHandbagAndBaggage()
        {
            Log.Info($"Check all flights have handbag and baggage on {Name}.");
            ReadOnlyCollection<IWebElement> pricesList = null;
            int counterBaggage = 0, counterHandbag = 0;
            bool equalFligftBaggage = false;
            try
            {
                ReadOnlyCollection<IWebElement> popularFiltersItems = WebDriver.FindElements(By.XPath("//ul[@class='popular-filters__list']/li"));
                foreach(IWebElement popularFiltersItem in popularFiltersItems)
                {
                    if (popularFiltersItem.Text.ToLower().Trim().Contains("багаж"))
                    {
                        Log.Info("Click the button \"Baggage and handbag\".");
                        popularFiltersItem.Click();
                    }
                }                
                pricesList = WebDriver.FindElements(By.XPath(productListItem));
                ReadOnlyCollection<IWebElement> baggages;
                ReadOnlyCollection<IWebElement> handbags;                
                foreach (IWebElement pricesItem in pricesList)
                {
                    handbags = pricesItem.FindElements(By.XPath(".//span[contains(@class,'bag-icon') and contains(@class,'--handbags') and contains(@class,'--more-than-one')]"));
                    if (handbags.Count > 0)
                        counterHandbag++;
                    baggages = pricesItem.FindElements(By.XPath(".//span[contains(@class,'bag-icon') and contains(@class,'--baggage') and contains(@class,'--more-than-one')]"));
                    if (baggages.Count > 0)
                        counterBaggage++;
                }
                if (counterHandbag == pricesList.Count && counterBaggage == pricesList.Count)
                {
                    equalFligftBaggage = true;
                    Log.Info("All flights include baggage and handbag.");
                }
                else
                    Log.Info($"Not all flights include baggage and handbag because number of flights = {pricesList.Count} but flights which include baggage = {counterBaggage} and handbad = {counterHandbag}.");
                return (pricesList.Count, counterHandbag, counterBaggage, equalFligftBaggage);
                    
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred while checking all flights have handbag and baggage on {Name}.");
                return (pricesList.Count, counterHandbag, counterBaggage, equalFligftBaggage);
            }
        }
    }
}
