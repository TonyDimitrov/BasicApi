namespace CarsApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CarsApi.Data;
    using CarsApi.Data.Models;
    using CarsApi.DTO.Models;

    public class CarsService : ICarService
    {
        private readonly IEntityIRepository<Car> carRepository;

        public CarsService(IEntityIRepository<Car> carRepository)
        {
            this.carRepository = carRepository;
        }

        public IQueryable<CarDTO> All()
        {
            return this.carRepository.All()
                .Select(c => new CarDTO
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    YearOfProduction = c.YearOfProduction,
                });
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

            return this.carRepository.All()
                .Where(c => c.YearOfProduction.Year >= from && c.YearOfProduction.Year <= to)
                .Select(c => new CarDTO
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    YearOfProduction = c.YearOfProduction,
                })
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
            return this.carRepository.All()
                .Where(c => c.Id == id)
                .Select(c => new CarDetailsDTO
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    YearOfProduction = c.YearOfProduction,
                    CreatedOn = c.CreatedOn,
                })
                .FirstOrDefault();
        }

        public async Task AddAsync(CarDTO car)
        {
            await this.carRepository.AddAsync(new Car
            {
                CreatedOn = DateTime.UtcNow,
                Brand = car.Brand,
                YearOfProduction = car.YearOfProduction,
            });
            await this.carRepository.SaveChangesAsync();
        }

        public async Task EditAsync(CarDTO car)
        {
            this.carRepository.Edit(new Car
            {
                Id = car.Id,
                CreatedOn = DateTime.UtcNow,
                Brand = car.Brand,
                YearOfProduction = car.YearOfProduction,
            });
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
