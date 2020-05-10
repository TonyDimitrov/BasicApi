namespace CarsApi.CustomAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RangeYearAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var year = (int?)value;

            if (year == null)
            {
                return ValidationResult.Success;
            }

            if (year.Value < GlobalConstants.MinYearValue || year.Value > DateTime.Now.Year)
            {
                return new ValidationResult($"Invalid year {year.Value}");
            }

            return ValidationResult.Success;
        }
    }
}
