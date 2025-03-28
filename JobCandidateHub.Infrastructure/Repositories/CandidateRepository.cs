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

    public async Task CreateOrUpdateAsync(Candidate candidate)
    {
        if (candidate == null)
        {
            throw new ArgumentNullException(nameof(candidate), "Candidate cannot be null.");
        }

        try
        {
            var existingCandidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Email == candidate.Email);

            if (existingCandidate != null)
            {
                existingCandidate.FirstName = candidate.FirstName;
                existingCandidate.LastName = candidate.LastName;
                existingCandidate.PhoneNumber = candidate.PhoneNumber;
                existingCandidate.Email = candidate.Email;
                existingCandidate.CallTimeInterval = candidate.CallTimeInterval;
                existingCandidate.LinkedInUrl = candidate.LinkedInUrl;
                existingCandidate.Comment = candidate.Comment;
                existingCandidate.GitHubUrl = candidate.GitHubUrl;

                _context.Candidates.Update(existingCandidate);
            }
            else
            {
                await _context.Candidates.AddAsync(candidate);
            }

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Database update error occurred.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new Exception("An invalid operation occurred.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred.", ex);
        }
    }


    public Task<List<Candidate>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Candidate?> GetByEmailAsync(string email)
    {
        return await _context.Candidates.FirstOrDefaultAsync(c => c.Email == email);

    }
}
