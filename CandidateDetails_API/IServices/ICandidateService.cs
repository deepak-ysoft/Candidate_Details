using CandidateDetails_API.Model;

namespace CandidateDetails_API.IServices
{
    public interface ICandidateService
    {
        public Task<bool> AddCandidates(Stream fileStream);
        public Task<bool> AddCandidate(Candidate candidate);
        public Task<List<Candidate>> GetCandidates();
    }
}
