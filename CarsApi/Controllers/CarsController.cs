namespace CarsApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CarsApi.DTO.Models;
    using CarsApi.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService carService;
        private readonly IMemoryCache memoryCache;

        public CarsController(ICarService carService, IMemoryCache memoryCache)
        {
            this.carService = carService;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CarDTO>> Get()
        {
            if (!this.memoryCache.TryGetValue<List<CarDTO>>("all-cars", out var allCars))
            {
                allCars = this.carService.All().ToList();
                this.memoryCache.Set("all-cars", allCars, TimeSpan.FromSeconds(20));
            }

            return allCars;
        }

        [HttpGet("{id}")]
        public ActionResult<CarDetailsDTO> Get(int id)
        {
            var result = this.carService.Details(id);

            if (result != null)
            {
                return result;
            }

            return this.NotFound();
        }

        [HttpGet("filter/{fromYear?}/{toYear?}")]
        public ActionResult<IEnumerable<CarDTO>> Get([FromQuery] CarYearsFilterDTO years)
        {
            if (!years.ValidYearsRange)
            {
                return this.BadRequest();
            }

            return this.carService.FilterByYearsOfProduction(years.FromYear, years.ToYear);
        }

        [HttpGet("sort/{byBrandAsc?}")]
        public ActionResult<IEnumerable<CarDTO>> Get(bool byBrandAsc = true)
        {
            return this.carService.AllSorted(byBrandAsc);
        }

        [HttpGet("page/{page?}/{pageSize?}")]
        public ActionResult<CarWithPagingDTO> Get([FromQuery] CarPageDTO carPage)
        {
            var cars = this.carService.All();
            return this.carService.AllByPage(carPage.Page, carPage.PageSize, cars);
        }

        [HttpGet("options/{byBrand?}/" +
            "{page?}/{pageSize?}/" +
            "{fromYear?}/{toYear?}")]
        public ActionResult<CarWithPagingDTO> Get([FromQuery] CarOptionsDTO options)
        {
            return this.carService.AllWithOptions(options);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CarDTO car)
        {
            await this.carService.AddAsync(car);

            return this.Created($"car/{car.Id}", car);
        }

        [HttpPut]
        public async Task<ActionResult> Put(CarDTO car)
        {
            await this.carService.EditAsync(car);

            return this.Ok(204);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await this.carService.DeleteAsync(id);

            return this.Ok(204);
        }
    }
}
