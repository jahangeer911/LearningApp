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
            var users = _context.users.Include(p => p.Photos)
                .OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }

            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateofBirth >= minDob && u.DateofBirth <= maxDob);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
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


        public async Task<IEnumerable<int>> GetUserLikes(int userid,bool likers)
        {
            var user = await _context.users.Include(x => x.Likees).Include(x => x.Likers).FirstOrDefaultAsync(u=>u.Id==userid);
            if (likers)
            {
                return   user.Likers.Where(u => u.LikeeId == userid).Select(i => i.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikerId == userid).Select(i => i.LikeeId);
            }
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
        public async Task<Like> IsUserLiked(int userid, int recepientuserid)
        {
            return await _context.likes.Where(u => u.LikeeId == userid && u.LikerId == recepientuserid).FirstOrDefaultAsync();
        }
        public async Task<Message> GetMessages(int id)
        {
            return await _context.messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageparams)
        {
            return null;
        }
        public async Task<IEnumerable<Message>> GetMessagesThread(int UserId, int recipientId)
        {
            return null;
        }
    }
}
