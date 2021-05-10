using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.ClassMappers;
using WebApplication1.Data;
using WebApplication1.Helper;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper mapper;

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            this.mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> Getusers([FromQuery]UserParams userParams)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type.Equals("UserID", StringComparison.InvariantCultureIgnoreCase)).Value);
            userParams.UserId = UserId;
            var userfromrepo =await _repo.GetUser(userParams.UserId);
            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userfromrepo.Gender == "male" ? "female" : "male";// whatever is the gender then change it to other gender
            }
            var users = await _repo.GetUsers(userParams);
            var usertoReturn = mapper.Map<IEnumerable<UserForListMapper>>(users);
            Response.AddPageinationHeaders(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
    
            return Ok(usertoReturn);
        }

        // GET: api/Users/5
        [HttpGet("{id}",Name ="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _repo.GetUser(id);
            var usertoreturn = mapper.Map<UserForDetailedMapper>(user);
            return Ok(usertoreturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateMapper userforupdatemapper)
        {
            Claim cliamid = User.Claims.FirstOrDefault(x => x.Type.Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
            if (cliamid != null)
            {
                int UserId = Convert.ToInt32(cliamid.Value);
                if (UserId != id)
                {
                    return Unauthorized();
                }
                var userfromRepo = await _repo.GetUser(id);
                mapper.Map(userforupdatemapper, userfromRepo);
                if (await _repo.SaveAll())
                {
                    return NoContent();
                }
            }
            else return Unauthorized();


            throw new Exception("Updating user failed on Save");
        }
        [HttpPost("{id}/like/{recipientid}")]
        public async Task<IActionResult> LikeUser(int id,int recipientid)
        {
            Claim cliamid = User.Claims.FirstOrDefault(x => x.Type.Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
            if (cliamid != null)
            {
                var like = await _repo.IsUserLiked(id, recipientid);
                if (like!=null)
                {
                    return BadRequest("User Already Liked");
                }
                like =new Like{ LikeeId=id,LikerId=recipientid};

                _repo.Add<Like>(like);
                if (await _repo.SaveAll())
                {
                    return Ok();
                }
                return BadRequest("Failed to Add Like for User");
            }
            else return Unauthorized();
        }
        // PUT: api/Users/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Users
        //[HttpPost]
        //public async Task<IActionResult> PostUser([FromBody] User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = await _context.users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return Ok(user);
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.users.Any(e => e.Id == id);
        //}
    }
}