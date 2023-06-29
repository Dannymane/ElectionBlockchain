using ElectionBlockchain.Model.DataModels;
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
      Task<bool> VerifyVotesAsync(string requestBodyWithVotes);
      void SendConfirmationToLeader(string confirmation);

      Task<string> SignVotesAsync(List<VoteQueue> voteQueues, string privateKey); //in BaseNodeService
      Task<bool> VerifyVoteAsync(VoteQueue voteQueue); //in BaseNodeService
      int GetNodeId();
      void SetNodeId(int id);
      Task SetPublicPrivateKeyAsync(RSAParameters publicPrivateKey);
      Task<string> GetPublicPrivateKeyAsync();
      Task<string> GenerateNodeKeysAsync();

   }
}
