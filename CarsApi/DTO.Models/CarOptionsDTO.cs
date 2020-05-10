namespace CarsApi.DTO.Models
{
    using System.ComponentModel.DataAnnotations;

    using CarsApi.CustomAttributes;

    public class CarOptionsDTO
    {
        public CarOptionsDTO()
        {
            this.Page = 1;
            this.PageSize = 10;
        }

        [Range(1, 100_000)]
        public int Page { get; set; }

        [Range(1, 100)]
        public int PageSize { get; set; }

        public int ByBrand { get; set; }

        [RangeYear]
        public int? FromYear { get; set; }

        [RangeYear]
        public int? ToYear { get; set; }

        public bool ValidYearsRange => this.FromYear < this.ToYear;
    }
}
