using ElectionBlockchain.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Services.Interfaces
{
   public interface ILeaderService
   {
      void AddVoteQueueToQueueAsync(string voteString);
      bool CreateAndAddNextBlock();
      Task<bool> CheckVerifierConfirmationAsync(string confirmation);
      Task<string> SignVotesAsync(List<VoteQueue> voteQueues, string privateKey); //in BaseNodeService
      Task<bool> VerifyVoteAsync(VoteQueue voteQueue);
      int GetNodeId();
      void SetNodeId(int id);

   }
}
