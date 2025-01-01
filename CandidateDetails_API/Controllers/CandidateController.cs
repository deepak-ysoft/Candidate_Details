using CandidateDetails_API.IServices;
using CandidateDetails_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Buffers;
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
                return Ok(new {success= Candidates });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetCandidates")]
        public async Task<IActionResult> GetCandidates(int page = 1, int pageSize = 10, string sortColumn = "id", string sortDirection = "asc", string SearchField = "", string SearchValue = "")
        {
            try
            {
                var totalRecordsParam = new SqlParameter("@TotalRecords", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
            

                // Define SQL parameters for the stored procedure
                var parameters = new[]
                {
                    new SqlParameter("@PageNumber", SqlDbType.Int) { Value = page },
                    new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize },
                    new SqlParameter("@SortColumn", SqlDbType.NVarChar, 50) { Value = sortColumn },
                    new SqlParameter("@SortOrder", SqlDbType.NVarChar, 4) { Value = sortDirection },
                    new SqlParameter("@SearchField", SqlDbType.NVarChar, 255) { Value = (object)SearchField ?? DBNull.Value },
                    new SqlParameter("@SearchValue", SqlDbType.NVarChar, 255) { Value = (object)SearchValue ?? DBNull.Value },
                    totalRecordsParam
                };

                // Call the stored procedure using FromSqlRaw
                var candidates = await _context.candidateDetails
                    .FromSqlRaw("EXEC usp_GetAllcandidate @PageNumber, @PageSize, @SortColumn, @SortOrder,@SearchField,@SearchValue,@TotalRecords OUT", parameters)
                    .ToListAsync();

                int totalRecords = (int)totalRecordsParam.Value;

                return Ok(new { data = candidates,totalCount= totalRecords });
            }
            catch (Exception ex)
            {
                // Log the error (Optional)
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AddEditCandidate")]
        public async Task<IActionResult> AddEditCandidate([FromForm] Candidate candidate)
            {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool res = false;

                // Save the CV file
                var CandidateCV = Path.Combine(Directory.GetCurrentDirectory(), "CandidateCV");
                if (!Directory.Exists(CandidateCV))
                {
                    Directory.CreateDirectory(CandidateCV);
                }
                if (candidate.id == null)
                    candidate.id = 0;
                if (candidate.id == 0)
                {
                    string beforeAt = candidate.email_ID.Split('@')[0];
                    // var uniqueFileName = $"{Guid.NewGuid()}_{candidate.cv.FileName}";
                    string fileName = $"{candidate.name}_Email_{beforeAt}{Path.GetExtension(candidate.cv.FileName)}";
                    var filePath = Path.Combine(CandidateCV, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await candidate.cv.CopyToAsync(stream);
                    }

                    // Save the file path in the database
                    candidate.cvPath = $"https://localhost:44319/CandidateCV/{fileName}";
                }
                else
                {
                    var can = await _context.candidateDetails.FirstOrDefaultAsync(x => x.id == candidate.id);
                    var previousCV = Path.GetFileName(can.cvPath);
                    var filePath = Path.Combine(CandidateCV, previousCV);

                    if (System.IO.File.Exists(filePath))
                    {
                        // Delete the file asynchronously
                        await Task.Run(() => System.IO.File.Delete(filePath));
                    }
                    string beforeAt = candidate.email_ID.Split('@')[0];
                    // var uniqueFileName = $"{Guid.NewGuid()}_{candidate.cv.FileName}";
                    string fileName = $"{candidate.name}_{candidate.id}_Email_{beforeAt}{Path.GetExtension(candidate.cv.FileName)}";
                    var UpdatefilePath = Path.Combine(CandidateCV, fileName);

                    using (var stream = new FileStream(UpdatefilePath, FileMode.Create))
                    {
                        await candidate.cv.CopyToAsync(stream);
                    }

                    // Save the file path in the database
                    candidate.cvPath = $"https://localhost:44319/CandidateCV/{fileName}";
                }
                // Save candidate details
                res = await _service.AddEditCandidate(candidate);


                return Ok(new { success = res });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("DeleteCandidate/{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            try
            {
                bool res = await _service.deleteCanndidate(id);
                return Ok(new { success = res });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("DownloadCV/{candidateId}")]
        public async Task<IActionResult> DownloadCV(int candidateId)
        {
            var candidate = await _context.candidateDetails.FirstOrDefaultAsync(x => x.id == candidateId);
            string getFileName = Path.GetFileName(candidate.cvPath);
            // Path where CV files are stored
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "CandidateCV", getFileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = $"{candidate.name}_CV.pdf";

            return File(fileBytes, "application/pdf", fileName);
        }
    }
}
