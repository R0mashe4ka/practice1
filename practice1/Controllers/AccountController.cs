using System;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using practice1.Models;
using practice1.Services;

namespace practice1.Controllers
{
    //[Authorize]
    [ApiController]

    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        private IUserService _userService;

        public AccountController(IUserService personService)
        {
            _userService = personService;
        }

        //[AllowAnonymous]
        //[HttpPost("authenticate")]
        //public IActionResult Authenticate([FromBody]PersonDTO personDto)
        //{
        //    var person = _userService.Authenticate(personDto.Login, personDto.Password);

        //    if (person == null)
        //        return BadRequest(new { message = "Username or password is incorrect" });

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes("test");
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name, person.UserId.ToString())
        //        }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenString = tokenHandler.WriteToken(token);

        //    // return basic user info (without password) and token to store client side
        //    return Ok(new
        //    {
        //        Id = person.UserId,
        //        Login = person.Name,
        //        Token = tokenString
        //    });
        //}

        //[AllowAnonymous]
        //[HttpPost("register")]
        //public IActionResult Register([FromBody]PersonDTO personDto)
        //{
        //    // map dto to entity

        //    try
        //    {
        //        // save 
        //        _userService.Create(, personDto.Password);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        // return error message if there was an exception
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        [HttpGet]
        public IActionResult GetAll()
        {
            var people = _userService.GetAll();
            return Ok(people);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var person = _userService.GetById(id);
            return Ok(person);
        }

        [HttpPut("{id}")]
        public IActionResult Update(User newuser)
        {
            _userService.Update(newuser);
            return Ok();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }

        [HttpPost]
        public void Create(UserDTO user)
        {
            User usr = new User
            {
                Address = user.Address,
                Name = user.Name,
                Password = user.Password,

               
                
                PhoneNumber = user.PhoneNumber,
                CompanyId = user.CompanyId,


            };
             _userService.Create(usr);
            

        }


    }
}
