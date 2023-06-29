using ElectionBlockchain.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Services.Interfaces
{
   public interface ILeaderService
   {
      Task<string> AddVoteQueueToQueueAsync(VoteQueue vote);
      Task CreateAndAddNextBlock();
      Task<bool> CheckVerifierConfirmationAsync(string confirmation);
      Task<string> SignVotesAsync(List<VoteQueue> voteQueues, string privateKey); //in BaseNodeService
      Task<bool> VerifyVoteAsync(VoteQueue voteQueue);
      int GetNodeId();
      void SetNodeId(int id);
      Task SetPublicPrivateKeyAsync(RSAParameters publicPrivateKey);
      Task<string> GetPublicPrivateKeyAsync();
      Task<string> GenerateNodeKeysAsync();



   }
}
