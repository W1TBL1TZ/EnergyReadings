using EnergyReadings.Models;
using FluentValidation;

namespace EnergyReadings.Validators
{
    public class MeterReadingValidator : AbstractValidator<MeterReading>
    {
        public MeterReadingValidator()
        {
            RuleFor(r => r).NotNull().Must(r => r.MeterReadValue >= 0).WithMessage("Meter readings cannot be a negative value");
            RuleFor(r => r).Must(r => r.MeterReadValue.ToString().Length <= 5).WithMessage("Meter readings can have amaximum lenght of 5 numbers");
            // If there were any further validations to be done or the model extended with more properties and validations, 
            // this is where it would probably happen.
        }
    }
}
