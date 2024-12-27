using CandidateDetails_API.IServices;
using CandidateDetails_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CandidateDetails_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _service;
        private readonly ApplicationDbContext _context;
        public CandidateController(ICandidateService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }
        /// <summary>
        /// To Add candidate in database from excel file  
        /// </summary>
        /// <param name="file">excel file object</param>
        /// <returns>Boolean </returns>
        [HttpPost("AddCandidatesFromExcel")]
        public async Task<IActionResult> AddCandidatesFromExcel(IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    return BadRequest("File not found");
                }

                var stream = file.OpenReadStream();
                var Candidates = await _service.AddCandidates(stream);
                return Ok(Candidates);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetCandidates")]
        public async Task<IActionResult> GetCandidates()
        {
            try
            {
             var candidate = await _service.GetCandidates();
                return Ok(candidate);
            }
            catch (Exception ex)
            {
                // Log the error (Optional)
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("AddCandidate")]
        public async Task<IActionResult> AddCandidate(Candidate candidate)
        {
            try
            {
                if (candidate == null)
                {
                    return BadRequest("File not found");
                }
                bool res = await _service.AddCandidate(candidate);
                return Ok(new { success = res });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
