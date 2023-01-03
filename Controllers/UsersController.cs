using APIMovies.Models;
using APIMovies.Models.Dtos;
using APIMovies.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace APIMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsersController(IUserRepository userRepo, IMapper mapper, IConfiguration config)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet]
        public IActionResult GetUsers() 
        {
            var listUsers = _userRepo.GetUsers();

            var listUsersDto = new List<UserDto>();

            foreach (var list in listUsers)
            {
                listUsersDto.Add(_mapper.Map<UserDto>(list));
            }

            return Ok(listUsersDto);
        }


        [HttpGet("{userId:int}", Name="GetUser")]
        public IActionResult GetUser(int userId)
        {
            var itemUser = _userRepo.GetUser(userId);

            if (itemUser == null)
            {
                return NotFound();
            }

            var itemUserDto = _mapper.Map<UserDto>(itemUser);
            return Ok(itemUserDto);
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp(UserAuthDto userAuthDto)
        {
            userAuthDto.User = userAuthDto.User.ToLower();

            if (_userRepo.ExistUser(userAuthDto.User))
            {
                return BadRequest("User already exist");
            }

            var userACreate = new User
            {
                UserA = userAuthDto.User
            };

            var userCreated = _userRepo.SignUp(userACreate, userAuthDto.Password);
            
            return Ok(userCreated);
        }

        [HttpPost("Login")]
        public IActionResult Login(UserAuthLoginDto userAuthLoginDto)
        {
            var userFromRepo = _userRepo.Login(userAuthLoginDto.User, userAuthLoginDto.Password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserA.ToString())
            };

            // Token generation
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
