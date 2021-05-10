using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Helper;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IDatingRepository
    {
        void Add<T>(T entity)where T:class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetUserMainPhoto(int userid);
        Task<Like> IsUserLiked(int userid, int recepientuserid);
    }
}
