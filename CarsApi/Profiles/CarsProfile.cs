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
            this.CreateMap<CarUpdateDTO, Car>();
            this.CreateMap<Car, CarUpdateDTO>();
            this.CreateMap<CarDTO, CarUpdateDTO>();
            this.CreateMap<CarUpdateDTO, CarDTO>();
        }
    }
}
