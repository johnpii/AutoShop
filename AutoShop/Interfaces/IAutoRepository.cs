using AutoShop.Models;

namespace AutoShop.Interfaces
{
    public interface IAutoRepository
    {
        Task<List<Auto>> GetAllAutos();
        Auto FindById(int id);
        void Save();
        void AddAuto(Auto auto);
        void DeleteAuto(Auto auto);
        int MaxId();
    }
}
