using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public class SeedingUserData
    {
        private readonly DataContext _context;

        public SeedingUserData(DataContext context)
        {
            _context = context; 
        }
        public void SeedUserData()
        {
            var userdata = File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<Models.User>>(userdata);
            foreach (var user in users)
            {
                byte[] passwordHash, passwrdSalt;
                CreateHashPassword("password", out passwordHash, out passwrdSalt);
                user.PasswordHashed = passwordHash;
                user.SaltHashed = passwrdSalt;
                user.UserName = user.UserName.ToLower();
                _context.users.Add(user);
            }
            _context.SaveChanges();
        }
        private void CreateHashPassword(string password, out byte[] hashedPassword, out byte[] saltedPassword)
        {
            using (var hmad5 = new System.Security.Cryptography.HMACSHA512())
            {
                saltedPassword = hmad5.Key;
                hashedPassword = hmad5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }
    }
}
