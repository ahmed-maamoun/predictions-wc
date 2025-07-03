using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Application.Interfaces;

namespace Application.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly IPredictionRepository _repo;
        private readonly IMatchRepository _matchRepo;
        public PredictionService(IPredictionRepository repo, IMatchRepository matchRepo) { _repo = repo; _matchRepo = matchRepo; }
        public Task<List<Prediction>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Prediction> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public async Task<bool> AddAsync(Prediction prediction)
        {
            var match = await _matchRepo.GetByIdAsync(prediction.MatchId);
            if (match == null || match.MatchDate <= System.DateTime.UtcNow.AddHours(3)) // GMT+3
                return false;
            await _repo.AddAsync(prediction);
            return true;
        }
        public Task UpdateAsync(Prediction prediction) => _repo.UpdateAsync(prediction);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<List<Prediction>> GetByMatchIdAsync(int matchId) => _repo.GetByMatchIdAsync(matchId);
        public Task<List<Prediction>> GetByPersonIdAsync(int personId) => _repo.GetByPersonIdAsync(personId);
    }
} 