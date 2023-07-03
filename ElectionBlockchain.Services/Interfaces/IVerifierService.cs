using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Model.DataTrasferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Services.Interfaces
{
   public interface IVerifierService
   {
      public static int NodeId { get; set; } = 0;
      Task<string> SignVotesAsync(List<VoteQueue> voteQueues); //in BaseNodeService
      Task<bool> VerifyVoteAsync(VoteQueue voteQueue); //in BaseNodeService
      int GetNodeId();
      void SetNodeId(int id);
      Task SetPublicPrivateKeyAsync(RSAParametersDto publicPrivateKeyString);
      public Task SetPublicKeyAsync(RSAParametersDto publicKeyDto, string nodeRole);
      public Task<string> GetPublicKeyAsync(string nodeRole);
      Task SetNodeUrlAsync(string nodeRole, string ip);
      Task<string> GetNodeUrlAsync(string nodeRole);
      Task<string> GetPublicPrivateKeyAsync();
      Task<string> GenerateNodeKeysAsync();


   }
}
