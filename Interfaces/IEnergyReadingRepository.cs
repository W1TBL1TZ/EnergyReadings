using EnergyReadings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyReadings.Interfaces
{
    public interface IEnergyReadingRepository
    {
        IEnumerable<Account> GetAccounts();
        IEnumerable<int> ReturnAccountIdsNotOnRecord(IEnumerable<int> enumerable);
        Task<int> PersistMeterReadings(List<MeterReading> readings);
    }
}
