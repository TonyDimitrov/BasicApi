﻿namespace CarsApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using CarsApi.DTO.Models;
    using CarsApi.Helpers;
    using CarsApi.Services;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService carService;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper mapper;

        public CarsController(
            ICarService carService,
            IMemoryCache memoryCache,
            IMapper mapper)
        {
            this.carService = carService;
            this.memoryCache = memoryCache;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<CarDTO>> Get()
        {
            if (!this.memoryCache.TryGetValue<List<CarDTO>>("all-cars", out var allCars))
            {
                allCars = this.carService.All().ToList();
                this.memoryCache.Set("all-cars", allCars, TimeSpan.FromSeconds(5));
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

        [HttpGet("({ids})")]
        public ActionResult<IEnumerable<CarDTO>> Get(
        [FromRoute]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        IEnumerable<int> ids)
        {
            if (ids == null)
            {
                return this.BadRequest();
            }

            var cars = this.carService.All(ids);
            if (ids.Count() != cars.Count)
            {
                return this.NotFound();
            }

            return this.Ok(cars);
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

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<CarUpdateDTO> patchDocument)
        {
            var dbCourse = this.carService.All().Where(c => c.Id == id).FirstOrDefault();

            if (dbCourse == null)
            {
                return this.NotFound();
            }

            var courseToPatch = this.mapper.Map<CarUpdateDTO>(dbCourse);

            patchDocument.ApplyTo(courseToPatch);

            if (!this.TryValidateModel(courseToPatch))
            {
                return this.ValidationProblem(this.ModelState);
            }

            var updatedCourse = this.mapper.Map(courseToPatch, dbCourse);

            await this.carService.EditPartlyAsync(updatedCourse);

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var car = this.carService.Details(id);

            if (car == null)
            {
                return this.NotFound();
            }

            await this.carService.DeleteAsync(id);

            return this.NoContent();
        }
    }
}
