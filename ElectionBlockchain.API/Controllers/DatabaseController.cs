using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ElectionBlockchain.API.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class DatabaseController : BaseController
   {
      private readonly IDatabaseService _databaseService;
      public DatabaseController(IDatabaseService _databaseService, IMapper mapper) : base(mapper)
      {
         _databaseService = _databaseService;
      }
      //3 nodes will be added in a special order, 1- Verifier1 2- Verifier2 3- Leader
      [HttpPost("addnode")]
      public async Task<IActionResult> AddNodeASync([FromBody] Node node)
      {
         Node n = await _databaseService.AddNodeAsync(node);
         if (n == null)
            return BadRequest("Node is not valid");

         return Ok(n);
      }


      [HttpDelete("tables/{table}")]
      public void Clean(string table)
      {
         _databaseService.CleanAsync(table);
      }

      [HttpGet("tables/{table}")]
      public async Task<IActionResult> GetTableAsync(string table)
      {
         var tableData = _databaseService.GetTableAsync(table);
         var body = new StringContent(tableData.ToString(), Encoding.UTF8, "application/json");
         return Ok(body);
      }
   }
}