using AutoMapper;
using ElectionBlockchain.Model.DataTrasferObjects;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElectionBlockchain.API.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class TestController : BaseController
   {
      private readonly ILeaderService _leaderService;
      private readonly IVerifierService _verifierService;

      public TestController(ILeaderService leaderService, IVerifierService verifierService, IMapper mapper) : base(mapper)
      {
         _leaderService = leaderService;
         _verifierService = verifierService;
      }
      [HttpPost("votes")]
      public IActionResult PostVotesAsync([FromBody] SignedVotesDto content) 
      {

         return Ok(_leaderService.CheckVerifierConfirmationAsync(content));
            

      }
      [HttpGet("CreateBlock")]
      public async Task GetCreateBlockAsync()
      {

         await _leaderService.CreateAndAddNextBlock();


      }

   }
}

