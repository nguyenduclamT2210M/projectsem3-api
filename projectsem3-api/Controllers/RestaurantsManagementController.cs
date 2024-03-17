using Microsoft.AspNetCore.Mvc;
using projectsem3_api.Context;
using projectsem3_api.Entities;
using projectsem3_api.DTOs;
using Microsoft.EntityFrameworkCore;
using projectsem3_api.Models;
using System.Reflection;

namespace projectsem3_api.Controllers
{
    [ApiController]
    [Route("/api/restaurant")]
    public class RestaurantsManagementController  : Controller
    {
        private readonly DataContext _dataContext;
        public RestaurantsManagementController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            List<RestaurantDTO> restauranDTOs = _dataContext.Restaurants
                .Include(x => x.City)
                .Include(x => x.Admin)
                .Include(x => x.Category)
                .Select(x => new RestaurantDTO
                {
                    Id = x.Id,
                    AdminId = x.AdminId,
                    CatId = x.CatId,
                    CityId = x.CityId,
                    Address = x.Address,
                    JoinDate = x.JoinDate,
                    DeliveryHours = x.DeliveryHours,
                    Thumbnail = x.Thumbnail,
                    Name = x.Name,
                    MinimumDelivery = x.MinimumDelivery,
                    Description = x.Description,
                    Banner = x.Banner,
                    Admin = _dataContext.Admins
                    .Where(add => add.Id == add.Id)
                    .Select(add => new Admin
                    {
                        Id = add.Id,
                        FullName = add.FullName,
                        Email = add.Email,
                        Username = add.Username,
                        Telephone = add.Telephone,
                        Role = _dataContext.Roles
                        .Where(addRole => addRole.Id == addRole.Id)
                        .Select(addRole => new Role
                        {
                            Id = addRole.Id,
                            Name = addRole.Name
                        }).FirstOrDefault()
                    }).FirstOrDefault(),
                    City = _dataContext.Cities
                    .Where(addCity => addCity.Id == addCity.Id)
                    .Select(addCity => new City
                    {
                        Id = addCity.Id,
                        Name = addCity.Name,
                        Thumbnail = addCity.Thumbnail
                    }).FirstOrDefault(),
                    Category = _dataContext.Categories
                    .Where(addCategory => addCategory.Id == addCategory.Id)
                    .Select(addCategory => new Category
                    {
                        Id= addCategory.Id,
                        Name = addCategory.Name,
                        Description = addCategory.Description,
                        
                    }).FirstOrDefault()
                }).ToList();
            return Ok(restauranDTOs);
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(RestauranModel restauranModel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    Restaurant addRetaurant = new Restaurant()
                    {
                        Name = restauranModel.Name,
                        AdminId = restauranModel.AdminId,
                        CatId = restauranModel.CatId,
                        CityId=restauranModel.CityId,
                        Address = restauranModel.Address,
                        Banner = restauranModel.Banner,
                        DeliveryHours = restauranModel.DeliveryHours,
                        Description = restauranModel.Description,
                        MinimumDelivery = restauranModel.MinimumDelivery,
                        JoinDate = restauranModel.JoinDate,
                        Thumbnail = restauranModel.Thumbnail,

                    };
                    _dataContext.Restaurants.Add(addRetaurant);
                    _dataContext.SaveChanges();
                    return Created("", new RestaurantDTO
                    {
                        Id =  restauranModel.Id,
                        Name = restauranModel.Name,
                        AdminId = restauranModel.AdminId,
                        CatId = restauranModel.CatId,
                        CityId = restauranModel.CityId,
                        Address = restauranModel.Address,
                        Banner = restauranModel.Banner,
                        DeliveryHours = restauranModel.DeliveryHours,
                        Description = restauranModel.Description,
                        MinimumDelivery = restauranModel.MinimumDelivery,
                        JoinDate = restauranModel.JoinDate,
                        Thumbnail = restauranModel.Thumbnail,
                    });
                }catch (Exception ex)
                {
                    return BadRequest("Create error");
                }
            }
            return Ok("Create success");
        }
        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(RestauranModel restauranModel, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Restaurant editRestaurant = _dataContext.Restaurants.Find(id);
                    if(editRestaurant == null)
                    {
                        return BadRequest("Restaurant not found.");
                    }
                    editRestaurant.Name = restauranModel.Name;
                    editRestaurant.AdminId = restauranModel.AdminId;
                    editRestaurant.CatId = restauranModel.CatId;
                    editRestaurant.CityId = restauranModel.CityId;
                    editRestaurant.Address = restauranModel.Address;
                    editRestaurant.Banner = restauranModel.Banner;
                    editRestaurant.DeliveryHours = restauranModel.DeliveryHours;
                    editRestaurant.Description = restauranModel.Description;
                    editRestaurant.MinimumDelivery = restauranModel.MinimumDelivery;
                    editRestaurant.JoinDate = restauranModel.JoinDate;
                    editRestaurant.Thumbnail = restauranModel.Thumbnail;
                    _dataContext.Update(editRestaurant);
                    _dataContext.SaveChanges();
                    return Ok("Edit success");
                }
                catch (Exception ex)
                {
                    return BadRequest("Edit error");
                }
            }
            return Ok("Edit Success");
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Restaurant deleteRestaurant = _dataContext.Restaurants.Find(id);
                if(deleteRestaurant == null)
                {
                    return BadRequest("Restaurant not found.");
                }
                _dataContext.Restaurants.Remove(deleteRestaurant);
                return Ok("Delete success.");
            }
            catch (Exception ex)
            {
                return BadRequest("Delete ");
            }
            
        }
    }
}
