using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.ClassMappers
{
    public class UserForDetailedMapper
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string Lookingfor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotosMapper> Photos { get; set; }
    }
}
