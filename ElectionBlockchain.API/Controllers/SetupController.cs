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
   public class SetupController : BaseController
   {
      private readonly ILeaderService _leaderService;
      private readonly IVerifierService _verifierService;

      public SetupController(ILeaderService leaderService, IVerifierService verifierService, IMapper mapper) : base(mapper)
      {
         _leaderService = leaderService;
         _verifierService = verifierService;
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

      [HttpGet("node/PrivateKey")]
      public async Task<IActionResult> GetKeyAsync()
      {
         var jsonKey = await _leaderService.GetPublicPrivateKeyAsync();
         return Content(jsonKey, "application/json");
      }

      [HttpPost("node/PrivateKey")]
      public async Task<IActionResult> PostSetKeyAsync([FromBody] RSAParametersDto publicPrivateKey)
      {
         await _leaderService.SetPublicPrivateKeyAsync(publicPrivateKey);
         var jsonKey = await _leaderService.GetPublicPrivateKeyAsync();
         return Content(jsonKey, "application/json");
      }
      //
      [HttpGet("node/PublicKey/{nodeRole}")]
      public async Task<IActionResult> GetPublicKeyAsync(string nodeRole)
      {
         var jsonKey = await _verifierService.GetPublicKeyAsync(nodeRole);
         return Content(jsonKey, "application/json");
      }

      [HttpPost("node/PublicKey/{nodeRole}")]
      public async Task<IActionResult> PostSetPublicKeyAsync([FromBody] RSAParametersDto publicKey, string nodeRole)
      {
         await _verifierService.SetPublicKeyAsync(publicKey, nodeRole);
         var jsonKey = await _verifierService.GetPublicKeyAsync(nodeRole);
         return Content(jsonKey, "application/json");
      }
      //
      [HttpGet("node/ip")]
      public async Task<IActionResult> GetNodeIpAsync()
      {
         return Ok(HttpContext.Connection.LocalIpAddress.ToString().Replace("::ffff:", ""));

      }
      [HttpGet("GenerateNodeKeys")]
      public async Task<IActionResult> GetGenerateNodeKeysAsync()
      {
         var jsonKeys = await _leaderService.GenerateNodeKeysAsync();
         return Content(jsonKeys, "application/json");
      }
      [HttpPost("node/{nodeRole}/{ip}")]
      public async Task<IActionResult> PostSetUrlAsync(string nodeRole, string ip)
      {
         
         await _verifierService.SetNodeUrlAsync(nodeRole, ip);

         var jsonNodeIp = await _verifierService.GetNodeUrlAsync(nodeRole);
         return Content(jsonNodeIp, "application/json");
      }
   }
}