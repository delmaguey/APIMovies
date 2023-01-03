using APIMovies.Data;
using APIMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ExistUser(string name)
        {
            if (_db.User.Any(x => x.UserA == name))
            {
                return true;
            }

            return false;
        }

        public User GetUser(int userId)
        {
            return _db.User.FirstOrDefault(c => c.Id == userId);
        }

        public ICollection<User> GetUsers()
        {
            return _db.User.OrderBy(c => c.UserA).ToList();
        }       

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public User Login(string user, string password)
        {
            var usr = _db.User.FirstOrDefault(x => x.UserA == user);

            if (usr == null) {
                return null;
            }

            if (!VerifyPasswordHash(password, usr.PasswordHash, usr.PasswordSalt))
            {
                return null;
            }

            return usr;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < hashComputed.Length; i++)
                {
                    if (hashComputed[i] != passwordHash[i]) 
                        return false;
                }
            }
            return true;
        }

        public User SignUp(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _db.User.Add(user);
            Save();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
