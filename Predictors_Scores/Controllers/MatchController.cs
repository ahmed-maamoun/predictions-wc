using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Predictors_Scores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _service;
        private readonly IPredictionService _predictionService;
        public MatchController(IMatchService service, IPredictionService predictionService) { _service = service; _predictionService = predictionService; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var matches = await _service.GetAllAsync();
            var allPredictions = await _predictionService.GetAllAsync();
            var dtos = matches.Select(m => new MatchDto
            {
                Id = m.Id,
                TeamA = m.TeamA,
                TeamB = m.TeamB,
                MatchDate = m.MatchDate,
                ScoreA = m.ScoreA,
                ScoreB = m.ScoreB,
                Predictions = allPredictions
                    .Where(p => p.MatchId == m.Id)
                    .Select(p => new PredictionDto
                    {
                        Id = p.Id,
                        PersonId = p.PersonId,
                        MatchId = p.MatchId,
                        PredictedScoreA = p.PredictedScoreA,
                        PredictedScoreB = p.PredictedScoreB,
                        PredictionPoints = p.PredictionPoints
                    }).ToList()
            });
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Match match)
        {
            await _service.AddAsync(match);
            var dto = new MatchDto
            {
                Id = match.Id,
                TeamA = match.TeamA,
                TeamB = match.TeamB,
                MatchDate = match.MatchDate,
                ScoreA = match.ScoreA,
                ScoreB = match.ScoreB
            };
            return Ok(dto);
        }

        [HttpPost("{id}/result")]
        public async Task<IActionResult> EnterResult(int id, [FromBody] MatchResultDto dto)
        {
            await _service.EnterResultAndCalculatePoints(id, dto.ScoreA, dto.ScoreB);
            return Ok();
        }
    }

    public class MatchResultDto
    {
        public int ScoreA { get; set; }
        public int ScoreB { get; set; }
    }

    public class MatchDto
    {
        public int Id { get; set; }
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public System.DateTime MatchDate { get; set; }
        public int? ScoreA { get; set; }
        public int? ScoreB { get; set; }
        public List<PredictionDto> Predictions { get; set; }
    }

    public class PredictionDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int MatchId { get; set; }
        public int PredictedScoreA { get; set; }
        public int PredictedScoreB { get; set; }
        public decimal? PredictionPoints { get; set; }
    }
} 