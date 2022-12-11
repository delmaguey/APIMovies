using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        public String Name { get; set; }

        public DateTime CreationDate { get; set; }

    }
    
}
