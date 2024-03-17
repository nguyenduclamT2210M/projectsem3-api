using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectsem3_api.Context;
using projectsem3_api.DTOs;
using projectsem3_api.Entities;
using projectsem3_api.Models;
using System.Data;

namespace projectsem3_api.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleManagementController : Controller
    {
        private readonly DataContext _dbContext;

        public RoleManagementController(DataContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                List<RoleDTO> roles = _dbContext.Roles
                    .Select(role => new RoleDTO()
                    {
                        Id = role.Id,
                        Name = role.Name
                    })
                    .ToList();

                return Ok(roles);
            }
            catch (Exception e)
            {
                return BadRequest("Get Role error");
            }

        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(RoleModel role)
        {
            if (ModelState.IsValid)
            { 
                try
                {
                    Role addRole = new Role
                    {
                        Name = role.Name
                    };
                    _dbContext.Roles.Add(addRole);
                    _dbContext.SaveChanges();
                    return Created("", new RoleDTO
                    {
                        Id =role.Id,
                        Name = role.Name
                    });
                }
                catch(Exception e)
                {
                    return BadRequest("Create fales");
                }
            }
            return BadRequest("Done");
        }
        [HttpPut]
        [Route("edit/{id}")]
        public IActionResult Edit(int id,RoleModel role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Role updateRole = _dbContext.Roles.Find(id);
                    if (updateRole == null)
                    {
                        return NotFound("Role Not found.");
                    }
                    updateRole.Name = role.Name;
                    
                    _dbContext.Update(updateRole);
                    _dbContext.SaveChanges();
                    return Ok("Editting success");
                }
                catch (Exception ex)
                {
                    return BadRequest("Editting is not successful");
                }
            }
            return BadRequest("Editting success");
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Role role = _dbContext.Roles.Find(id);
                _dbContext.Roles.Remove(role);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

