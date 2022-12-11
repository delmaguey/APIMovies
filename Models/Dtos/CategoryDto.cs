using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Models.Dtos
{
    public class CategoryDto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "The name is required")]
        public String Name { get; set; }

        public DateTime CreationDate { get; set; }

    }
    
}
