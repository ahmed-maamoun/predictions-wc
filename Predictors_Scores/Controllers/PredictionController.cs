using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain;
using System.Threading.Tasks;
using System.Linq;

namespace Predictors_Scores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController : ControllerBase
    {
        private readonly IPredictionService _service;
        public PredictionController(IPredictionService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var predictions = await _service.GetAllAsync();
            var dtos = predictions.Select(p => new PredictionDto
            {
                Id = p.Id,
                PersonId = p.PersonId,
                MatchId = p.MatchId,
                PredictedScoreA = p.PredictedScoreA,
                PredictedScoreB = p.PredictedScoreB,
                PredictionPoints=p.PredictionPoints
                
            }).ToList();
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Prediction prediction)
        {
            await _service.AddAsync(prediction);
            var dto = new PredictionDto
            {
                Id = prediction.Id,
                PersonId = prediction.PersonId,
                MatchId = prediction.MatchId,
                PredictedScoreA = prediction.PredictedScoreA,
                PredictedScoreB = prediction.PredictedScoreB
            };
            return Ok(dto);
        }
    }
} 