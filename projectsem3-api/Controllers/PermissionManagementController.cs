using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectsem3_api.Context;
using projectsem3_api.DTOs;
using projectsem3_api.Entities;
using projectsem3_api.Models;

namespace projectsem3_api.Controllers
{
    [ApiController]
    [Route("api/permission")]
    public class PermissionManagementController : Controller
    {
        private readonly DataContext _dbContext;

        public PermissionManagementController(DataContext context)
        {
            _dbContext = context;
        }
        [HttpGet]
        [Route("index")]
        public ActionResult Index()
        {
            try
            {
                List<PermissionDTO> permissions = _dbContext.Permissions
                     .Include(ad => ad.Role)
                    .Select(ad => new PermissionDTO()
                    {
                        Id = ad.Id,
                        Name = ad.Name,
                        FaIcon = ad.FaIcon,
                        Prefix = ad.Prefix,
                        Role = new RoleDTO()
                        {
                            Id = ad.Role.Id,
                            Name = ad.Role.Name,
                        }
                    })
                    .ToList();

                return Ok(permissions);
            }
            catch (Exception e)
            {
                return BadRequest("Get Role error");
            }
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(PermissionModel permission)
        {
            if (ModelState.IsValid)
            {
                Role role = _dbContext.Roles.Find(permission.RoleId);
                if (role == null)
                {
                    return NotFound("Role not found");
                }
                try
                {
                   Permission addPermission = new Permission
                   {
                       Name = permission.Name,
                       FaIcon = permission.FaIcon,
                       Prefix = permission.Prefix,
                       RoleId = role.Id,
                   };
                    _dbContext.Add(addPermission);
                    _dbContext.SaveChanges();
                    return Created("", new PermissionDTO
                    {
                        Name = addPermission.Name,
                        FaIcon = addPermission.FaIcon,
                        Prefix = addPermission.Prefix,
                        
                    });
                }
                catch (Exception e)
                {
                    return BadRequest("Create fales");
                }
            }
            return BadRequest("Done");
        }
        [HttpPut]
        [Route("edit/{id}")]
        public IActionResult Edit(int id ,PermissionModel model) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Permission updatePermission = _dbContext.Permissions.Find(id);
                    if (updatePermission == null)
                    {
                        return NotFound("Permission Not found.");
                    }
                    updatePermission.Name = model.Name;
                    updatePermission.Prefix = model.Prefix;
                    updatePermission.FaIcon= model  .FaIcon;
                    updatePermission.RoleId= model.RoleId;
                    _dbContext.Update(updatePermission);
                    _dbContext.SaveChanges();
                    return Ok("Editting success");
                }
                catch(Exception e)
                {
                    return BadRequest("Editting is not Successful");
                }
            }
           return BadRequest("Editting is not Successful");
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Permission per = _dbContext.Permissions.Find(id);
                _dbContext.Permissions.Remove(per);
                return Ok("Delete success");
            }
            catch (Exception ex)
            {
                return BadRequest("Delete is not successful");
            }

        }
    }
}
