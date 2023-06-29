using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ElectionBlockchain.API.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class DatabaseController : BaseController
   {
      private readonly IDatabaseService _databaseService;
      public DatabaseController(IDatabaseService databaseService, IMapper mapper) : base(mapper)
      {
         _databaseService = databaseService;
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
      public IActionResult Clean(string table)
      {
         int affectedRows = _databaseService.Clean(table);
         return Ok("Deleted rows: " + affectedRows);
      }

      [HttpGet("tables/{table}")]
      public async Task<IActionResult> GetTableAsync(string table)
      { //must be await _databaseService, without await I have to use .Result
         var tableData = await _databaseService.GetTableAsync(table);
         return Content(tableData, "application/json");
      }

      [HttpGet("resetCitizensAndRelatedTables")]
      public IActionResult GetResetCitizensAndRelatedTables()
      {
         string jsonString = _databaseService.ResetCitizensAndRelatedTables();

         return Content(jsonString, "application/json");
      }




   }
}