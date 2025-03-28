using JobCandidateHub.Application.DTOs.CandidateDTOs;

namespace JobCandidateHub.Application.Interfaces.Services;

public interface ICandidateService
{
    Task CreateOrUpdate(CandidateDTO candidateDTO);
}