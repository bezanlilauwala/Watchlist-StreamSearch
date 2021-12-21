using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI3.Identity;
using WebAPI3.Models;
using Microsoft.AspNetCore.Cors;
using System.Net.Http;
using System.Net;

namespace WebAPI3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase    //All API controllers are derived from ControllerBase, MVC controllers are derived from Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;     //Private variables begin with an underscore
        private readonly WatchlistDBContext _context;

        public AuthController(UserManager<ApplicationUser> userManager, WatchlistDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            //Verify that the user logged in successfully
            ApplicationUser user = await _userManager.FindByNameAsync(userDto.UserName);
            if (user is null)
            {
                return NotFound();
            }
            bool result = await _userManager.CheckPasswordAsync(user, userDto.Password);
            if (!result)
            {
                return Ok(new { result = false });
            }

            SymmetricSecurityKey secretKey = new (Encoding.UTF8.GetBytes("Csun586@12:00PMS#cretKey"));
            SigningCredentials signingCreds = new (secretKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken tokenOptions = new(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(1440),
                signingCredentials: signingCreds
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            string username = userDto.UserName;

            IQueryable<User> theUser = _context.Users.Where(u => u.UserName == username);
            List<User> userList = theUser.ToList();
            int id = 0;
            if (userList.Count > 0)
            {
                id = userList[0].Id;
            }

            return Ok(new 
            { 
                Token = tokenString,
                Username = username,
                Id = id
            });
        }

        //This function is used to create a user
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(string UserName, string Password, string Email)
        {
            ApplicationUser user = new()
            {
                UserName = UserName,
                Email = Email,
                EmailConfirmed = true
            };
            IdentityResult result = await _userManager.CreateAsync(user, Password);
               
            //Add the new user to the WatchlistDB User table
            User newUser = new User();
            newUser.UserName = UserName;
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(result);
        }
    }

    public class UserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }

}
