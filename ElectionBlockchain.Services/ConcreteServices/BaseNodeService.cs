using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Services.ConcreteServices
{
   public abstract class BaseNodeService
   {
      protected readonly ApplicationDbContext DbContext = null!;
      protected readonly IMapper Mapper = null!;
      public BaseNodeService(ApplicationDbContext dbContext, IMapper mapper)
      {
         DbContext = dbContext;
         Mapper = mapper;

      }
      public async Task<string> SignVotesAsync(List<VoteQueue> voteQueues, string privateKey)
      {
         return " ";
      }

      public async Task<bool> VerifyVoteAsync(VoteQueue voteQueue)
      {
         return true;
      }
   }
   
}