using CandidateDetails_API.Model;

namespace CandidateDetails_API.IServices
{
    public interface ICandidateService
    {
        public Task<bool> AddCandidates(Stream fileStream);
        public Task<bool> AddEditCandidate(Candidate candidate);
        public Task<List<Candidate>> GetCandidates();
        public Task<bool> deleteCanndidate(int id);
    }
}
