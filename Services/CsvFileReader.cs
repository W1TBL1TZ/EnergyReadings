using CsvHelper;
using EnergyReadings.Interfaces;
using EnergyReadings.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyReadings.Services
{
    public class CsvFileReader : ICsvFileReader
    {
        public void ConvertCsvFileToMeterReadingList(IFormFile file, out IList<CsvConversionError> errors, out List<MeterReading> meterReadings)
        {
            errors = new List<CsvConversionError>();
            meterReadings = new List<MeterReading>();
            using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
            CsvReader csvReader = new CsvReader(reader, CultureInfo.GetCultureInfo("en-AU"));

            var readings = csvReader.GetRecords<MeterReadingValidation>().ToList();
            try
            {
                foreach (var reading in readings)
                {
                    var meterReading = new MeterReading();

                    if (int.TryParse(reading.AccountId, out int accountId))
                    {
                        meterReading.AccountId = accountId;
                    }
                    else
                    {
                        var error = new CsvConversionError
                        {
                            ErrorMessage = $"Could not parse account id for: {reading.AccountId}"
                        };
                        errors.Add(error);
                        continue;
                    }

                    if (DateTime.TryParse(reading.MeterReadingDateTime, out DateTime meterReadingDateTime))
                    {
                        meterReading.MeterReadingDateTime = meterReadingDateTime;
                    }
                    else
                    {
                        var error = new CsvConversionError
                        {
                            AccountId = meterReading.AccountId,
                            ErrorMessage = $"Could not parse MeterReadingDateTime for: {reading.MeterReadingDateTime}"
                        };
                        errors.Add(error);
                        continue;
                    }

                    if (int.TryParse(reading.MeterReadValue, out int meterReadValue))
                    {
                        meterReading.MeterReadValue = meterReadValue;
                    }
                    else
                    {
                        var error = new CsvConversionError
                        {
                            AccountId = meterReading.AccountId,
                            ErrorMessage = $"Could not parse MeterReadValue for: {reading.MeterReadValue}"
                        };
                        errors.Add(error);
                        continue;
                    }

                    meterReadings.Add(meterReading);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
