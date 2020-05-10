namespace CarsApi.DTO.Models
{
    using System.Collections.Generic;

    public class CarWithPagingDTO
    {
        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int ItemsPerPage { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }

        public IEnumerable<CarDTO> Cars { get; set; }
    }
}
