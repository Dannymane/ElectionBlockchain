using AutoMapper;
using ElectionBlockchain.Model.DataTrasferObjects;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
      public async Task<StringContent> ReceiveVotes(SignedVotesDto confirmation)
      {
         var votesQueue = confirmation.Votes;
         string LSignature = confirmation.LSignature;

         string signedVotesDtoString = JsonConvert.SerializeObject(confirmation);
         StringContent content = new StringContent(signedVotesDtoString, Encoding.UTF8, "application/json");

         if (!VerifySignedVotes(votesQueue, LSignature, LeaderPublicKeyParameter))
            return content;

         if (VerifySignedVotes(votesQueue, confirmation.V1Signature, Verifier1PublicKeyParameter) &&
               VerifySignedVotes(votesQueue, confirmation.V2Signature, Verifier2PublicKeyParameter))
         {
            await AddBlockToDatabaseAsync(confirmation);
            return content;
         }

         string Signature = await SignVotesAsync(votesQueue);

         if(NodeId == 1) 
            confirmation.V1Signature = Signature;

         if(NodeId == 2)
            confirmation.V2Signature = Signature;

         signedVotesDtoString = JsonConvert.SerializeObject(confirmation);
         content = new StringContent(signedVotesDtoString, Encoding.UTF8, "application/json");

         return content;

      }

   }
}
