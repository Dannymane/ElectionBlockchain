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
using ElectionBlockchain.Model.DataTrasferObjects;
using Microsoft.AspNetCore.Http;
using System.Reflection.Metadata;

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
         string signature = await SignVotesAsync(votes);

         SignedVotesDto signedVotesDto = new SignedVotesDto()
         {
            Votes = votes,
            Signature = signature
         };
         string signedVotesDtoString = JsonConvert.SerializeObject(signedVotesDto);

         using (HttpClient client = new HttpClient())
         {
            client.BaseAddress = new Uri("https://localhost:44335");

            var content = new StringContent(signedVotesDtoString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("test/votes", content);

            string responseBody = await response.Content.ReadAsStringAsync();
         }


      }
      public Task<bool> CheckVerifierConfirmationAsync(SignedVotesDto confirmation)
      {
         var Votes = confirmation.Votes;
         string Signature = confirmation.Signature;



         if (VerifySignedVotes(Votes, Signature, PublicPrivateKeyParameter))
            return Task.FromResult(true);
         else
            return Task.FromResult(false);
      }

   }
}
