namespace CarsApi.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CarsApi.DTO.Models;

    public interface ICarService
    {
        IQueryable<CarDTO> All();

        List<CarDTO> AllSorted(bool sortByBrandAsc);

        CarWithPagingDTO AllByPage(int page, int pageSize, IQueryable<CarDTO> cars);

        List<CarDTO> FilterByYearsOfProduction(int? fromYear, int? toYear);

        CarWithPagingDTO AllWithOptions(CarOptionsDTO options);

        CarDetailsDTO Details(int id);

        Task AddAsync(CarDTO car);

        Task EditAsync(CarDTO car);

        Task DeleteAsync(int id);
    }
}
