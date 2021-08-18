using System.Collections.Generic;

namespace EnergyReadings.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<MeterReading> MeterReadings { get; set; }

        public void AddMeterReading(MeterReading reading)
        {
            if(MeterReadings == null)
            {
                MeterReadings = new List<MeterReading>();
            }
            MeterReadings.Add(reading);
        }
    }
}
