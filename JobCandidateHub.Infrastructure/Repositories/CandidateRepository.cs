using JobCandidateHub.Application.Interfaces.Repositories;
using JobCandidateHub.Domain.Entities;
using JobCandidateHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobCandidateHub.Infrastructure.Repositories;
public class CandidateRepository : ICandidateRepository
{
    private readonly ApplicationDbContext _context;

    public CandidateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Candidate candidate)
    {
        await _context.Candidates.AddAsync(candidate);
    }


    public async Task<Candidate?> GetByEmailAsync(string email)
    {
        return await _context.Candidates.FirstOrDefaultAsync(c => c.Email == email);

    }

    public async Task UpdateAsync(Candidate candidate)
    {
         _context.Candidates.Update(candidate);
    }
}
