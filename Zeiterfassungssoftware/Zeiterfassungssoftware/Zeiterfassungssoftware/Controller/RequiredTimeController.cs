using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Zeiterfassungssoftware.Data;
using Zeiterfassungssoftware.Mapper;
using Zeiterfassungssoftware.SharedData.Classes;
using Zeiterfassungssoftware.SharedData.RequiredTimes;
using Zeiterfassungssoftware.SharedData.Times;

namespace Zeiterfassungssoftware.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class RequiredTimesController : ControllerBase
    {
        private ApplicationDbContext _context;

        public RequiredTimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of all the RequiredTimes.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RequiredTimeDto>))]
        public async Task<ActionResult<List<RequiredTimeDto>>> GetRequiredTimes()
        {
            var RequiredTimes = await _context.RequiredTimes.Where(e => e.ValidUntil > DateTime.Now)
                                                        .Select(e => RequiredTimeMapper.ToDTO(e))
                                                        .ToListAsync();
            return Ok(RequiredTimes);
        }

        /// <summary>
        /// Gets a specific RequiredTime.
        /// </summary>
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequiredTimeDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RequiredTimeDto>> GetRequiredTimeById(Guid id)
        {
            var RequiredTime = await _context.RequiredTimes.FirstOrDefaultAsync(e => e.Id == id && e.ValidUntil > DateTime.Now);

            if(RequiredTime is null)
                return NotFound();

            return Ok(RequiredTimeMapper.ToDTO(RequiredTime));
        }

        /// <summary>
        /// Updates a specific RequiredTime.
        /// </summary>
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequiredTimeDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RequiredTimeDto>> UpdateRequiredTime(Guid id, [FromBody, Required] RequiredTimeDto RequiredTimeDto)
        {
            if (!RequiredTimeMapper.ValidateDto(RequiredTimeDto))
                return BadRequest();

            var OldRequiredTime = _context.RequiredTimes.FirstOrDefault(e => e.Id == id);
            if (OldRequiredTime is null)
                return NotFound();

            if (OldRequiredTime.ValidUntil < DateTime.Now)
                return NotFound();

            OldRequiredTime.ValidUntil = DateTime.Now;

            var NewRequiredTime = RequiredTimeMapper.FromDTO(RequiredTimeDto);
            NewRequiredTime.Id = Guid.NewGuid();
            _context.RequiredTimes.Add(NewRequiredTime);

            await _context.SaveChangesAsync();

            return Ok(RequiredTimeMapper.ToDTO(NewRequiredTime));
        }


        /// <summary>
        /// Creates a new RequiredTime
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRequiredTime([FromBody] RequiredTimeDto RequiredTimeDto)
        {
            if (!RequiredTimeMapper.ValidateDto(RequiredTimeDto))
                return BadRequest("Invalid data");

            var RequiredTime = RequiredTimeMapper.FromDTO(RequiredTimeDto);
            RequiredTime.Id = Guid.NewGuid();
            RequiredTime.ValidUntil = DateTime.MaxValue;

            _context.RequiredTimes.Add(RequiredTime);
            await _context.SaveChangesAsync();

            return Ok(RequiredTimeMapper.ToDTO(RequiredTime));
        }

        /// <summary>
        /// Deletes a specific RequiredTime.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRequiredTime(Guid id)
        {
            var RequiredTime = await _context.RequiredTimes.FirstOrDefaultAsync(e => e.Id == id);
            if (RequiredTime is null || RequiredTime.ValidUntil < DateTime.Now)
                return NotFound();

            RequiredTime.ValidUntil = DateTime.Now;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
