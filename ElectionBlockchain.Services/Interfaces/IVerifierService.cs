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
      Task<bool> VerifyVotesAsync(string requestBodyWithVotes);
      void SendConfirmationToLeader(string confirmation);

      //Task<string> SignVotesAsync(List<VoteQueue> voteQueues, string privateKey); //in BaseNodeService
      //Task<bool> VerifyVoteAsync(VoteQueue voteQueue); //in BaseNodeService


   }
}
