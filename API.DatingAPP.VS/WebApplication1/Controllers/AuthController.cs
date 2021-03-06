﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.ClassMappers;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _conf;
        private readonly IMapper _Mapper;

        public AuthController(IAuthRepository repo, IConfiguration conf,IMapper _mapper)
        {
            _repo = repo;
            _conf = conf;
            _Mapper = _mapper;
        }
        // GET: api/Auth
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" ,"value3"};
        }

        // GET: api/Auth/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Auth
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserRegisterClassMapper usertorregister )
        {
            //validate request
            usertorregister.username = usertorregister.username.ToLower();
            if (await _repo.UserExists(usertorregister.username))
               return BadRequest("User Name Already Exists");
            var createNewUser = _Mapper.Map<User>(usertorregister);

            var user = await _repo.RegisterUser(createNewUser, usertorregister.password);

            var usertoreturn = _Mapper.Map<UserForDetailedMapper>(createNewUser);
            return CreatedAtRoute("GetUser", new { controller = "Users", id = createNewUser.Id }, usertoreturn);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserLoginClassMapper userlogin)
        {
            var loginuser = await _repo.Login(userlogin.username, userlogin.password);
            if (loginuser==null)
                return Unauthorized();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["Jwt:SigningKey"]));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "Reader"));// Roles based need to work on Later
            claims.Add(new Claim("UserID", loginuser.Id.ToString()));
            claims.Add(new Claim("unique_name", loginuser.UserName.ToString()));

            var jwttoken = new JwtSecurityToken(
                issuer: _conf["Jwt:Issuer"],
                audience: _conf["Jwt:Audience"],
                expires: DateTime.Now.AddHours(Convert.ToInt32(_conf["Jwt:ExpiryInHours"].ToString())),
                signingCredentials: signingCredentials
                , claims: claims
            );
            string generatedtoken = new JwtSecurityTokenHandler().WriteToken(jwttoken);
            var user =_Mapper.Map<UserPhotoURl>(loginuser);
            return Ok(new { token = generatedtoken ,user= user });
        }
        // PUT: api/Auth/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
