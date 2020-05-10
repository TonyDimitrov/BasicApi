namespace CarsApi.DTO.Models
{
    using System.Collections.Generic;

    public class CarsDTO
    {
        public IEnumerable<CarDTO> Cars { get; set; }
    }
}
