namespace CarsApi.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CarsApi.CustomAttributes;

    public class Car
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string Brand { get; set; }

        [RangeDateTimeAttribute]
        public DateTime YearOfProduction { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
