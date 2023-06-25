using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElectionBlockchain.API.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class SetupController : BaseController
   {
      private readonly IDatabaseService _databaseService;
      public SetupController(IDatabaseService _databaseService, IMapper mapper) : base(mapper)
      {
         _databaseService = _databaseService;
      }
      //3 nodes will be added in a special order, 1- Verifier1 2- Verifier2 3- Leader
      [HttpPost("addnode")]
      public async Task<IActionResult> AddNodeASync([FromBody] Node node)
      {
         Task<Node> n = await _databaseService.AddNodeAsync(node);

         return Ok(n);
      }


      [HttpDelete("cleantable/{table}")]
      public void Clean(string table)
      {
         _databaseService.CleanAsync(table);
      }

   }
}
