using JobCandidateHub.Domain.Entities;

namespace JobCandidateHub.Application.Interfaces.Repositories;

public interface ICandidateRepository
{
    Task<Candidate?> GetByEmailAsync(string email);
    Task CreateOrUpdateAsync(Candidate candidate);
    Task<List<Candidate>> GetAllAsync();
}
