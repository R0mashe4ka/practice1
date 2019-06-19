using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using practice1.Controllers;
using practice1.Helpers;
using practice1.Models;
using practice1.Services;

namespace practice1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        private IPersonService _personService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AccountController(
            IPersonService personService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _personService = personService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]PersonDTO personDto)
        {
            var person = _personService.Authenticate(personDto.Login, personDto.Password);

            if (person == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, person.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = person.Id,
                Login = person.Login,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]PersonDTO personDto)
        {
            // map dto to entity
            var person = _mapper.Map<Person>(personDto);

            try
            {
                // save 
                _personService.Create(person, personDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var people = _personService.GetAll();
            var personDTOs = _mapper.Map<IList<PersonDTO>>(people);
            return Ok(personDTOs);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var person = _personService.GetById(id);
            var personDTO = _mapper.Map<PersonDTO>(person);
            return Ok(personDTO);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]PersonDTO personDTO)
        {
            // map dto to entity and set id
            var person = _mapper.Map<Person>(personDTO);
            person.Id = id;

            try
            {
                // save 
                _personService.Update(person, personDTO.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _personService.Delete(id);
            return Ok();
        }

    }
}
