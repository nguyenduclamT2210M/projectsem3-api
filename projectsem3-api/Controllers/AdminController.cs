using Microsoft.AspNetCore.Mvc;
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
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly DataContext _dbContext;
        private IConfiguration configuration;

        public AdminController(DataContext context, IConfiguration config)
        {
            _dbContext = context;
            configuration = config;
        }



        [HttpPost]
        [Route("register")]
        public IActionResult CreateAccount(AdminModel newAccModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_dbContext.Admins.Any(adm => adm.Username == newAccModel.Username))
                    {
                        return Unauthorized("Username Already Exist.");
                    }

                    Role role = _dbContext.Roles.Find(newAccModel.RoleId);
                    if (role == null)
                    {
                        return NotFound("Role not found");
                    }

                    string salt = BCrypt.Net.BCrypt.GenerateSalt(10);

                    string hashedPwd = BCrypt.Net.BCrypt.HashPassword(newAccModel.Password, salt);

                    Admin newAcc = new Admin()
                    {
                        Username = newAccModel.Username,
                        FullName = newAccModel.Fullname,
                        Email = newAccModel.Email,
                        Telephone = newAccModel.Telephone,
                        Password = hashedPwd,
                        RoleId = role.Id
                    };

                    _dbContext.Add(newAcc);
                    _dbContext.SaveChanges();

                    return Created("Account created.", new AdminDTO()
                    {
                        Id = newAcc.Id,
                        Username = newAcc.Username,
                        FullName = newAcc.FullName,
                        Permissions = null,
                        Token = null
                    });
                }
                catch (Exception e)
                {
                    return Unauthorized("Create Account Error");
                }
            }

            return Unauthorized("Create account error");
        }

        [HttpGet]
        [Route("index")]
        public IActionResult GetAccountDetail(int id)
        {
            try
            {
                Admin accDetail = _dbContext.Admins.Find(id);
                if (accDetail == null)
                {
                    return NotFound("Account not found");
                }

                var detailmodel = new AccountDTO()
                {
                    Username = accDetail.Username,
                    Email = accDetail.Email,
                    Fullname = accDetail.FullName,
                    Role = _dbContext.Roles
                    .Where(a => a.Id == accDetail.RoleId)
                    .Select(a => new RoleDTO()
                    {
                        Id = a.Id,
                        Name = a.Name
                    }).FirstOrDefault()
                   
                };

                return Ok(detailmodel);

            }
            catch (Exception e)
            {
                return NotFound("Account not found.");
            }
        }
        //admin login

        [HttpPost]
        [Route("admin/login")]
        public IActionResult AdmLogin(AdminLoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized("Email or password is incorrect");
            }
            try
            {
                var admin = _dbContext.Admins.Include(a => a.Role).SingleOrDefault(a => a.Username == loginModel.Username);

                if (admin == null)
                {
                    throw new Exception("Email or password is incorrect");
                }
                bool verified = BCrypt.Net.BCrypt.Verify(loginModel.Password, admin.Password);
                if (!verified)
                {
                    throw new Exception("Email or password is incorrect");
                }
                List<PermissionDTO> permissions = _dbContext.Permissions
                          .Where(p => p.RoleId == admin.RoleId)
                          .Select(p => new PermissionDTO()
                          {
                              Name = p.Name,
                              Prefix = p.Prefix,
                              FaIcon = p.FaIcon
                          })
                          .ToList();
                return Ok(new AdminDTO
                {
                    Username = admin.Username,
                    FullName = admin.FullName,
                    Token = GenJWTAdmin(admin),
                    Permissions = permissions
                });
            }
            catch (Exception e)
            {
                return Unauthorized("Email or password is incorrect");
            }
        }
       

        private string GenJWTAdmin(Admin admin)
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
                new Claim(ClaimTypes.NameIdentifier,admin.Id.ToString()),
                new Claim(ClaimTypes.Email,admin.Email),
                new Claim("Role", admin.Role.Name),
                new Claim(ClaimTypes.Name,admin.FullName),

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

    }
}

