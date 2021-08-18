using System;

namespace EnergyReadings.Models
{
    public class MeterReading
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int MeterReadValue { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is MeterReading meterReading)
            {
                return AccountId == meterReading.AccountId
                    && MeterReadingDateTime == meterReading.MeterReadingDateTime
                    && MeterReadValue == meterReading.MeterReadValue;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashAccountId = AccountId.GetHashCode();
            var hashMeterReadingDateTime = MeterReadingDateTime.GetHashCode();
            var hashMeterReadValue = MeterReadValue.GetHashCode();
            return hashAccountId ^ hashMeterReadingDateTime ^ hashMeterReadValue;
        }
    }

    public class MeterReadingValidation
    {
        public string AccountId { get; set; }
        public string MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
}
