using APIMovies.Data;
using APIMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateCategory(Category category)
        {
            _db.Category.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _db.Category.Remove(category);
            return Save();
        }

        public bool ExistCategory(string name)
        {
            bool value = _db.Category.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool ExistCategory(int categoryId)
        {
            return _db.Category.Any(c => c.Id == categoryId);
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Category.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _db.Category.FirstOrDefault(c => c.Id == categoryId);
        }

        public bool UpdateCategory(Category category)
        {
            _db.Category.Update(category);
            return Save();
            
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
