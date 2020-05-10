namespace CarsApi.CustomAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RangeDateTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            if (date.Year < GlobalConstants.MinYearValue || date.Year > DateTime.Now.Year)
            {
                return new ValidationResult($"Invalid date {date}");
            }

            if (date.Year == DateTime.Now.Year && date.DayOfYear > DateTime.Now.DayOfYear)
            {
                return new ValidationResult($"Invalid date {date}");
            }

            return ValidationResult.Success;
        }
    }
}
