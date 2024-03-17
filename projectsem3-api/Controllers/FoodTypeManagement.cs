using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Crmf;
using projectsem3_api.Context;
using projectsem3_api.DTOs;
using projectsem3_api.Entities;
using projectsem3_api.Models;
using System.Reflection.Metadata.Ecma335;

namespace projectsem3_api.Controllers
{
    [ApiController]
    [Route("api/food")]
    public class FoodTypeManagement : Controller
    {
        private readonly DataContext _dbContext;
        public FoodTypeManagement(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                List<FoodTypeDTO> foodTypes = _dbContext.FoodTypes
                    .Select(add => new FoodTypeDTO
                    {
                        Id=add.Id,
                        Description=add.Description,
                        Name=add.Name,
                    }).ToList();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest("Get food error");
            }
           
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(FoodTypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    FoodType foodType = new FoodType()
                    {
                        Name= model.Name,
                        Description=model.Description,
                    };
                    _dbContext.FoodTypes.Add(foodType);
                    _dbContext.SaveChanges();
                    return Created("", new FoodTypeDTO
                    {
                        Id=model.Id,
                        Name=model.Name,
                        Description=model.Description,
                    });
                }catch(Exception ex)
                {
                    return BadRequest("Create is not successful");
                }
            }
            return Ok("create success");
        }
        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, FoodTypeModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    FoodType editFoodType = _dbContext.FoodTypes.Find(id);
                    if(editFoodType == null)
                    {
                        return BadRequest("Food Type does not exist");
                    }
                    editFoodType.Name = model.Name;
                    editFoodType.Description = model.Description;
                    return Ok("Editting success");
                }catch(Exception ex)
                {
                    return BadRequest("Edit error");
                }
            }
            return Ok("Editting success");
        }
    }
}
