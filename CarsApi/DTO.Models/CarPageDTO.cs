namespace CarsApi.DTO.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CarPageDTO
    {
        public CarPageDTO()
        {
            this.Page = 1;
            this.PageSize = 10;
        }

        [Range(1, 100_000)]
        public int Page { get; set; }

        [Range(1, 100)]
        public int PageSize { get; set; }
    }
}
