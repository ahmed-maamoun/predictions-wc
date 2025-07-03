using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Domain;

namespace Infrastructure.Repositories
{
    public class PredictionRepository : IPredictionRepository
    {
        private readonly ApplicationDbContext _context;
        public PredictionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Prediction>> GetAllAsync() => await _context.Predictions.ToListAsync();
        public async Task<Prediction> GetByIdAsync(int id) => await _context.Predictions.FindAsync(id);
        public async Task AddAsync(Prediction prediction)
        {
            await _context.Predictions.AddAsync(prediction);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Prediction prediction)
        {
            _context.Predictions.Update(prediction);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var prediction = await _context.Predictions.FindAsync(id);
            if (prediction != null)
            {
                _context.Predictions.Remove(prediction);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Prediction>> GetByMatchIdAsync(int matchId) => await _context.Predictions.Where(p => p.MatchId == matchId).ToListAsync();
        public async Task<List<Prediction>> GetByPersonIdAsync(int personId) => await _context.Predictions.Where(p => p.PersonId == personId).ToListAsync();
    }
} 