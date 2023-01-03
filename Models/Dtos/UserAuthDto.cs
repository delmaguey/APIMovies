using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Models.Dtos
{
    public class UserAuthDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="User is mandatory")]
        public string User { get; set; }

        [Required(ErrorMessage ="Password is mandatory")]
        [StringLength(10, MinimumLength =4, ErrorMessage ="Password must have 4 to 10 characters")]
        public string Password { get; set; }
    }
}
