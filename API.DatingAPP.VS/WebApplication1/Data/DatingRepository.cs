using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Helper;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext _context)
        {
            this._context = _context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await _context.photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.users.Include(p => p.Photos).Where(x => x.Id != userParams.UserId).Where(y=>y.Gender == userParams.Gender)
                .OrderByDescending(l=>l.LastActive).AsQueryable();
            if (userParams.MinAge!=18 ||userParams.MaxAge!=99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(u => u.DateofBirth >= minDob && u.DateofBirth <= maxDob);
            }
            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch(userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize); 
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Photo> SetMainPhoto(int photoid)
        {
            var photo = await _context.photos.FirstOrDefaultAsync(p => p.Id == photoid);
            photo.isMain = true;
            return null;// _context.SaveChangesAsync();

        }
        public async Task<Photo> GetUserMainPhoto(int userid)
        {
            return await _context.photos.Where(u => u.UserId == userid).FirstOrDefaultAsync(p=>p.isMain);
        }

    }
}
