using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace ElectionBlockchain.API.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class LeaderController : BaseController
   {
      private readonly ILeaderService _leaderService;
      public LeaderController(ILeaderService leaderService, IMapper mapper) : base(mapper)
      {
         _leaderService = leaderService;
      }

      [HttpPost("vote")]
      public async Task<IActionResult> PostVoteAsync([FromBody] VoteQueue vote)
      {
         string result = await _leaderService.AddVoteQueueToQueueAsync(vote);
         return Ok(result);
      }
   }
}
