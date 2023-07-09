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
using System.Net.Http.Json;

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
         List<VoteQueue> votesQueue = await DbContext.VotesQueue.Take(4).ToListAsync();

         if (votesQueue == null) return;

         string LSignature = await SignVotesAsync(votesQueue);

         SignedVotesDto? signedVotesDto = new SignedVotesDto()
         {
            Votes = votesQueue,
            LSignature = LSignature,
            V1Signature = null,
            V2Signature = null
         };
         //asynchronous sending to verifiers
         var signedVotesDtoV1 = SendVotesToVerifier(signedVotesDto, Verifier1Url);
         var signedVotesDtoV2 = SendVotesToVerifier(signedVotesDto, Verifier2Url);

         string? V1Signature = (await signedVotesDtoV1).V1Signature;
         if (!VerifySignedVotes(signedVotesDto.Votes, V1Signature, Verifier1PublicKeyParameter))
         {
            await Task.Delay(1000);
            await CreateAndAddNextBlock(); //start assebmling again
            return;
         }

         string? V2Signature = (await signedVotesDtoV2).V2Signature;
         if (!VerifySignedVotes(signedVotesDto.Votes, V2Signature, Verifier2PublicKeyParameter))
         {
            await Task.Delay(1000);
            await CreateAndAddNextBlock(); //start assebmling again
            return;
         }

         signedVotesDto.V1Signature = V1Signature;
         signedVotesDto.V2Signature = V2Signature;

         var signedVotesDtoV1Confirmation = SendVotesToVerifier(signedVotesDto, Verifier1Url);
         var signedVotesDtoV2Confirmation = SendVotesToVerifier(signedVotesDto, Verifier2Url);

         var signedVotesDtoV1ConfirmationResult = await signedVotesDtoV1Confirmation;
         var signedVotesDtoV2ConfirmationResult = await signedVotesDtoV2Confirmation;

         //For comparing 
         string LSignedVotesString = JsonConvert.SerializeObject(signedVotesDto);
         string V1SignedVotesString = JsonConvert.SerializeObject(signedVotesDtoV1ConfirmationResult);
         string V2SignedVotesString = JsonConvert.SerializeObject(signedVotesDtoV2ConfirmationResult);
    
         if ((LSignedVotesString == V1SignedVotesString) && (LSignedVotesString == V2SignedVotesString))
         {
            await AddBlockToDatabaseAsync(signedVotesDto);
            DbContext.VotesQueue.RemoveRange(votesQueue);
            await DbContext.SaveChangesAsync();
         }
         else
         {
            await Task.Delay(1000);
            await CreateAndAddNextBlock(); //start assebmling again
            return;
         }
      }

      public async Task<SignedVotesDto> SendVotesToVerifier(SignedVotesDto signedVotesDto, string verifierUrl)
      {
         using (HttpClient client = new HttpClient())
         {
            //client.BaseAddress = new Uri(verifierUrl);
            client.BaseAddress = new Uri("https://localhost:44335/");

            string signedVotesDtoString = JsonConvert.SerializeObject(signedVotesDto);
            var content = new StringContent(signedVotesDtoString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("verifier/VotesFromLeader", content);

            string responseBody = await response.Content.ReadAsStringAsync();
            SignedVotesDto? r = JsonConvert.DeserializeObject<SignedVotesDto>(responseBody);
            return r ?? signedVotesDto;
         }
      }



   }
}
