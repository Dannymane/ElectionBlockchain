using AutoMapper;
using Microsoft.Extensions.Configuration;
using ElectionBlockchain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Services.ConcreteServices
{
   public class LeaderService : BaseNodeService, ILeaderService
   {
      public LeaderService(ApplicationDbContext dbContext, IMapper mapper)
         : base(dbContext, mapper)
      {

      }
      public void AddVoteQueueToQueueAsync(string voteString)
      {
         throw new NotImplementedException();
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
