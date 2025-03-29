using AutoMapper;
using JobCandidateHub.Application.DTOs.CandidateDTOs;
using JobCandidateHub.Application.Exceptions;
using JobCandidateHub.Application.Interfaces.Repositories;
using JobCandidateHub.Application.Interfaces.Services;
using JobCandidateHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobCandidateHub.Application.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CandidateService> _logger;

        public CandidateService(ICandidateRepository candidateRepository, IMapper mapper, ILogger<CandidateService> logger)
        {
            _candidateRepository=candidateRepository;
            _mapper=mapper;
            _logger=logger;
        }

        public async Task CreateOrUpdate(CandidateDTO candidateDTO)
        {
            try
            {
                var exists = await IfExists(candidateDTO.Email);

                if (exists)
                {
                    var candidate = await _candidateRepository.GetByEmailAsync(candidateDTO.Email);
                    if (candidate == null)
                    {
                        _logger.LogWarning($"Candidate with email {candidateDTO.Email} not found while updating.", candidateDTO.Email);
                        throw new NotFoundException($"Candidate with email {candidateDTO.Email} not found.");
                    }

                    _mapper.Map(candidateDTO, candidate);
                    await _candidateRepository.UpdateAsync(candidate);
                    _logger.LogInformation($"Candidate with email {candidateDTO.Email} successfully updated.", candidateDTO.Email);
                }
                else
                {
                    var candidate = _mapper.Map<Candidate>(candidateDTO);
                    await _candidateRepository.CreateAsync(candidate);
                    _logger.LogInformation($"Candidate with email {candidateDTO.Email} successfully created.", candidateDTO.Email);
                }
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, $"NotFoundException occurred: {ex.Message}", ex.Message);
                throw; 
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred.");
                throw new Exception("Database error while saving data. Please try again later.");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, $"A null argument was passed: {ex.Message}", ex.Message);
                throw new Exception("Invalid data received.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                throw new Exception("Something went wrong while processing your request.");
            }
        }

        private async Task<bool> IfExists(string email)
        {
            var candidate = await _candidateRepository.GetByEmailAsync(email);

            if(candidate != null)
            {
                return true;
            }

            return false;
        }
    }
}
