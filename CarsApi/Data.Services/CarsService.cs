namespace CarsApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using CarsApi.Data;
    using CarsApi.Data.Models;
    using CarsApi.DTO.Models;

    public class CarsService : ICarService
    {
        private readonly IEntityIRepository<Car> carRepository;
        private readonly IMapper mapper;

        public CarsService(IEntityIRepository<Car> carRepository, IMapper mapper)
        {
            this.carRepository = carRepository;
            this.mapper = mapper;
        }

        public IQueryable<CarDTO> All()
        {
            return this.mapper.Map<IEnumerable<CarDTO>>(this.carRepository.All().AsEnumerable()).AsQueryable();
        }

        public List<CarDTO> AllSorted(bool sortByBrandAsc)
        {
            if (sortByBrandAsc)
            {
                return this.All()
                     .OrderBy(c => c.Brand)
                     .ToList();
            }
            else
            {
                return this.All()
                     .OrderByDescending(c => c.Brand)
                     .ToList();
            }
        }

        public List<CarDTO> FilterByYearsOfProduction(int? fromYear, int? toYear)
        {
            int from = fromYear != null ? fromYear.Value : GlobalConstants.MinYearValue;
            int to = toYear != null ? toYear.Value : DateTime.Now.Year;

            return this.mapper.Map<IEnumerable<CarDTO>>(this.carRepository.All()
                .Where(c => c.YearOfProduction.Year >= from && c.YearOfProduction.Year <= to))
                .ToList();
        }

        public CarWithPagingDTO AllByPage(int page, int pageSize, IQueryable<CarDTO> cars)
        {
            var totalItems = cars.Count();

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var isFirstPage = page == 1 ? true : false;

            var isLastPage = page == totalPages ? true : false;

            var skipItems = (page * pageSize) - pageSize;

            var pageCars = cars
             .Skip(skipItems)
             .Take(pageSize)
             .ToList();

            return new CarWithPagingDTO
            {
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = totalPages,
                ItemsPerPage = pageSize,
                IsFirstPage = isFirstPage,
                IsLastPage = isLastPage,
                Cars = pageCars,
            };
        }

        public CarWithPagingDTO AllWithOptions(CarOptionsDTO options)
        {
            var filteredByYears = this.FilterByYearsOfProduction(options.FromYear, options.ToYear)
                .AsQueryable();

            IQueryable<CarDTO> sorted = null;
            if (options.ByBrand == 1)
            {
                sorted = filteredByYears
                     .OrderBy(c => c.Brand);
            }
            else if (options.ByBrand == 2)
            {
                sorted = filteredByYears
                       .OrderByDescending(c => c.Brand);
            }

            var carsWithPages = this.AllByPage(options.Page, options.PageSize, sorted ?? filteredByYears);

            return carsWithPages;
        }

        public CarDetailsDTO Details(int id)
        {
           return this.mapper.Map<CarDetailsDTO>(this.carRepository.All()
                .Where(c => c.Id == id)
                .FirstOrDefault());
        }

        public async Task AddAsync(CarDTO car)
        {
            await this.carRepository.AddAsync(this.mapper.Map<Car>(car));
            await this.carRepository.SaveChangesAsync();
        }

        public async Task EditAsync(CarDTO car)
        {
            this.carRepository.Edit(this.mapper.Map<Car>(car));
            await this.carRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var carToDelete = this.carRepository.All()
                .FirstOrDefault(c => c.Id == id);

            this.carRepository.Delete(carToDelete);
            await this.carRepository.SaveChangesAsync();
        }
    }
}
