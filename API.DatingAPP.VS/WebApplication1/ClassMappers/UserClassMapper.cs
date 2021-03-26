using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ClassMappers
{
    public class UserRegisterClassMapper
    {
        [Required]
        public string username { get; set; }
        [StringLength(8,MinimumLength =8,ErrorMessage ="Minimum Lenght must be 8 charaacters")]
        public string password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateofBirth { get; set; }
        [Required]
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        public UserRegisterClassMapper()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
    public class UserLoginClassMapper
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
