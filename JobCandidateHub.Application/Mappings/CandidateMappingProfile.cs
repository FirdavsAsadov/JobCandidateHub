using AutoMapper;
using JobCandidateHub.Application.DTOs.CandidateDTOs;
using JobCandidateHub.Domain.Entities;

namespace JobCandidateHub.Application.Mappings;

public class CandidateMappingProfile : Profile
{
    public CandidateMappingProfile()
    {
        CreateMap<Candidate, CandidateDTO>().ReverseMap();
    }
}
