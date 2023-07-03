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
      Task<string> SignVotesAsync(List<VoteQueue> voteQueues); //in BaseNodeService
      Task<bool> VerifyVoteAsync(VoteQueue voteQueue); //in BaseNodeService
      Task<string> ReceiveVotesAsync(SignedVotesDto confirmation);
      int GetNodeId();
      void SetNodeId(int id);
      Task SetPublicPrivateKeyAsync(RSAParametersDto publicPrivateKeyString);
      Task SetPublicKeyAsync(RSAParametersDto publicKeyDto, string nodeRole);
      Task<string> GetPublicKeyAsync(string nodeRole);
      Task SetNodeUrlAsync(string nodeRole, string ip);
      Task<string> GetNodeUrlAsync(string nodeRole);
      Task<string> GetPublicPrivateKeyAsync();
      Task<string> GenerateNodeKeysAsync();



   }
}
