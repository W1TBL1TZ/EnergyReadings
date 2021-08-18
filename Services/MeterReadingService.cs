using EnergyReadings.Interfaces;
using EnergyReadings.Models;
using EnergyReadings.Validators;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyReadings.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly ICsvFileReader _csvFileReader;
        private readonly IEnergyReadingRepository _energyReadingRepository;

        public MeterReadingService(ICsvFileReader csvFileReader, IEnergyReadingRepository energyReadingRepository)
        {
            _csvFileReader = csvFileReader;
            _energyReadingRepository = energyReadingRepository;
        }

        public async Task<MeterReadingUploadsResult> UploadMeterReadings(IEnumerable<IFormFile> files)
        {
            var result = new MeterReadingUploadsResult();
            var readingsToPersist = new List<MeterReading>();
            foreach (var file in files)
            {
                _csvFileReader.ConvertCsvFileToMeterReadingList(file, out IList<CsvConversionError> errors, out List<MeterReading> readings);
                result.FailCount += errors.Count;

                // Ensure that you cannot load the same entry twice
                // In a real world development environment, I would definitely have questioned the requirement of what defines the uniqueness of an entry.
                // Here I assumed that the combination of account id, reading value, and date time would form a composite key,
                // but I can definitely see multiple other definitions depending on the way the business would want to use this data.
                readings = readings.Distinct().ToList();

                // Validate the readings against a fluent validator that would encapsulate all the validation rules
                var validator = new MeterReadingsValidator();
                var validationResult = validator.Validate(readings);
                if (!validationResult.IsValid)
                {
                    // If the reading is not vaild, increase the fail count and remove it from the list to be persisted
                    result.FailCount += validationResult.Errors.Count;
                    var invalidAccounts = validationResult.Errors.Select(e => ((MeterReading)e.AttemptedValue));
                    foreach (var invalidAccount in invalidAccounts)
                    {
                        readings.Remove(invalidAccount);
                    }
                    
                }
                // Add the readings that passed validation into the list that will be persisted
                readingsToPersist.AddRange(readings);
            }
            //Persist the validated readings
            var persistanceResult = await SaveReadingsToStorage(readingsToPersist);

            result.FailCount += persistanceResult.FailCount;
            result.SuccessCount += persistanceResult.SuccessCount;

            return result;
        }

        private async Task<MeterReadingUploadsResult> SaveReadingsToStorage(List<MeterReading> readings)
        {
            var result = new MeterReadingUploadsResult();

            // Send the account Ids of the readings to be saved to the database to compare with the account Ids that we have on record
            var accountIdsNotFound = _energyReadingRepository.ReturnAccountIdsNotOnRecord(readings.Select(r => r.AccountId));

            // Update the required variables
            result.FailCount = accountIdsNotFound.Count();
            readings.RemoveAll(r => accountIdsNotFound.Contains(r.AccountId));

            // Finally, we persist all the valid meter readings
            result.SuccessCount = await _energyReadingRepository.PersistMeterReadings(readings);

            return result;
        }
    }
}
