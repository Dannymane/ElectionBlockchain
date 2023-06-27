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
               return "The vote is verified and added to queue";
            }
            else
               return "The vote is not verified";
         }catch (Exception ex){
            return ex.Message;
         }
      }
      public bool CreateAndAddNextBlock()
      {
         throw new NotImplementedException();
      }
      public Task<bool> CheckVerifierConfirmationAsync(string confirmation)
      {
         throw new NotImplementedException();
      }


   }
}
