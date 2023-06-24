using ElectionBlockchain.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Services.Interfaces
{
   public interface IVerifierService
   {
      Task<bool> VerifyVote(VoteQueue voteQueue);
      Task<bool> VerifyVotes(string requestBodyWithVotes);
      string SignVoteByLeader(List<VoteQueue> voteQueues);
      void SendConfirmationToLeader(string confirmation);

   }
}
