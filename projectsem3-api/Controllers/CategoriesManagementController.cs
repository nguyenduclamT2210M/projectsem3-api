using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectsem3_api.Context;
using projectsem3_api.DTOs;
using projectsem3_api.Entities;
using projectsem3_api.Models;
using System.Reflection.Metadata.Ecma335;

namespace projectsem3_api.Controllers
{
    [ApiController]
    [Route("/api/category")]
    public class CategoriesManagementController : Controller
    {
       private readonly DataContext _dataContext;
        public CategoriesManagementController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            List<CategoriesDTO> listCategories = _dataContext.Categories
                .Select(category => new CategoriesDTO()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                }).ToList();
            return Ok(listCategories);
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(CategoriesModel newCategoriesModel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if (_dataContext.Categories.Any(category => category.Name == newCategoriesModel.Name))
                    {
                        return BadRequest("Name is already exist");
                    }
                    Category newCategory = new Category()
                    {
                        Name = newCategoriesModel.Name,
                        Description = newCategoriesModel.Description,
                    };
                    _dataContext.Categories.Add(newCategory);
                    _dataContext.SaveChanges();
                    return Created("", new CategoriesDTO()
                    {
                        Id = newCategory.Id,
                        Name = newCategoriesModel.Name,
                        Description = newCategoriesModel.Description,
                    });
                }catch(Exception ex)
                {
                    return BadRequest("Create is not successful");
                }
            }
            return BadRequest("Create success");
        }
        [HttpPut]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, CategoriesModel editcategoriesModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category category = _dataContext.Categories.Find(id);
                    if(category == null)
                    {
                        return BadRequest("Category not found");
                    }
                    category.Name = editcategoriesModel.Name;
                    category.Description = editcategoriesModel.Description;
                    _dataContext.Update(category);
                    _dataContext.SaveChanges();
                    return Ok("Editting success");
                   
                }catch(Exception ex)
                {
                    return BadRequest("Edit is not successful");
                }
            }
            return BadRequest("Update category error.");
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                Category deleteCategory = _dataContext.Categories.Find(id);
                if (deleteCategory == null)
                {
                    return BadRequest("Category not found");
                }
                _dataContext.Categories.Remove(deleteCategory);
                _dataContext.SaveChanges();
                return Ok("Delete success");
            }
            catch (Exception ex)
            {
                return BadRequest("Delete is not seccessful");
            }

        }
    }
}
