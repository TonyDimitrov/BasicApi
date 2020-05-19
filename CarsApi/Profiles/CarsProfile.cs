namespace CarsApi.Profiles
{
    using AutoMapper;
    using CarsApi.Data.Models;
    using CarsApi.DTO.Models;

    public class CarsProfile : Profile
    {
        public CarsProfile()
        {
            this.CreateMap<Car, CarDTO>();
            this.CreateMap<CarDTO, Car>();
            this.CreateMap<Car, CarDetailsDTO>();
        }
    }
}
