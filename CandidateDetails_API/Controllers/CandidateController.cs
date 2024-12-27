using CandidateDetails_API.IServices;
using CandidateDetails_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<IActionResult> GetCandidates(int page = 1, int pageSize = 10, string searchTerm = "")
        {
            try
            {
                var query = _context.candidateDetails.AsQueryable(); // Make it Queryable

                if (!string.IsNullOrEmpty(searchTerm)) // If user want to search something
                {
                    query = query.Where(c => c.Name.Contains(searchTerm) ||
                                             c.Email_ID.Contains(searchTerm) ||
                                             (c.id).ToString().Contains(searchTerm));
                }

                var totalCount = await query.CountAsync(); // Count pages for example if 1000 customer in list then according pageSize=10 here totalCount of page is 100

                var customers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                    .ToListAsync(); // When user click on another page 

                return Ok(new { data = customers, totalCount = totalCount });
            }
            catch (Exception ex)
            {
                throw ex;
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
