using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;
        public MatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Match>> GetAllAsync() => await _context.Matches.ToListAsync();
        public async Task<Match> GetByIdAsync(int id) => await _context.Matches.Where(m => m.Id == id).FirstOrDefaultAsync();
        public async Task AddAsync(Match match)
        {
            await _context.Matches.AddAsync(match);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Match match)
        {
            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();
            }
        }
    }
} 