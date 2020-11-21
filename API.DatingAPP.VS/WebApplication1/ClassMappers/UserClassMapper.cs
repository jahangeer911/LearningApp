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
    }
    public class UserLoginClassMapper
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
