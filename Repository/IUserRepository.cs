using APIMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Repository
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int userId);

        bool ExistUser(string name);

        User SignUp(User user, string password);

        User Login(string user, string password);

        bool Save();
    }
}
