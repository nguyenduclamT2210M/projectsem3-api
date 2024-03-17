using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectsem3_api.Context;
using projectsem3_api.DTOs;
using projectsem3_api.Entities;
using projectsem3_api.Models;

namespace projectsem3_api.Controllers
{
    [ApiController]
    [Route("/api/cart")]
    public class CartManagementController : Controller
    {
        private readonly DataContext _dataContext;
        public CartManagementController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            List<CartDTO> indexCart = _dataContext.Carts
                .Include(a => a.UserDTO)
                .Select(a => new CartDTO
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Quantity = a.Quantity,
                    User = _dataContext.Users
                    .Where(add => add.id == a.UserId)
                    .Select(add => new User()
                    {
                        id = add.id,
                        fullname = add.fullname,
                        email = add.email,
                        telephone = add.telephone

                    }).FirstOrDefault()

                }).ToList();
            return Ok(indexCart);
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(CartModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Cart newCart = new Cart()
                    {
                        UserId = model.UserId,
                        Quantity = model.Quantity,
                    };
                    _dataContext.Carts.Add(newCart);
                    _dataContext.SaveChanges();
                    return Created("", new CartDTO
                    {
                        Id = model.Id,
                        UserId = model.UserId,
                        Quantity = model.Quantity,
                    });

                } catch (Exception ex)
                {
                    return BadRequest("Create is not successful");
                }

            }
            return BadRequest("Create success");
        }
        [HttpPut]
        [Route("edit/{id}")]
        public IActionResult Edit(int id,CartModel cartModel)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    Cart editCart = _dataContext.Carts.Find(id);
                    if(editCart == null)
                    {
                        return BadRequest("Cart does not exist");
                    }
                  
                    editCart.UserId = cartModel.UserId;
                    editCart.Quantity = cartModel.Quantity;
                    _dataContext.Update(editCart);
                    _dataContext.SaveChanges();
                    return Ok("Editting success");
                } catch (Exception ex)
                {
                    return BadRequest("Editting is not successful");
                }
            }

            return BadRequest("Editting is not success");
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Cart deletCart = _dataContext.Carts.Find(id);
                if (deletCart == null)
                {
                    return BadRequest("Cart does not exist");
                }
                _dataContext.Carts.Remove(deletCart);
                _dataContext.SaveChanges();
                return Ok("Delete success");
            }
            catch (Exception ex)
            {
                return BadRequest("Delete is not successful");
            }
        }
    }
}
