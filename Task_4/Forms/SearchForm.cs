using System;
using System.Globalization;
using Test.Framework.Elements;
using Test.Framework.Forms;
using Test.Framework.Logging;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
namespace Test.Forms
{
    class SearchForm : BaseForm
    {
        public Button SearchTickets;
        public Input DeparteCity;
        public Input ArrivalCity;    
        public Label LabelBokingHotel;
        private string departeCityLocator = ".//input[@type='text'][@id='origin']";
        private string irrivalCityLocator = ".//input[@type='text'][@id='destination']";
        private string checkboxHotelLocator = ".//label[@class='of_input_checkbox__label'][@for='clicktripz']";
        private string searchTicketsLocator = ".//button[contains(@class,'--on-home')]";

        public SearchForm(By locator, string name) :base(locator,name)
        {
            DeparteCity = new Input(By.XPath(departeCityLocator), "Departe city");
            ArrivalCity = new Input(By.XPath(irrivalCityLocator), "Irrival city");
            LabelBokingHotel = new Label(By.XPath(checkboxHotelLocator), "Checkbox booking hotel");
            SearchTickets = new Button(By.XPath(searchTicketsLocator), "Search ticket button");
            DeparteCity.WebElement = base.FormElement.FindElement(DeparteCity.Locator);
            ArrivalCity.WebElement = base.FormElement.FindElement(ArrivalCity.Locator);
            LabelBokingHotel.WebElement = base.FormElement.FindElement(LabelBokingHotel.Locator);
            SearchTickets.WebElement = base.FormElement.FindElement(SearchTickets.Locator);
        }

        public DateTime ChooseActualDate()
        {            
            try { 
                Log.Info("Start looking up an actual date.");
                IWebElement calendare = base.FormElement.FindElement(By.XPath(".//div[contains(@class, 'trip-duration__input-wrapper') and contains(@class, '--departure')]"));            
                calendare.Click();
                IWebElement priceElement = base.WebDriver.ExplicitWait(By.CssSelector("span._price_price_3lFsL.trip_dates_price"), TimeSpan.FromSeconds(10));
                if (priceElement == null)
                    throw new NullReferenceException("Price have not downloaded into celendar.");
                ReadOnlyCollection<IWebElement> calendareMounths = base.FormElement.FindElements(By.XPath(".//div[@class='calendar__month'][@role='grid']"));
                foreach(IWebElement mounth in calendareMounths)
                {
                    string currentMounth = mounth.FindElement(By.XPath(".//p[@class='calendar-caption__text']")).Text;
                    ReadOnlyCollection<IWebElement> calendarDays = mounth.FindElements(By.XPath(".//div[@class='calendar__day-cell ']"));
                    foreach(IWebElement day in calendarDays)
                    {
                        IWebElement rate;
                        string choseDate = null;
                        try
                        {
                            rate = day.FindElement(By.XPath(".//span[@class='_currency_sign_currencySign__GHEA']"));
                            choseDate = day.FindElement(By.XPath(".//div[@class='calendar-day__date']")).Text;                            
                        }
                        catch 
                        {
                            continue;
                        }                        
                        if (!String.IsNullOrEmpty(rate.Text))
                        {
                            var cultureInfo = new CultureInfo("ru-RU");
                            DateTime dateTime = DateTime.Parse($"{choseDate} {currentMounth}", cultureInfo);
                            day.Click();
                            return dateTime;
                        }                        
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex,"Unexpected error occurred during choosing actual date.");
            }
            return DateTime.MinValue;
        }
        public void CLickButtonDeleteReturnTicket()
        {
            Log.Info("Click the button Delete return ticket");
            string ButtonDeleteReturnTicketLocator = ".//button[contains(@class,'trip-duration__cancel-departure')]";
            Button deleteReturnTicket = new Button(By.XPath(ButtonDeleteReturnTicketLocator),"Delete return ticket");            
            try
            {
                deleteReturnTicket.WebElement = base.FormElement.FindElement(deleteReturnTicket.Locator);
                string cl = deleteReturnTicket.GetAttribute("class");
                deleteReturnTicket.Click();
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Unexpected error occurred during clicking button Dlelte return ticket");
            }
        }        
    }
}
