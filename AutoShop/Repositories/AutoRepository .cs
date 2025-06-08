using AutoShop.Data;
using AutoShop.Interfaces;
using AutoShop.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoShop.Repositories
{
    public class AutoRepository : IAutoRepository
    {
        private readonly ApplicationDbContext _context;
        public AutoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Auto>> GetAllAutos(CancellationToken cancellationToken)
        {
            return await _context.Autos.ToListAsync(cancellationToken);
        }

        public Auto FindById(int id)
        {
            return _context.Autos.FirstOrDefault(a => a.Id == id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void AddAuto(Auto auto)
        {
            _context.Autos.Add(auto);
            _context.SaveChanges();
        }

        public void DeleteAuto(Auto auto)
        {
            _context.Autos.Remove(auto);
            _context.SaveChanges();
        }

        public int MaxId()
        {
            return _context.Autos.Max(a => a.Id);
        }
    }
}
