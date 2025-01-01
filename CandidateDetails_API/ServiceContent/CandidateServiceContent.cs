using CandidateDetails_API.IServices;
using CandidateDetails_API.Model;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Globalization;

namespace CandidateDetails_API.ServiceContent
{
    public class CandidateServiceContent : ICandidateService
    {
        private readonly ApplicationDbContext _context;
        public CandidateServiceContent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEditCandidate(Candidate candidate)
        {
            int res = 0;
            if (candidate.id == 0)
            {
                candidate.isDelete = false;
                await _context.candidateDetails.AddAsync(candidate);
                res = await _context.SaveChangesAsync();
            }
            else
            {
                var existingEntity = _context.ChangeTracker.Entries<Candidate>().FirstOrDefault(e => e.Entity.id == candidate.id);

                if (existingEntity != null)
                {
                    _context.Entry(existingEntity.Entity).State = EntityState.Detached;
                }

                candidate.isDelete = false;
                _context.Entry(candidate).State = EntityState.Modified;
                // _context.candidateDetails.Update(candidate);
                res = await _context.SaveChangesAsync();
            }
            if (res == 0)
                return false;
            return true;
        }

        public async Task<bool> AddCandidates(Stream fileStream)
        {
            var Candidates = new List<Candidate>();
            int n = 0;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Required for EPPlus

            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0]; // First sheet
                int filledRowCount = 0;
                // Loop through each row
                for (int row = worksheet.Dimension.Start.Row; row <= worksheet.Dimension.End.Row; row++)
                {
                    bool isRowFilled = false;

                    // Check each cell in the row
                    for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                    {
                        var cellValue = worksheet.Cells[row, col].Value;
                        if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            isRowFilled = true;
                            break; // Exit the column loop once a filled cell is found
                        }
                    }
                    if (isRowFilled)
                    {
                        filledRowCount++;
                    }
                }
                for (int row = 2; row <= filledRowCount; row++) // Headers are in the first row
                {
                    var candidate = new Candidate
                    {
                        id = int.Parse(worksheet.Cells[row, 1].Text),
                        date = DateTime.Parse(worksheet.Cells[row, 2].Text),
                        name = worksheet.Cells[row, 3].Text,
                        contact_No = worksheet.Cells[row, 4].Text,
                        linkedin_Profile = worksheet.Cells[row, 5].Text,
                        email_ID = worksheet.Cells[row, 6].Text,
                        roles = worksheet.Cells[row, 7].Text,
                        experience = worksheet.Cells[row, 8].Text,
                        skills = worksheet.Cells[row, 9].Text,
                        ctc = decimal.Parse(worksheet.Cells[row, 10].Text),
                        etc = decimal.Parse(worksheet.Cells[row, 11].Text),
                        notice_Period = worksheet.Cells[row, 12].Text,
                        current_Location = worksheet.Cells[row, 13].Text,
                        prefer_Location = worksheet.Cells[row, 14].Text,
                        reason_For_Job_Change = worksheet.Cells[row, 15].Text,
                        schedule_Interview = DateTime.Parse(worksheet.Cells[row, 16].Text),
                        schedule_Interview_status = worksheet.Cells[row, 17].Text,
                        comments = worksheet.Cells[row, 18].Text,
                        isDelete=false
                    };
                    if (double.TryParse(candidate.contact_No, out double number))
                    {
                        // Convert to plain text without scientific notation
                        candidate.contact_No = number.ToString("F0", CultureInfo.InvariantCulture);
                    }
                    Candidates.Add(candidate);
                }
            }
            await _context.candidateDetails.AddRangeAsync(Candidates);
            n = await _context.SaveChangesAsync();
            if (n == 0)
                return false;
            return true;
        }

        public async Task<List<Candidate>> GetCandidates()
        {
            var candidates = await _context.candidateDetails.ToListAsync();
            return candidates;
        }

        public async Task<bool> deleteCanndidate(int id)
        {
            var candidate = await _context.candidateDetails.Where(x => x.id == id).FirstOrDefaultAsync();
            var candidateObj = new Candidate
            {
                id = candidate.id,
                date = candidate.date,
                name = candidate.name,
                contact_No = candidate.contact_No,
                linkedin_Profile = candidate.linkedin_Profile,
                email_ID = candidate.email_ID,
                roles = candidate.roles,
                experience = candidate.experience,
                skills = candidate.skills,
                ctc = candidate.ctc,
                etc = candidate.etc,
                notice_Period = candidate.notice_Period,
                current_Location = candidate.current_Location,
                prefer_Location = candidate.prefer_Location,
                reason_For_Job_Change = candidate.reason_For_Job_Change,
                schedule_Interview = candidate.schedule_Interview,
                schedule_Interview_status = candidate.schedule_Interview_status,
                comments = candidate.comments,
                cvPath = candidate.cvPath,
                isDelete = false,
            };
            var existingEntity = _context.ChangeTracker.Entries<Candidate>().FirstOrDefault(e => e.Entity.id == candidate.id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity.Entity).State = EntityState.Detached;
            }
            _context.Entry(candidateObj).State = EntityState.Modified;
            var res = await _context.SaveChangesAsync();
            if (res == 0)
                return false;
            return true;
        }
    }
}
