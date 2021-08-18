using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EnergyReadings.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using CsvHelper;
using System.Globalization;
using System.Linq;
using EnergyReadings.Interfaces;
using EnergyReadings.Validators;

namespace EnergyReadings.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMeterReadingService _meterReadingService;

        public HomeController(IMeterReadingService csvFileReader)
        {
            _meterReadingService = csvFileReader;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost("/meter-reading-uploads")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MeterReadingUploadsResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MeterReadingUploadsResult))]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            var result = await _meterReadingService.UploadMeterReadings(files);

            if (result.SuccessCount == 0 && result.FailCount > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
