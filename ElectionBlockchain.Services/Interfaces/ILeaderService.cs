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
      Task<VoteQueue> AddVoteQueueToQueue(VoteQueue voteQueue);
      Boolean VerifyVote(VoteQueue voteQueue);
      void CreateAndAddNextBlock();
      Boolean CheckVerifierConfirmation(string confirmation);


   }
}
