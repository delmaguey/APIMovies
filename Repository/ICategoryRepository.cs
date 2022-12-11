using APIMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Repository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int categoryId);

        bool ExistCategory(string name);

        bool ExistCategory(int categoryId);

        bool CreateCategory(Category category);

        bool UpdateCategory(Category category);

        bool DeleteCategory(Category category);

        bool Save();
    }
}
