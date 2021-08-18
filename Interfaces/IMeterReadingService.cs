using EnergyReadings.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyReadings.Interfaces
{
    public interface IMeterReadingService
    {
        Task<MeterReadingUploadsResult> UploadMeterReadings(IEnumerable<IFormFile> files);
    }
}
