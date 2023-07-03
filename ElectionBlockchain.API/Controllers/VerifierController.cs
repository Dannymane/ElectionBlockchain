using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Model.DataTrasferObjects;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace ElectionBlockchain.API.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class VerifierController : BaseController
   {
      private readonly IVerifierService _verifierService;
      public VerifierController(IVerifierService verifierService, IMapper mapper) : base(mapper)
      {
         _verifierService = verifierService;
      }

      [HttpPost("VotesFromLeader")]
      public async Task<IActionResult> Post([FromBody] SignedVotesDto content)
      {
         return Content(await _verifierService.ReceiveVotesAsync(content), "application/json");
      }
   }
}