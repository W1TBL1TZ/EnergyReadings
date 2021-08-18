using EnergyReadings.Interfaces;
using EnergyReadings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyReadings.Services
{
    // Given the lack of time, I would rather just explain the database implementation I would have used and still provide all the code in the other layers.
    // I would have gone with a simple SQL Server database with 2 tables, one for accounts and one for the meter readings with a FK for account id.
    // I would create and index on the account id FK, and then implement any selects required from the API into views, and any data altering
    // requrests would have an accompanying stored procedure to make the data changes. These SPs would all have locked down rights, where access,
    // would only be given to the user account that the API is executing under.
    public class EnergyReadingRepository : IEnergyReadingRepository
    {
        private List<Account> accounts = new List<Account>
        {
            new Account{ AccountId = 2344, FirstName = "Tommy", LastName = "Test"},
            new Account{ AccountId = 2233, FirstName = "Barry", LastName = "Test"},
            new Account{ AccountId = 8766, FirstName = "Sally", LastName = "Test"},
            new Account{ AccountId = 2345, FirstName = "Jerry", LastName = "Test"},
            new Account{ AccountId = 2346, FirstName = "Ollie", LastName = "Test"},
            new Account{ AccountId = 2347, FirstName = "Tara", LastName = "Test"},
            new Account{ AccountId = 2348, FirstName = "Tammy", LastName = "Test"},
            new Account{ AccountId = 2349, FirstName = "Simon", LastName = "Test"},
            new Account{ AccountId = 2350, FirstName = "Colin", LastName = "Test"},
            new Account{ AccountId = 2351, FirstName = "Gladys", LastName = "Test"},
            new Account{ AccountId = 2352, FirstName = "Greg", LastName = "Test"},
            new Account{ AccountId = 2353, FirstName = "Tony", LastName = "Test"},
            new Account{ AccountId = 2355, FirstName = "Arthur", LastName = "Test"},
            new Account{ AccountId = 2356, FirstName = "Craig", LastName = "Test"},
            new Account{ AccountId = 6776, FirstName = "Laura", LastName = "Test"},
            new Account{ AccountId = 4534, FirstName = "JOSH", LastName = "TEST"},
            new Account{ AccountId = 1234, FirstName = "Freya", LastName = "Test"},
            new Account{ AccountId = 1239, FirstName = "Noddy", LastName = "Test"},
            new Account{ AccountId = 1240, FirstName = "Archie", LastName = "Test"},
            new Account{ AccountId = 1241, FirstName = "Lara", LastName = "Test"},
            new Account{ AccountId = 1242, FirstName = "Tim", LastName = "Test"},
            new Account{ AccountId = 1243, FirstName = "Graham", LastName = "Test"},
            new Account{ AccountId = 1244, FirstName = "Tony", LastName = "Test"},
            new Account{ AccountId = 1245, FirstName = "Neville", LastName = "Test"},
            new Account{ AccountId = 1246, FirstName = "Jo", LastName = "Test"},
            new Account{ AccountId = 1247, FirstName = "Jim", LastName = "Test"},
            new Account{ AccountId = 1248, FirstName = "Pam", LastName = "Test"}
        };
        public IEnumerable<Account> GetAccounts()
        {
            return accounts;
        }

        public async Task<int> PersistMeterReadings(List<MeterReading> readings)
        {
            var readingsAdded = 0;
            foreach (var reading in readings.Where(a => accounts.Select(r => r.AccountId).Contains(a.AccountId)))
            {
                var account = accounts.FirstOrDefault(a => a.AccountId == reading.AccountId);
                account.AddMeterReading(reading);
                readingsAdded++;
            }
            return readingsAdded;
        }

        public IEnumerable<int> ReturnAccountIdsNotOnRecord(IEnumerable<int> accountIds)
        {
            return accountIds.Except(accounts.Select(a => a.AccountId));
        }
    }
}
