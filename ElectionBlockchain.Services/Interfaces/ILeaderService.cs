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
   public interface ILeaderService
   {
      Task<string> AddVoteQueueToQueueAsync(VoteQueue vote);
      Task CreateAndAddNextBlock();
      Task<bool> CheckVerifierConfirmationAsync(SignedVotesDto confirmation);
      Task<string> SignVotesAsync(List<VoteQueue> voteQueues); //in BaseNodeService
      Task<bool> VerifyVoteAsync(VoteQueue voteQueue);
      int GetNodeId();
      void SetNodeId(int id);
      public Task SetPublicPrivateKeyAsync(RSAParametersDto publicPrivateKeyString);
      Task<string> GetPublicPrivateKeyAsync();
      Task<string> GenerateNodeKeysAsync();



   }
}
