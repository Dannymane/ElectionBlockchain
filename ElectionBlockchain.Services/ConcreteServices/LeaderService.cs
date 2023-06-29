using AutoMapper;
using Microsoft.Extensions.Configuration;
using ElectionBlockchain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionBlockchain.Model.DataModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace ElectionBlockchain.Services.ConcreteServices
{
   public class LeaderService : BaseNodeService, ILeaderService
   {
      public LeaderService(ApplicationDbContext dbContext, IMapper mapper)
         : base(dbContext, mapper)
      {

      }
      public async Task<string> AddVoteQueueToQueueAsync(VoteQueue vote)
      {
         try
         {
            if (await VerifyVoteAsync(vote))
            {
               await DbContext.VotesQueue.AddAsync(vote);
               await DbContext.SaveChangesAsync();
               
               int countVotes = await DbContext.VotesQueue.CountAsync();
               if (countVotes >= 4)
                  CreateAndAddNextBlock();

               return "The vote is verified and added to queue";
            }
            else
               return "The vote is not verified/ already in blockchain/ already in the queue";
         }catch (Exception ex){
            return ex.Message;
         }
      }
      public async Task CreateAndAddNextBlock()
      {
         List<VoteQueue> votes = DbContext.VotesQueue.Take(4).ToList();
         string signature = SignVotesAsync(votes, )



      }
      public Task<bool> CheckVerifierConfirmationAsync(string confirmation)
      {
         throw new NotImplementedException();
      }


   }
}
