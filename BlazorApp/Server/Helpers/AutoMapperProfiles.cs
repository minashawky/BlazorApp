using AutoMapper;
using BlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Server.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Person, Person>()
                .ForMember(x => x.Picture, options => options.Ignore());
            CreateMap<Movie, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore());
        }
    }
}
