using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyReadings.Models
{
    public class CsvConversionError
    {
        public int AccountId { get; set; }

        public string ErrorMessage { get; set; }
    }
}
