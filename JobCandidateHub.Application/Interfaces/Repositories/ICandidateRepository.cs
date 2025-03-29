using JobCandidateHub.Domain.Entities;

namespace JobCandidateHub.Application.Interfaces.Repositories;

public interface ICandidateRepository
{
    Task<Candidate?> GetByEmailAsync(string email);
    Task CreateAsync(Candidate candidate);
    Task UpdateAsync(Candidate candidate);
}
