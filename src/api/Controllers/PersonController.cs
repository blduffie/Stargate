using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;

namespace StargateAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly StargateContext _context;

        public PersonController(StargateContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetAllPeople()
        {
            try
            {
                var people = await _context.People.ToListAsync();
                return Ok(people);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Person>> GetPersonByName(string name)
        {
            try
            {
                var person = await _context.People
                    .SingleOrDefaultAsync(p => p.Name == name);

                if (person == null)
                    return NotFound($"No person found with Name='{name}'");

                return Ok(person);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Person>> AddOrUpdatePerson([FromBody] Person incoming)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(incoming.Name))
                    return BadRequest("Person name cannot be empty.");
                var existing = await _context.People
                    .SingleOrDefaultAsync(p => p.Name == incoming.Name);

                if (existing == null)
                {
                    var newPerson = new Person
                    {
                        Name = incoming.Name
                    };
                    _context.People.Add(newPerson);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetPersonByName),
                        new { name = newPerson.Name }, newPerson);
                }
                else
                {
                    existing.Name = incoming.Name;

                    await _context.SaveChangesAsync();
                    return Ok(existing);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{name}/duties")]
        public async Task<ActionResult<IEnumerable<AstronautDuty>>> GetAstronautDutiesByName(string name)
        {
            try
            {
                var astronaut = await _context.Astronauts
                    .Include(a => a.Duties)
                    .SingleOrDefaultAsync(a => a.Name == name);

                if (astronaut == null)
                    return NotFound($"No Astronaut found with Name='{name}'");

                return Ok(astronaut.Duties ?? new List<AstronautDuty>());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{name}/duties")]
        public async Task<ActionResult<AstronautDuty>> AddAstronautDuty(string name, [FromBody] AstronautDuty incoming)
        {
            try
            {
                var astronaut = await _context.Astronauts
                    .Include(a => a.Duties)
                    .SingleOrDefaultAsync(a => a.Name == name);

                if (astronaut == null)
                {
                    var person = await _context.People
                        .SingleOrDefaultAsync(p => p.Name == name);

                    if (person == null)
                    {
                        return NotFound($"No Person found with Name='{name}'.");
                    }
                    else
                    {
                        astronaut = new Astronaut
                        {
                            Id = person.Id,                // must match existing Person PK
                            Name = person.Name,            // carry over the base fields
                            Rank = incoming.Rank,          // or default
                            CurrentDutyTitle = incoming.DutyTitle,
                            CareerStartDate = DateTime.UtcNow
                        };
                        _context.Astronauts.Add(astronaut);
                    }
                }

                var duty = new AstronautDuty
                {
                    AstronautId = astronaut.Id,
                    DutyTitle = incoming.DutyTitle,
                    Rank = incoming.Rank,
                    DutyStartDate = incoming.DutyStartDate,
                    DutyEndDate = incoming.DutyEndDate
                };

                _context.AstronautDuties.Add(duty);

                astronaut.Rank = incoming.Rank;
                astronaut.CurrentDutyTitle = incoming.DutyTitle;

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAstronautDutiesByName),
                    new { name = astronaut.Name }, duty);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
    // [ApiController]
    // [Route("[controller]")]
    // public class PersonController : ControllerBase
    // {
    //     private readonly StargateContext _context;

    //     public PersonController(StargateContext context)
    //     {
    //         _context = context;
    //     }

    // [HttpGet()]
    // public async Task<ActionResult<IEnumerable<PersonAstronaut>>> GetPeople()
    // {
    //     try
    //     {
    //         var result = await _mediator.Send(new GetPeople());

    //         await LogProcessAsync("Info", "Retrieved all people successfully.");
    //         return Ok(result);
    //     }
    //     catch (Exception ex)
    //     {
    //         await LogProcessAsync("Error", ex.Message);
    //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    //     }
    // }

    // [HttpGet()]
    // public async Task<ActionResult<IEnumerable<Person>>> GetAll()
    // {
    //     if (_context.People == null)
    //     {
    //         return NotFound("No people found.");
    //     }
    //     return await _context.People
    //         .Include(p => p.AstronautDetail)
    //         .Select(p => new PersonAstronaut
    //         {
    //             PersonId = p.Id,
    //             Name = p.Name,
    //             CurrentRank = p.AstronautDetail!.CurrentRank,
    //             CurrentDutyTitle = p.AstronautDetail.CurrentDutyTitle,
    //             CareerStartDate = p.AstronautDetail.CareerStartDate,
    //             CareerEndDate = p.AstronautDetail.CareerEndDate,
    //         })
    //         .ToListAsync();
    // }

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<AstronautDto>>> GetAll()
    // {
    //     try
    //     {
    //         var astronauts = await _context.Astronauts
    //             .Include(a => a.Duties)
    //             .ToListAsync();

    //         return Ok(astronauts);
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(StatusCodes.Status500InternalServerError,
    //                           new { message = ex.Message });
    //     }
    // }



    // [HttpGet("{name}")]
    // public async Task<IActionResult> GetPersonByName(string name)
    // {
    //     try
    //     {
    //         var result = await _mediator.Send(new GetPersonByName { Name = name });

    //         await LogProcessAsync("Info", $"Retrieved person '{name}'.");
    //         return this.GetResponse(result);
    //     }
    //     catch (Exception ex)
    //     {
    //         await LogProcessAsync("Error", ex.Message);
    //         return this.GetResponse(new BaseResponse
    //         {
    //             Message = ex.Message,
    //             Success = false,
    //             ResponseCode = (int)HttpStatusCode.InternalServerError
    //         });
    //     }
    // }

    // [HttpGet("{name}")]
    // public async Task<ActionResult<PersonAstronaut>> GetPersonByName(string name)
    // {
    //     try
    //     {
    //         var person = await _mediator.Send(new GetPersonByName { Name = name });
    //         if (person == null)
    //         {
    //             await LogProcessAsync("Info", $"Person '{name}' not found.");
    //             return NotFound($"No person found with name '{name}'.");
    //         }
    //         await LogProcessAsync("Info", $"Retrieved person '{name}'.");
    //         return Ok(person);
    //     }
    //     catch (Exception ex)
    //     {
    //         await LogProcessAsync("Error", ex.Message);
    //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    //     }
    // }



    // [HttpPost("")]
    // public async Task<IActionResult> CreatePerson([FromBody] string name)
    // {
    //     try
    //     {
    //         var result = await _mediator.Send(new CreatePerson { Name = name });

    //         await LogProcessAsync("Info", $"Created person '{name}' successfully.");
    //         return this.GetResponse(result);
    //     }
    //     catch (Exception ex)
    //     {
    //         await LogProcessAsync("Error", ex.Message);
    //         return this.GetResponse(new BaseResponse
    //         {
    //             Message = ex.Message,
    //             Success = false,
    //             ResponseCode = (int)HttpStatusCode.InternalServerError
    //         });
    //     }
    // }

    // Add or Update (Upsert) a Person by Name
    // [HttpPut("{name}")]
    // public async Task<IActionResult> UpsertPersonByName(string name, [FromBody] string newName)
    // {
    //     try
    //     {
    //         // Is there a Person matching the 'name' in route?
    //         var person = await _context.People
    //             .FirstOrDefaultAsync(p => p.Name == name);

    //         if (person == null)
    //         {
    //             // Add new
    //             person = new Person { Name = newName };
    //             _context.People.Add(person);
    //         }
    //         else
    //         {
    //             // Update existing
    //             person.Name = newName;
    //             _context.People.Update(person);
    //         }

    //         await _context.SaveChangesAsync();

    //         await LogProcessAsync("Info",
    //             $"Upserted (add/update) person from '{name}' to '{newName}'.");

    //         return this.GetResponse(new BaseResponse
    //         {
    //             Message = "Upsert successful.",
    //             Success = true,
    //             ResponseCode = (int)HttpStatusCode.OK
    //         });
    //     }
    //     catch (Exception ex)
    //     {
    //         await LogProcessAsync("Error", ex.Message);
    //         return this.GetResponse(new BaseResponse
    //         {
    //             Message = ex.Message,
    //             Success = false,
    //             ResponseCode = (int)HttpStatusCode.InternalServerError
    //         });
    //     }
    // }

    // private async Task LogProcessAsync(string level, string message)
    // {
    //     _context.ProcessLogs.Add(new ProcessLog
    //     {
    //         Level = level,
    //         Message = message,
    //         Timestamp = DateTime.UtcNow
    //     });
    //     await _context.SaveChangesAsync();
    // }
    // }
}
