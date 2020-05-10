namespace CarsApi.DTO.Models
{
    using CarsApi.CustomAttributes;

    public class CarYearsFilterDTO
    {
        [RangeYear]
        public int? FromYear { get; set; }

        [RangeYear]
        public int? ToYear { get; set; }

        // As each parameter is optional if both are NOT provided we set true
        public bool ValidYearsRange => !this.FromYear.HasValue || !this.ToYear.HasValue
            ? true : this.FromYear.Value < this.ToYear.Value ? true : false;
    }
}
