using Microsoft.EntityFrameworkCore;

namespace CandidateDetails_API.Model
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base (options) { }
        public DbSet<Candidate> candidateDetails { get; set; }
    }
}
