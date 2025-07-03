using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Application.Interfaces;
using System.Linq;
using Application.Services;

namespace Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _repo;
        private readonly IPredictionRepository _predictionRepo;
        private readonly IPersonRepository _personRepo;
        private readonly PredictionScoringService _scoringService;
        public MatchService(IMatchRepository repo, IPredictionRepository predictionRepo, IPersonRepository personRepo, PredictionScoringService scoringService)
        {
            _repo = repo;
            _predictionRepo = predictionRepo;
            _personRepo = personRepo;
            _scoringService = scoringService;
        }
        public Task<List<Match>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Match> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public async Task AddAsync(Match match)
        {
            if (match.MatchDate.Kind == System.DateTimeKind.Unspecified)
                match.MatchDate = System.DateTime.SpecifyKind(match.MatchDate, System.DateTimeKind.Utc);
            else
                match.MatchDate = match.MatchDate.ToUniversalTime();
            await _repo.AddAsync(match);
        }
        public Task UpdateAsync(Match match) => _repo.UpdateAsync(match);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public async Task EnterResultAndCalculatePoints(int matchId, int scoreA, int scoreB)
        {
            var match = await _repo.GetByIdAsync(matchId);
            if (match == null) return;
            match.ScoreA = scoreA;
            match.ScoreB = scoreB;
            await _repo.UpdateAsync(match);
            var predictions = await _predictionRepo.GetByMatchIdAsync(matchId);
            var totalPredictors = predictions.Count();
            var correctWinnerCount = predictions.Count(p => _scoringService.IsCorrectWinner(p.PredictedScoreA, p.PredictedScoreB, scoreA, scoreB));
            foreach (var prediction in predictions)
            {
                var person = prediction.Person;
                if (person == null)
                {
                    person = await _personRepo.GetByIdAsync(prediction.PersonId);
                }
                decimal points = _scoringService.CalculatePoints(prediction.PredictedScoreA, prediction.PredictedScoreB, scoreA, scoreB);
                if (_scoringService.IsCorrectWinner(prediction.PredictedScoreA, prediction.PredictedScoreB, scoreA, scoreB))
                {
                    points += _scoringService.CalculateWinnerPoints(totalPredictors, correctWinnerCount);
                }
                person.Points += points;
                prediction.PredictionPoints= points;
                await _personRepo.UpdateAsync(person);
            }
        }
    }
} 