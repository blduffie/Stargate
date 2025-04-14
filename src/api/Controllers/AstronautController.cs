using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautController : ControllerBase
    {
        private readonly StargateContext _context;

        public AstronautController(StargateContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AstronautDto>>> GetAllAstronauts()
        {
            try
            {
                var astronauts = await _context.Set<Astronaut>()
                    .Include(a => a.Duties)
                    .ToListAsync();
                var dtoList = astronauts.Select(a => new AstronautDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Rank = a.Rank,
                    CurrentDutyTitle = a.CurrentDutyTitle,
                    CareerStartDate = a.CareerStartDate,
                    CareerEndDate = a.CareerEndDate,
                    Duties = a.Duties?.Select(d => new AstronautDutyDto
                    {
                        Id = d.Id,
                        DutyTitle = d.DutyTitle,
                        Rank = d.Rank,
                        DutyStartDate = d.DutyStartDate,
                        DutyEndDate = d.DutyEndDate
                    }).ToList() ?? new List<AstronautDutyDto>()
                }).ToList();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<AstronautDto>> GetAstronautByName(string name)
        {
            try
            {
                var astronaut = await _context.Set<Astronaut>()
                    .Include(a => a.Duties)
                    .SingleOrDefaultAsync(a => a.Name == name);
                if (astronaut == null) return NotFound($"No astronaut record found for '{name}'.");
                var dto = new AstronautDto
                {
                    Id = astronaut.Id,
                    Name = astronaut.Name,
                    Rank = astronaut.Rank,
                    CurrentDutyTitle = astronaut.CurrentDutyTitle,
                    CareerStartDate = astronaut.CareerStartDate,
                    CareerEndDate = astronaut.CareerEndDate,
                    Duties = astronaut.Duties?.Select(d => new AstronautDutyDto
                    {
                        Id = d.Id,
                        DutyTitle = d.DutyTitle,
                        Rank = d.Rank,
                        DutyStartDate = d.DutyStartDate,
                        DutyEndDate = d.DutyEndDate
                    }).ToList() ?? new List<AstronautDutyDto>()
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AstronautDto>> CreateAstronaut([FromBody] AstronautRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest("Person name is required.");
                var person = await _context.People.SingleOrDefaultAsync(p => p.Name == request.Name);
                if (person == null)
                {
                    person = new Person { Name = request.Name };
                    _context.People.Add(person);
                    await _context.SaveChangesAsync();
                }
                var existingAstronaut = await _context.Set<Astronaut>().SingleOrDefaultAsync(a => a.Id == person.Id);
                if (existingAstronaut != null) return Conflict($"'{request.Name}' is already an astronaut with Id={existingAstronaut.Id}.");
                var astronaut = new Astronaut
                {
                    Id = person.Id,
                    Name = person.Name,
                    Rank = request.Rank ?? string.Empty,
                    CurrentDutyTitle = request.CurrentDutyTitle ?? string.Empty,
                    CareerStartDate = request.CareerStartDate,
                    CareerEndDate = request.CareerEndDate
                };
                _context.Set<Astronaut>().Add(astronaut);
                await _context.SaveChangesAsync();
                var dto = new AstronautDto
                {
                    Id = astronaut.Id,
                    Name = astronaut.Name,
                    Rank = astronaut.Rank,
                    CurrentDutyTitle = astronaut.CurrentDutyTitle,
                    CareerStartDate = astronaut.CareerStartDate,
                    CareerEndDate = astronaut.CareerEndDate,
                    Duties = new List<AstronautDutyDto>()
                };
                return CreatedAtAction(nameof(GetAstronautByName), new { name = dto.Name }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("{name}/duties")]
        public async Task<ActionResult<AstronautDutyDto>> AddAstronautDuty(string name, [FromBody] AstronautDutyRequest dutyRequest)
        {
            try
            {
                var astronaut = await _context.Set<Astronaut>()
                    .Include(a => a.Duties)
                    .SingleOrDefaultAsync(a => a.Name == name);
                if (astronaut == null) return NotFound($"No astronaut record found for '{name}'.");
                if (astronaut.Duties == null || !astronaut.Duties.Any()) astronaut.CareerStartDate = dutyRequest.DutyStartDate;
                var currentDuty = astronaut.Duties?.FirstOrDefault(d => d.DutyEndDate == null);
                if (currentDuty != null) currentDuty.DutyEndDate = dutyRequest.DutyStartDate.AddDays(-1);
                var newDuty = new AstronautDuty
                {
                    AstronautId = astronaut.Id,
                    DutyTitle = dutyRequest.DutyTitle,
                    Rank = dutyRequest.Rank,
                    DutyStartDate = dutyRequest.DutyStartDate,
                    DutyEndDate = null
                };
                _context.Set<AstronautDuty>().Add(newDuty);
                astronaut.Rank = dutyRequest.Rank;
                astronaut.CurrentDutyTitle = dutyRequest.DutyTitle;
                if (dutyRequest.DutyTitle.ToUpper() == "RETIRED") astronaut.CareerEndDate = dutyRequest.DutyStartDate.AddDays(-1);
                await _context.SaveChangesAsync();
                var dutyDto = new AstronautDutyDto
                {
                    Id = newDuty.Id,
                    DutyTitle = newDuty.DutyTitle,
                    Rank = newDuty.Rank,
                    DutyStartDate = newDuty.DutyStartDate,
                    DutyEndDate = newDuty.DutyEndDate
                };
                return CreatedAtAction(nameof(GetAstronautByName), new { name = astronaut.Name }, dutyDto);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{name}/duties")]
        public async Task<ActionResult<IEnumerable<AstronautDutyDto>>> GetAstronautDuties(string name)
        {
            try
            {
                var astronaut = await _context.Set<Astronaut>()
                    .Include(a => a.Duties)
                    .SingleOrDefaultAsync(a => a.Name == name);
                if (astronaut == null) return NotFound($"No astronaut record found for '{name}'.");
                var dutiesDesc = astronaut.Duties?
                    .OrderByDescending(d => d.DutyStartDate)
                    .Select(d => new AstronautDutyDto
                    {
                        Id = d.Id,
                        DutyTitle = d.DutyTitle,
                        Rank = d.Rank,
                        DutyStartDate = d.DutyStartDate,
                        DutyEndDate = d.DutyEndDate
                    })
                    .ToList() ?? new List<AstronautDutyDto>();
                return Ok(dutiesDesc);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
