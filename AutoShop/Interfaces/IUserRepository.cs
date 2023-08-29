using AutoShop.Models;

namespace AutoShop.Interfaces
{
    public interface IUserRepository
    {
        bool ExistByEmail(string email);
        void AddUser(User user);
        User FindByEmailAndPassWord(string email, string password);
    }
}
