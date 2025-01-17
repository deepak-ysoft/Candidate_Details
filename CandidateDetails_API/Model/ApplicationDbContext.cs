using CandidateDetails_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateDetails_API.Model
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base (options) { }
        public DbSet<Candidate> candidateDetails { get; set; } // DbSet for Candidate model
        public DbSet<Roles> Roles { get; set; } // DbSet for Candidate model
        public DbSet<Calendar> calendar { get; set; } // DbSet for Calendar model
    }
}
