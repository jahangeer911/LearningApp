using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.users.Include(p => p.Photos).ToListAsync();
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
