using EnergyReadings.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyReadings.Validators
{
    public class MeterReadingsValidator : AbstractValidator<IEnumerable<MeterReading>>
    {
        public MeterReadingsValidator()
        {
            RuleFor(r => r).NotNull().Must(r => r.Any()).WithMessage("Meter readings must contain at least one value");
            RuleForEach(r => r).SetValidator(new MeterReadingValidator());
        }
    }
}
