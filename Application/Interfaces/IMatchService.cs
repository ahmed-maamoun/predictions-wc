using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IMatchService
    {
        Task<List<Match>> GetAllAsync();
        Task<Match> GetByIdAsync(int id);
        Task AddAsync(Match match);
        Task UpdateAsync(Match match);
        Task DeleteAsync(int id);
        Task EnterResultAndCalculatePoints(int matchId, int scoreA, int scoreB);
    }
} 