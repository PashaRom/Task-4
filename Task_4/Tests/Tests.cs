using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Test.Framework;
using Test.Models;
using Test.PageObjects;
using Test.Framework.Configuration;
using Test.Framework.Logging;
using OpenQA.Selenium;

namespace Test.Test
{
    [TestFixture]
    public class Tests
    {        
        public static IEnumerable<Flight> Flights1
        {
            get
            {
                ConfigurationManager configurationManager = ConfigurationManager.GetConfigurationManager();
                return configurationManager.GetData().GetSectionWithArray<Flight>("flight1");
            }
        }
        public static IEnumerable<Flight> Flights2
        {
            get
            {
                ConfigurationManager configurationManager = ConfigurationManager.GetConfigurationManager();
                return configurationManager.GetData().GetSectionWithArray<Flight>("flight2");
            }
        }
        public static IEnumerable<Flight> Flights3
        {
            get
            {
                ConfigurationManager configurationManager = ConfigurationManager.GetConfigurationManager();
                return configurationManager.GetData().GetSectionWithArray<Flight>("flight3");
            }
        }        
        [SetUp]
        public void Setup()
        {
            InstanceWebDriver instanceWebDriver = InstanceWebDriver.GetInstanceWebDriver();
            WebDriver webDriver = instanceWebDriver.GetDriver();
        }
        [Test]
        [TestCaseSource(nameof(Flights1))]
        [Category("TC-1")] 
        [Order(1)]
        public void FindCheapestTicketInTopPriceList(Flight actualFlight)
        {
            Log.Info("Start TC-1.");
            Flight expectedFlight;            
            actualFlight.DeparteDateTime = HomePageStapes(actualFlight, false);
            TicketsPage ticketsPage = new TicketsPage();
            Log.Info($"Load {ticketsPage.Name}.");
            ticketsPage.LoadPage();
            Assert.IsTrue(ticketsPage.IsShowTicketsPriceList(),
                $"Tickets price list did not show on {ticketsPage.Name}");
            expectedFlight = ticketsPage.CompareBetweenSearchedOptionsAndFoundTickets(actualFlight);
            Assert.AreEqual(expectedFlight, actualFlight,
                $"Actuale data departe city \"{actualFlight.DeparteCity}\" arrival city \"{actualFlight.ArrivalCity}\" departe date {actualFlight.DeparteDateTime.Date.ToString()} were not equivalent searching resualt departe city \"{expectedFlight.DeparteCity}\" arrival city \"{expectedFlight.ArrivalCity}\" departe date {expectedFlight.DeparteDateTime.Date.ToString()}");
            (int Cheapest, int FirstItem) Prices = ticketsPage.GetCheapestPriceFirst();
            Assert.AreEqual(Prices.Cheapest, Prices.FirstItem,
                $"The cheapest prices {Prices.Cheapest} and the first price in list items {Prices.FirstItem} are not equqle.");
            Log.Info("Finish TC-1.");
        }
        [Test]
        [TestCaseSource(nameof(Flights2))]
        [Category("TC-2")]
        [Order(2)]
        public void FindCheapestDirectFlightTicketInBottomPriceList(Flight actualFlight)
        {
            Log.Info("Start TC-2.");
            Flight expectedFlight;
            actualFlight.DeparteDateTime = HomePageStapes(actualFlight, false);            
            TicketsPage ticketsPage = new TicketsPage();
            Log.Info($"Load {ticketsPage.Name}.");
            ticketsPage.LoadPage();
            Assert.IsTrue(ticketsPage.IsShowTicketsPriceList(),
                $"Tickets price list did not show on {ticketsPage.Name}");
            expectedFlight = ticketsPage.CompareBetweenSearchedOptionsAndFoundTickets(actualFlight);
            Assert.AreEqual(expectedFlight, actualFlight,
                $"Actuale data departe city \"{actualFlight.DeparteCity}\" arrival city \"{actualFlight.ArrivalCity}\" departe date {actualFlight.DeparteDateTime.Date.ToString()} were not equivalent searching resualt departe city \"{expectedFlight.DeparteCity}\" arrival city \"{expectedFlight.ArrivalCity}\" departe date {expectedFlight.DeparteDateTime.Date.ToString()}");
            (int Cheapest, int LastItem) Prices = ticketsPage.GetCheapestPriceLast();
            Assert.AreEqual(Prices.Cheapest, Prices.LastItem,
               $"The cheapest prices {Prices.Cheapest} and the bottom list price on list items {Prices.LastItem} are not equqle.");
            Log.Info("Finish TC-2.");
        }
        [Test]
        [TestCaseSource(nameof(Flights3))]
        [Category("TC-3")]
        [Order(3)]
        public void AllFlightsHaveHandbagAndBaggage(Flight actualFlight)
        {
            Log.Info("Start TC-3.");
            Flight expectedFlight;
            actualFlight.DeparteDateTime = HomePageStapes(actualFlight, true);            
            TicketsPage ticketsPage = new TicketsPage();
            Log.Info($"Load {ticketsPage.Name}.");
            ticketsPage.LoadPage();
            Assert.IsTrue(ticketsPage.IsShowTicketsPriceList(),
                $"Tickets price list did not show on {ticketsPage.Name}");
            expectedFlight = ticketsPage.CompareBetweenSearchedOptionsAndFoundTickets(actualFlight);
            Assert.AreEqual(expectedFlight, actualFlight,
                $"Actuale data departe city \"{actualFlight.DeparteCity}\" arrival city \"{actualFlight.ArrivalCity}\" departe date {actualFlight.DeparteDateTime.Date.ToString()} were not equivalent searching resualt departe city \"{expectedFlight.DeparteCity}\" arrival city \"{expectedFlight.ArrivalCity}\" departe date {expectedFlight.DeparteDateTime.Date.ToString()}");
            (int NumberFlights, int NumberHandbag, int NumberBaggage, bool EqualFligftBaggage) optionsFlightsBaggage = ticketsPage.CheckAllFlightsHaveHandbagAndBaggage();
            Assert.IsTrue(optionsFlightsBaggage.EqualFligftBaggage,
                $"Not all flights include baggage and handbag because number of flights = {optionsFlightsBaggage.NumberFlights} but flights which include baggage = {optionsFlightsBaggage.NumberBaggage} and handbad = {optionsFlightsBaggage.NumberHandbag}.");
            Log.Info("Finish TC-3.");

        }
        [TearDown]
        public void TearDown()
        {
            InstanceWebDriver instanceWebDriver = InstanceWebDriver.GetInstanceWebDriver();
            instanceWebDriver.GetDriver().Quit();
            InstanceWebDriver.QuitInstanceWebDriver();
        }        
        public DateTime HomePageStapes(Flight actualFlight, bool refreshPage)
        {
            HomePage homePage = new HomePage();
            DateTime departeDateTime = DateTime.MinValue;
            try {
                Log.Info($"Load {homePage.Name}.");
                homePage.LoadPage();
                if (refreshPage) 
                { 
                    homePage.RefreshPage();
                    homePage.IsLoadPage();
                }
                Log.Info($"Clear departure the {homePage.SearchForm.DeparteCity.Name} input.");
                homePage.SearchForm.DeparteCity.Clear();
                Log.Info($"Send key \"\" to the {homePage.SearchForm.DeparteCity.Name} input.");
                homePage.SearchForm.DeparteCity.SendKeys("");
                Log.Info($"Send key to the \"{actualFlight.DeparteCity}\" to departure city input.");
                homePage.SearchForm.DeparteCity.SendKeys(actualFlight.DeparteCity);
                Log.Info($"Click on the {homePage.SearchForm.DeparteCity.Name} input.");
                homePage.SearchForm.DeparteCity.Click();
                Log.Info($"Send key to the \"{actualFlight.ArrivalCity}\" to departure city input.");
                homePage.SearchForm.ArrivalCity.SendKeys(actualFlight.ArrivalCity);
                Log.Info($"Click on the {homePage.SearchForm.ArrivalCity.Name} input.");
                homePage.SearchForm.ArrivalCity.Click();
                Log.Info("Choose actual date.");
                departeDateTime = homePage.SearchForm.ChooseActualDate();
                Log.Info("Click on the \"delete return ticket\" button.");
                homePage.SearchForm.CLickButtonDeleteReturnTicket();
                Log.Info("Click on the \"Booking hotell\" checkbox.");
                homePage.SearchForm.LabelBokingHotel.Click();
                Log.Info($"Click on the {homePage.SearchForm.SearchTickets.Name} button.");
                homePage.SearchForm.SearchTickets.Submit();
                return departeDateTime;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred while executing repeatAction on \"{homePage.Name}\"");
                return departeDateTime;
            }
        }
    }
}