using System;
namespace Test.Models
{
    public class Flight
    {
        public string DeparteCity{ get; set; }
        public string ArrivalCity { get; set; }
        public DateTime DeparteDateTime { get; set; }
        public override bool Equals(object obj)
        {
            Flight flight = obj as Flight;
            if (
                DeparteCity.ToLower().Trim().Equals(flight.DeparteCity.ToLower().Trim())
                && ArrivalCity.ToLower().Trim().Equals(flight.ArrivalCity.ToLower().Trim())
                && DeparteDateTime.Date.Equals(flight.DeparteDateTime.Date)
                )
                return true;
            return base.Equals(obj);
        }
    }
}
