using JobCandidateHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobCandidateHub.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Candidate> Candidates { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidate>()
            .HasIndex(c => c.Email)
            .IsUnique(); 

        base.OnModelCreating(modelBuilder);
    }
}
