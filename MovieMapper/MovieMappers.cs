using APIMovies.Models;
using APIMovies.Models.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.MovieMapper
{
    public class MovieMappers : Profile
    {
        public MovieMappers()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }

    }
}
