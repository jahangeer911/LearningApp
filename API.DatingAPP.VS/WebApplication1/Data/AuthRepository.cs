using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            if (username==null ||username==""||password==null||password=="")
                return null;

            var user =await _context.users.FirstOrDefaultAsync(x => x.UserName == username);
            if(user!=null)
                if (!VerifyPassword(password, user.PasswordHashed, user.SaltHashed))
                    return null;

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            if (username == null || username == "" )
                return false;
            var user = await _context.users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user!=null)
                return true;
            return false;
        }

        public async Task<User> RegisterUser(User user, string password)
        {
            var usereists = await _context.users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if (usereists == null) { 
                byte[] HashedPassword, SaltedPassword;
                CreateHashPassword(password ,out HashedPassword, out SaltedPassword);
                user.PasswordHashed = HashedPassword;
                user.SaltHashed = SaltedPassword;
                await _context.users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            return null; 
        }

        private void CreateHashPassword(string password, out byte[] hashedPassword, out byte[] saltedPassword)
        {
            using (var hmad5 = new System.Security.Cryptography.HMACSHA512())
            {
                saltedPassword = hmad5.Key;
                hashedPassword = hmad5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }
        private bool VerifyPassword(string password, byte[] passwordHashed, byte[] saltHashed)
        {
            using (var hmad5 = new System.Security.Cryptography.HMACSHA512(saltHashed))
            {
                var hashedPassword = hmad5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < passwordHashed.Length; i++)
                {
                    if (passwordHashed[i] != hashedPassword[i]) return false;
                }
            }
            return true;
        }

    }
}
