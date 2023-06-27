using AutoMapper;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Services.ConcreteServices
{
   public class VerifierService : BaseNodeService, IVerifierService
   {
      public VerifierService(ApplicationDbContext dbContext, IMapper mapper)
         : base(dbContext, mapper)
      {
      }

      public void SendConfirmationToLeader(string confirmation)
      {
         throw new NotImplementedException();
      }

      public Task<bool> VerifyVotesAsync(string requestBodyWithVotes)
      {
         throw new NotImplementedException();
      }
   }
}
