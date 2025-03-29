using AutoMapper;
using JobCandidateHub.Application.DTOs.CandidateDTOs;
using JobCandidateHub.Application.Interfaces.Repositories;
using JobCandidateHub.Application.Interfaces.Services;
using JobCandidateHub.Domain.Entities;

namespace JobCandidateHub.Application.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IMapper _mapper;
        public CandidateService(ICandidateRepository candidateRepository, IMapper mapper)
        {
            _candidateRepository=candidateRepository;
            _mapper=mapper;
        }

        public async Task CreateOrUpdate(CandidateDTO candidateDTO)
        {

            var exsists = await IfExsits(candidateDTO.Email);

            if (exsists)
            {
                var candidate = await _candidateRepository.GetByEmailAsync(candidateDTO.Email);
                candidate = _mapper.Map(candidateDTO, candidate);
                await _candidateRepository.UpdateAsync(candidate);
            }
            else
            {
                var candidate = _mapper.Map<Candidate>(candidateDTO);
                await _candidateRepository.CreateAsync(candidate);
            }
        }

        private async Task<bool> IfExsits(string email)
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
