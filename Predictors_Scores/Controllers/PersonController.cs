using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain;
using System.Threading.Tasks;
using System.Linq;

namespace Predictors_Scores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _service;
        public PersonController(IPersonService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var persons = await _service.GetAllAsync();
            var dtos = persons.Select(p => new PersonDto
            {
                Id = p.Id,
                Name = p.Name,
                Points = p.Points
            }).ToList();
            return Ok(dtos);
        }

        /// <summary>
        /// Adds a new person. You can set the initial score by providing the 'points' property in the request body.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Person person)
        {
            await _service.AddAsync(person);
            var dto = new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                Points = person.Points
            };
            return Ok(dto);
        }
    }

    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Points { get; set; }
    }
} 