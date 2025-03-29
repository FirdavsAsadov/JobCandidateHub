using JobCandidateHub.Application.DTOs.CandidateDTOs;
using JobCandidateHub.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobCandidateHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidateController : ControllerBase
{
    private readonly ICandidateService _candidateService;

    public CandidateController(ICandidateService candidateService)
    {
        _candidateService=candidateService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate([FromBody] CandidateDTO candidateDTO)
    {
        await _candidateService.CreateOrUpdate(candidateDTO);
        return Ok();
    }
}
