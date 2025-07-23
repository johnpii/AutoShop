using AutoShop.Entities;

namespace AutoShop.Interfaces
{
    public interface IAutoRepository
    {
        Task<List<Auto>> GetAllAutos(CancellationToken cancellationToken);
        Auto FindById(int id);
        void Save();
        void AddAuto(Auto auto);
        void DeleteAuto(Auto auto);
        int MaxId();
    }
}
