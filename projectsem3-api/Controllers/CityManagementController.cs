using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using projectsem3_api.Context;
using projectsem3_api.DTOs;
using projectsem3_api.Entities;
using projectsem3_api.Models;
using System.Reflection.Metadata.Ecma335;

namespace projectsem3_api.Controllers
{
    [ApiController]
    [Route("/api/cities")]
    public class CityManagementController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CityManagementController(DataContext context, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
          List<CityDTO> listCities = _dbContext.Cities
                .Select( a => new CityDTO()
                {
                    Id=a.Id,
                    Name=a.Name,
                }).ToList();
            return Ok(listCities);
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(CityModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    City addCity = new City
                    {
                        Name = model.Name,
                        Thumbnail = model.Thumbnail,
                    };
                    _dbContext.Cities.Add(addCity);
                    _dbContext.SaveChanges();
                    return Created("", new CityDTO
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Thumbnail = model.Thumbnail,
                    });
                }
                catch (DbUpdateException ex)
                {
                    // Hiển thị thông điệp lỗi từ inner exception
                    var innerException = ex.InnerException;
                    while (innerException != null)
                    {
                        Console.WriteLine(innerException.Message);
                        innerException = innerException.InnerException;
                    }

                }
            }
            return BadRequest("Create is not success");
        }
        [HttpPut]
        [Route("edit/{id}")]
        public IActionResult EditCity(int id , CityModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    City editCity = _dbContext.Cities.Find(id);
                    if(editCity == null) 
                    {
                        return BadRequest("City not found");
                    }
                    editCity.Name = model.Name;
                    _dbContext.Update(editCity);
                    _dbContext.SaveChanges();
                    return Ok("Editting seccess");
                }catch (Exception ex)
                {
                    return BadRequest("Editting is not Seccessful");
                }
            }
            return BadRequest("Edit is not success");
        }
        [HttpDelete]
        public IActionResult DeleteCity(int id )
        {
            try
            {
                City deleteCity = _dbContext.Cities.Find(id);
                if(deleteCity == null)
                {
                    return BadRequest("City not found");
                }
                _dbContext.Cities.Remove(deleteCity);
                _dbContext.SaveChanges();
                return BadRequest("Delete success");
            }catch (Exception ex)
            {
                return BadRequest("Delete is not seccessful");
            }

        }
    }
}
