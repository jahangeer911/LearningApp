using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using AutoMapper;
using WebApplication1.Helper;
using CloudinaryDotNet;
using WebApplication1.ClassMappers;
using System.Security.Claims;
using CloudinaryDotNet.Actions;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudanaryconfig;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo,IMapper mapper,IOptions<CloudinarySettings> cloudanaryconfig)
        {
            this._repo = repo;
            this._mapper = mapper;
            this._cloudanaryconfig = cloudanaryconfig;
            Account account = new Account(_cloudanaryconfig.Value.CloudName,_cloudanaryconfig.Value.ApiKey,_cloudanaryconfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        
        // GET: api/Photos/5
        [HttpGet("{id}",Name ="GetPhoto")]
        public async Task<IActionResult> GetPhoto( int id)
        {

            var photofromrepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotosMapper>(photofromrepo);
            return Ok(photo);
            
        }

        // PUT: api/Photos/5
        [HttpPost("{photoID}/setMain")]
        public async Task<IActionResult> setMainPhoto(int UserID, int photoID)
        {
            Claim cliamid = User.Claims.FirstOrDefault(x => x.Type.Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
            if (cliamid != null)
            {
                int loginuserid = Convert.ToInt32(cliamid.Value);
                if (loginuserid != UserID)
                {
                    return Unauthorized();
                }
                var userfromRepo = await _repo.GetUser(UserID);
                if (!userfromRepo.Photos.Any(p => p.Id == photoID))
                {
                    return Unauthorized();
                }

                var photofromRepo = await _repo.GetPhoto(photoID);
                if (photofromRepo.isMain)
                {
                    return BadRequest("Already Main Photo");
                }
                var currentmainphoto = await _repo.GetUserMainPhoto(UserID);
                currentmainphoto.isMain = false;
                photofromRepo.isMain = true;
                if (await _repo.SaveAll())
                    return NoContent();
                else
                    return BadRequest("Could not set main photo"); 
            }
            return Unauthorized();
        }
        // POST: api/Photos
        [HttpPost]
        public async Task<IActionResult> PostPhoto(int userId, [FromForm]PhotoForCreation photoForCreation)
        {
            Claim cliamid = User.Claims.FirstOrDefault(x => x.Type.Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
            if (cliamid != null)
            {
                int UserId = Convert.ToInt32(cliamid.Value);
                if (UserId != userId)
                {
                    return Unauthorized();
                }
                var userfromRepo = await _repo.GetUser(userId);
                var file = photoForCreation.File;
                var uploadResult = new  ImageUploadResult();
                if (file.Length > 0)
                {
                    using (var stream =file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(file.Name,stream),
                            Transformation =new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                        };
                        uploadResult = _cloudinary.Upload(uploadParams);
                        
                    }
                    photoForCreation.Url = uploadResult.Url.ToString();
                    photoForCreation.PublicId = uploadResult.PublicId.ToString();
                    var photo = _mapper.Map<Photo>(photoForCreation);

                    if (!(userfromRepo.Photos.Any(u => u.isMain)))
                        photo.isMain = true;
                    userfromRepo.Photos.Add(photo);

                    if (await _repo.SaveAll())
                    {
                        var phototoreturn = _mapper.Map<PhotosMapper>(photo);
                        return CreatedAtRoute("GetPhoto", new { id = photo.Id }, phototoreturn);
                    }

                }
            }
            return BadRequest("could not upload photo");

        }

        // DELETE: api/Photos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, [FromRoute] int id)
        {
            Claim cliamid = User.Claims.FirstOrDefault(x => x.Type.Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
            if (cliamid != null)
            {
                int UserId = Convert.ToInt32(cliamid.Value);
                if (UserId != userId)
                {
                    return Unauthorized();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var photo = await _repo.GetPhoto(id);
                if (photo == null)
                {
                    return NotFound();
                }
                if (photo.isMain)
                {
                    return BadRequest("You cannot delete Main Photo");
                }
                if (photo.PublicId != null)
                {
                    string pulicid = photo.PublicId;
                    var deletionParams = new DeletionParams(pulicid);

                    var deletionResult = _cloudinary.Destroy(deletionParams);
                    if (deletionResult.Result == "ok")
                    {
                        _repo.Delete(photo);
                    }
                }
                else //public id is null
                    _repo.Delete(photo);
                if ( await _repo.SaveAll())
                    return Ok();
            }
            return BadRequest("unable to delete");
        }

        //private bool PhotoExists(int id)
        //{
        //    return _context.photos.Any(e => e.Id == id);
        //}
    }
}