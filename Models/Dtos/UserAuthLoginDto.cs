using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Models.Dtos
{
    public class UserAuthLoginDto
    {
        [Required(ErrorMessage = "User is mandatory")]
        public string User { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; }
    }
}
