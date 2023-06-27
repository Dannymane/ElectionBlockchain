using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElectionBlockchain.API.Controllers
{
   public class LeaderController : BaseController
   {
      private readonly ILeaderService _leaderService;
      public LeaderController(ILeaderService leaderService, IMapper mapper) : base(mapper)
      {
         _leaderService = leaderService;
      }

      [HttpGet("node/id")]
      public IActionResult GetNodeId()
      {
         return Ok(_leaderService.GetNodeId());
      }

      [HttpPost("node/{id}")]
      public IActionResult PostNodeId(int id)
      {
         _leaderService.SetNodeId(id);
         return Ok(_leaderService.GetNodeId());
      }

      [HttpPost("vote")]
      public async Task<IActionResult> PostVoteAsync([FromBody] VoteQueue vote)
      {
         string result = await _leaderService.AddVoteQueueToQueueAsync(vote);
         return Ok(result);
      }
   }
}
