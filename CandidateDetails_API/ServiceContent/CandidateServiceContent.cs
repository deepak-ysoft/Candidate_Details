using CandidateDetails_API.IServices;
using CandidateDetails_API.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Drawing.Printing;
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

        public async Task<bool> AddCandidate(Candidate candidate)
        {
            await _context.candidateDetails.AddAsync(candidate);
            int res = await _context.SaveChangesAsync();
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
                        Date = DateTime.Parse(worksheet.Cells[row, 2].Text),
                        Name = worksheet.Cells[row, 3].Text,
                        Contact_No = worksheet.Cells[row, 4].Text,
                        Linkedin_Profile = worksheet.Cells[row, 5].Text,
                        Email_ID = worksheet.Cells[row, 6].Text,
                        Roles = worksheet.Cells[row, 7].Text,
                        Experience = worksheet.Cells[row, 8].Text,
                        Skills = worksheet.Cells[row, 9].Text,
                        CTC = decimal.Parse(worksheet.Cells[row, 10].Text),
                        ETC = decimal.Parse(worksheet.Cells[row, 11].Text),
                        Notice_Period = worksheet.Cells[row, 12].Text,
                        Current_Location = worksheet.Cells[row, 13].Text,
                        Prefer_Location = worksheet.Cells[row, 14].Text,
                        Reason_For_Job_Change = worksheet.Cells[row, 15].Text,
                        Schedule_Interview = DateTime.Parse(worksheet.Cells[row, 16].Text),
                        Schedule_Interview_status = worksheet.Cells[row, 17].Text,
                        Comments = worksheet.Cells[row, 18].Text
                    };
                    if (double.TryParse(candidate.Contact_No, out double number))
                    {
                        // Convert to plain text without scientific notation
                        candidate.Contact_No = number.ToString("F0", CultureInfo.InvariantCulture);
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
    }
}
