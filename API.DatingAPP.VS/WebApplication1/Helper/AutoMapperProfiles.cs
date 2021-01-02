using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ClassMappers;
using WebApplication1.Models;

namespace WebApplication1.Helper
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDetailedMapper>().
                ForMember(dest => dest.PhotoUrl, opt =>
                   {
                       opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.isMain).Url);
                   }).ForMember(dest => dest.Age, opt => {
                       opt.ResolveUsing(src => src.DateofBirth.CalculateAge());
                   });
            CreateMap<User, UserForListMapper>().
                    ForMember(dest => dest.PhotoUrl, opt => {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.isMain).Url);
                    }).ForMember(dest => dest.Age, opt => {
                        opt.ResolveUsing(src => src.DateofBirth.CalculateAge());
                    }); ;
            CreateMap<Photo, PhotosMapper>();

        }
    }
}
