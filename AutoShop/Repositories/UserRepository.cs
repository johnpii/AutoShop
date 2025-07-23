using AutoShop.Data;
using AutoShop.Entities;
using AutoShop.Interfaces;
using AutoShop.Utilities;

namespace AutoShop.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool ExistByEmail(string email)
        {
            return _context.Users.Any(p => p.Email == email);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User FindByEmailAndPassWord(string email, string password)
        {
            return _context.Users.FirstOrDefault(p => p.Email == email && p.Password == PasswordEncryption.EncryptPassword(password));
        }
    }
}
