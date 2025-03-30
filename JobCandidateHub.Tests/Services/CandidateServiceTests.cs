using System;
using System.Threading.Tasks;
using AutoMapper;
using JobCandidateHub.Application.DTOs.CandidateDTOs;
using JobCandidateHub.Application.Exceptions;
using JobCandidateHub.Application.Interfaces.Repositories;
using JobCandidateHub.Application.Services;
using JobCandidateHub.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace JobCandidateHub.Tests.Services
{
    public class CandidateServiceTests
    {
        private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CandidateService>> _loggerMock;
        private readonly CandidateService _candidateService;

        public CandidateServiceTests()
        {
            _candidateRepositoryMock = new Mock<ICandidateRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CandidateService>>();
            _candidateService = new CandidateService(_candidateRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateOrUpdate_ShouldCreateCandidate_WhenCandidateDoesNotExist()
        {
            var candidateDto = new CandidateDTO
            {
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };

            var candidateEntity = new Candidate
            {
                Email = candidateDto.Email,
                FirstName = candidateDto.FirstName,
                LastName = candidateDto.LastName
            };

            _candidateRepositoryMock.Setup(r => r.GetByEmailAsync(candidateDto.Email)).ReturnsAsync((Candidate)null);
            _mapperMock.Setup(m => m.Map<Candidate>(candidateDto)).Returns(candidateEntity);

            await _candidateService.CreateOrUpdate(candidateDto);

            _candidateRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Candidate>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrUpdate_ShouldUpdateCandidate_WhenCandidateExists()
        {
            var candidateDto = new CandidateDTO
            {
                Email = "existing@example.com",
                FirstName = "Updated",
                LastName = "User"
            };

            var existingCandidate = new Candidate
            {
                Email = candidateDto.Email,
                FirstName = "Old",
                LastName = "User"
            };

            _candidateRepositoryMock.Setup(r => r.GetByEmailAsync(candidateDto.Email)).ReturnsAsync(existingCandidate);
            _mapperMock.Setup(m => m.Map(candidateDto, existingCandidate));

            await _candidateService.CreateOrUpdate(candidateDto);

            _candidateRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Candidate>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrUpdate_ShouldHandleDbUpdateException()
        {
            var candidateDto = new CandidateDTO { Email = "test@example.com" };
            _candidateRepositoryMock.Setup(r => r.GetByEmailAsync(candidateDto.Email)).ThrowsAsync(new Exception("Database error"));

            await Assert.ThrowsAsync<Exception>(() => _candidateService.CreateOrUpdate(candidateDto));
        }
    }
}
