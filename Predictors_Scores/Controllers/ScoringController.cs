using Microsoft.AspNetCore.Mvc;
using Application.Services;

namespace Predictors_Scores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoringController : ControllerBase
    {
        private readonly PredictionScoringService _scoringService;
        public ScoringController(PredictionScoringService scoringService) { _scoringService = scoringService; }

        [HttpGet("calculate-points")]
        public IActionResult CalculatePoints(int predictedA, int predictedB, int actualA, int actualB)
        {
            var points = _scoringService.CalculatePoints(predictedA, predictedB, actualA, actualB);
            return Ok(points);
        }

        [HttpGet("calculate-winner-points")]
        public IActionResult CalculateWinnerPoints(int totalPredictors, int correctWinnerCount)
        {
            var points = _scoringService.CalculateWinnerPoints(totalPredictors, correctWinnerCount);
            return Ok(points);
        }

        [HttpGet("is-correct-winner")]
        public IActionResult IsCorrectWinner(int predictedA, int predictedB, int actualA, int actualB)
        {
            var isWinner = _scoringService.IsCorrectWinner(predictedA, predictedB, actualA, actualB);
            return Ok(isWinner);
        }
    }
} 