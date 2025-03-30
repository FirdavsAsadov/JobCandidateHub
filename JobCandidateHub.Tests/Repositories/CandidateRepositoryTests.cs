using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using JobCandidateHub.Domain.Entities;
using JobCandidateHub.Infrastructure.Persistence;
using JobCandidateHub.Infrastructure.Repositories;

public class CandidateRepositoryTests
{
    private readonly CandidateRepository _candidateRepository;
    private readonly ApplicationDbContext _dbContext;

    public CandidateRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _candidateRepository = new CandidateRepository(_dbContext);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCandidate()
    {
        var candidate = new Candidate { Id = 1, FirstName = "Firdavs", LastName = "Asadov", Email = "firdavsasadov5831@gmail.com" };

        await _candidateRepository.CreateAsync(candidate);
        var result = await _dbContext.Candidates.FirstOrDefaultAsync(c => c.Id == 1);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(candidate);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnCandidate()
    {
        var candidate = new Candidate { Id = 2, FirstName = "Firdavs", LastName = "Firdavs", Email = "firdavsasadov5831@gmail.com" };
        await _dbContext.Candidates.AddAsync(candidate);
        await _dbContext.SaveChangesAsync();

        var result = await _candidateRepository.GetByEmailAsync("firdavsasadov5831@gmail.com");

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(candidate);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCandidate()
    {
        var candidate = new Candidate { Id = 3, FirstName = "Alice", LastName = "Smith", Email = "alice@gmail.com" };
        await _dbContext.Candidates.AddAsync(candidate);
        await _dbContext.SaveChangesAsync();

        candidate.LastName = "Johnson";
        await _candidateRepository.UpdateAsync(candidate);
        var result = await _dbContext.Candidates.FindAsync(3);

        result.Should().NotBeNull();
        result.LastName.Should().Be("Johnson");
    }
}
