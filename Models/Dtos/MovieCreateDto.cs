using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static APIMovies.Models.Movie;

namespace APIMovies.Models.Dtos
{
    public class MovieCreateDto
    {

        [Required(ErrorMessage = "The name is mandatory")]
        public string Name { get; set; }
        
        public string ImagePath { get; set; }

        public IFormFile Photo { get; set; }
        
        [Required(ErrorMessage = "The Description is mandatory")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The Duration is mandatory")]
        public string Duration { get; set; }

        public ClassificationType Classification { get; set; }

        public int categoryID { get; set; }

    }
}
