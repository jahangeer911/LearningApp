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
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        //// GET: api/Messages
        //[HttpGet]
        //public async Task<Message> Getmessages()
        //{
        //    return await repo.GetMessages(1);
        //}

        // GET: api/Messages/5
        [HttpGet("{id}",Name ="GetMessage")]
        public async Task<IActionResult> GetMessage([FromRoute] int userId,int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Claim cliamid = User.Claims.FirstOrDefault(x => x.Type.Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
            if (cliamid != null)
            {
                int loginuserid = Convert.ToInt32(cliamid.Value);
                if (loginuserid != userId)
                {
                    return Unauthorized();
                }
                var message = await _repo.GetMessages(id);
                if (message == null)
                {
                    return NotFound();
                }

                return Ok(message);
            }
            return BadRequest();
        }

        //// PUT: api/Messages/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMessage([FromRoute] int id, [FromBody] Message message)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != message.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(message).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MessageExists(id))
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

        // POST: api/Messages
        [HttpPost]
        public async Task<IActionResult> PostMessage(int userId, MessageMapper messagemapper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Claim cliamid = User.Claims.FirstOrDefault(x => x.Type.Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
            if (cliamid != null)
            {
                int loginuserid = Convert.ToInt32(cliamid.Value);
                if (loginuserid != userId)
                {
                    return Unauthorized();
                }
                messagemapper.SenderId = userId;
                var recipient = await _repo.GetUser(messagemapper.RecipientId);
                if (recipient == null)
                    return BadRequest("Could not find User");

                var message = _mapper.Map<Message>(messagemapper);
                _repo.Add(message);
                var messagemaptertoreturn=_mapper.Map<MessageMapper>(message);
                if (await _repo.SaveAll())
                {
                    return CreatedAtAction("GetMessage", new { id = message.Id }, messagemaptertoreturn);
                }
            }
            
            //_context.messages.Add(message);
            //await _context.SaveChangesAsync();
            return BadRequest("Invalid Message Send");
        }

        //// DELETE: api/Messages/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMessage([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var message = await _context.messages.FindAsync(id);
        //    if (message == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.messages.Remove(message);
        //    await _context.SaveChangesAsync();

        //    return Ok(message);
        //}

        //private bool MessageExists(int id)
        //{
        //    return _context.messages.Any(e => e.Id == id);
        //}
    }
}