using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using projectsem3_api.Context;
using projectsem3_api.DTOs;
using projectsem3_api.Entities;
using projectsem3_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace projectsem3_api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountSettingController : Controller
    {
        private readonly DataContext dbContext;
        private IConfiguration configuration;

        public AccountSettingController(DataContext context, IConfiguration config)
        {
            dbContext = context;
            configuration = config;
        }

        
        private string GenJWT(User user)
        {

            string key = configuration["JWT:Key"];
            int lifeTime = Convert.ToInt32(configuration["JWT:Lifetime"]);
            string issuer = configuration["JWT:Issuer"];
            string audience = configuration["JWT:Audience"];

            var securityKey = new byte[32];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(securityKey);
            }

            var signatureKey = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256);
            var payload = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.id.ToString()),
                new Claim(ClaimTypes.Email,user.email),
                new Claim(ClaimTypes.Name,user.fullname),

            };

            var token = new JwtSecurityToken(
                    issuer,
                    audience,
                    payload,
                    expires: DateTime.Now.AddMinutes(lifeTime),
                    signingCredentials: signatureKey
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserModel regModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (dbContext.Users.Any(user => user.email == regModel.Email))
                    {
                        return Unauthorized("Email is already exist. Please enter another email.");
                    }

                    string salt = BCrypt.Net.BCrypt.GenerateSalt(10);
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(regModel.Password, salt);

                    User newUser = new User
                    {
                        fullname = regModel.FullName,
                        email = regModel.Email,
                        telephone = regModel.Telephone,
                        password = hashedPassword
                    };

                    dbContext.Users.Add(newUser);
                    dbContext.SaveChanges();
                    return Created("", new UserDTO
                    {
                        id = newUser.id,
                        fullname = newUser.fullname,
                        email = newUser.email,
                        token = null
                    });
                }
                catch (Exception e)
                {
                    return Unauthorized("Registration error.");
                }
            }
            return Unauthorized("Registration error.");
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized("Email or password is incorrect");
            }
            try
            {
                var user = dbContext.Users.Where(u => u.email.Equals(model.Email)).First();
                if (user == null)
                {
                    throw new Exception("Email or password is incorrect");
                }
                bool verified = BCrypt.Net.BCrypt.Verify(model.Password, user.password);
                if (!verified)
                {
                    throw new Exception("Email or password is incorrect");
                }
                return Ok(new UserDTO
                {
                    id = user.id,
                    email = user.email,
                    fullname = user.fullname,
                    token = GenJWT(user)
                });
            }
            catch (Exception e)
            {
                return Unauthorized("Email or password is incorrect");
            }
        }

        [HttpPost]
        [Route("changepassword")]
        [Authorize]
        public IActionResult ChangePassword(ChangePasswordModel changePwdModel)
        {
            var client = HttpContext.User;
            var username = client.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;

            User updateClient = dbContext.Users.SingleOrDefault(e => e.email == username);

            if (updateClient != null)
            {
                bool passwordMatch = BCrypt.Net.BCrypt.Verify(changePwdModel.currentPassword, updateClient.password);

                if (passwordMatch)
                {
                    updateClient.password = BCrypt.Net.BCrypt.HashPassword(changePwdModel.newPassword);
                    dbContext.SaveChanges();
                    return Ok(new
                    {
                        Message = "Password change",
                        username = username
                    }
                    );
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }

        }
        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            List<UserDTO> listUser = dbContext.Users
                .Select( a => new UserDTO()
                {
                    id = a.id,
                    email = a.email,
                    fullname = a.fullname,
                    
                }).ToList();
            return Ok(listUser);
        }
        
        [HttpPut]
        [Route("edit/{id}")]
        public IActionResult Edit(UserModel model)
        {
              if(ModelState.IsValid)
              {
                    try
                    {
                    dbContext.Users.Update(new Entities.User
                    {
                        fullname = model.FullName,
                        email = model.Email,
                        telephone = model.Telephone,
                    });
                    }catch(Exception ex)
                    {
                    return BadRequest("Editing is not successful!");
                    }
              }
              return BadRequest("Edit successful");
        }
        [HttpPost]
        [Route("delete/{id}")]
        public IActionResult DeleteAccount (int id)
        {
            try
            {
                User deleteUser = dbContext.Users.Find(id);
                if (deleteUser == null)
                {
                    return NotFound("Employee not found");
                }

                dbContext.Users.Remove(deleteUser);
                dbContext.SaveChanges();
                return BadRequest("Delete successful");
            }
            catch (Exception e)
            {
                return BadRequest("Delete is not successful");
            }

        }

 
    }
}
