namespace CarsApi.DTO.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CarsApi.CustomAttributes;

    public class CarUpdateDTO
    {
        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string Brand { get; set; }

        [RangeDateTimeAttribute]
        public DateTime YearOfProduction { get; set; }
    }
}
