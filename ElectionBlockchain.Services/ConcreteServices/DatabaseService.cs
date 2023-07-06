using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Model.DataTrasferObjects;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ElectionBlockchain.Services.ConcreteServices
{
   public class DatabaseService : IDatabaseService
   {
      protected readonly ApplicationDbContext DbContext = null!;
      public DatabaseService(ApplicationDbContext dbContext)
      {
         DbContext = dbContext;
      }
      public async Task<Node> AddNodeAsync(Node node)
      {
         if (node == null)
            return null;

         if (string.IsNullOrEmpty(node.PublicKey))
            return null;

         if (string.IsNullOrEmpty(node.IpAddress))
            return null;

         var n = await DbContext.Nodes.AddAsync(node);
         await DbContext.SaveChangesAsync();
         return n.Entity;
      }
      public int Clean(string table)
      {
         Task<int> afectedRows = DbContext.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}]");
         DbContext.SaveChanges();
         return afectedRows.Result;
      }

      public async Task<string> GetTableAsync(string table)
      {
         table = table.ToLower();
         //that is sad that EF Core does not support smth like DbContext.Database.SqlQuery<string>($"SELECT * FROM {table}").ToList();
         switch (table)
         {
            case "nodes":
               return JsonSerializer.Serialize<IList<Node>>(await DbContext.Nodes.ToListAsync());
            case "blocks":
               return JsonSerializer.Serialize<IList<Block>>(await DbContext.Blocks.ToListAsync());
            case "citizens":
               return JsonSerializer.Serialize<IList<Citizen>>(await DbContext.Citizens.ToListAsync());
            case "votes":
               return JsonSerializer.Serialize<IList<Vote>>(await DbContext.Votes.ToListAsync());
            case "votesqueue":
               return JsonSerializer.Serialize<IList<VoteQueue>>(await DbContext.VotesQueue.ToListAsync());
            case "candidates":
               return JsonSerializer.Serialize<IList<Candidate>>(await DbContext.Candidates.ToListAsync());
            case "citizenprivatekeys":
               return JsonSerializer.Serialize<IList<CitizenPrivateKey>>(await DbContext.CitizenPrivateKeys.ToListAsync());
         }

         return "Table not found";
      }

      public string ResetCitizensAndRelatedTables() //returns signed votes (vote for candidate 1 and vote for candidate 2)
      {
         Clean("blocks");
         Clean("votes");
         Clean("votesqueue");
         Clean("citizens");
         Clean("citizenprivatekeys");

         List<VoteQueue> voteQueueList = new List<VoteQueue>();
         Citizen citizen;
         CitizenPrivateKey citizenPrivateKey;
         VoteQueue vote;
         int CandidateId1, CandidateId2;
         string signedVote1, signedVote2;
         //every time it runs, it generates different keys
         for (int i = 10; i < 22; i++)
         {
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            RSAParameters PublicK = RSAalg.ExportParameters(false);
            RSAParameters PrivateK = RSAalg.ExportParameters(true);

            string PublicKString = JsonConvert.SerializeObject(PublicK);
            string PrivateKString = JsonConvert.SerializeObject(PrivateK);


            citizen = new Citizen()
            {
               PublicKey = PublicKString,
               DocumentId = "DOC0" + i.ToString()
            };
            DbContext.Citizens.Add(citizen);

            
            citizenPrivateKey = new CitizenPrivateKey()
            {
               DocumentId = citizen.DocumentId,
               PrivateKey = PrivateKString
            };
            DbContext.CitizenPrivateKeys.Add(citizenPrivateKey);

            CandidateId1 = 1;
            CandidateId2 = 2;

            //must be Encoding.UTF8.GetBytes to do not change the data
            byte[] DataToSignByte1 = Encoding.UTF8.GetBytes(citizen.DocumentId+CandidateId1.ToString()); 
            signedVote1 = Convert.ToBase64String(RSAalg.SignData(DataToSignByte1, SHA256.Create())); //must be Convert.ToBase64String

            byte[] DataToSignByte2 = Encoding.UTF8.GetBytes(citizen.DocumentId + CandidateId2.ToString());
            signedVote2 = Convert.ToBase64String(RSAalg.SignData(DataToSignByte2, SHA256.Create()));

            vote = new VoteQueue()
            {
               CitizenDocumentId = citizen.DocumentId,
               CandidateId = CandidateId1,
               CitizenSignature = signedVote1
            };
            voteQueueList.Add(vote);

            vote = new VoteQueue()
            {
               CitizenDocumentId = citizen.DocumentId,
               CandidateId = CandidateId2,
               CitizenSignature = signedVote2
            };
            voteQueueList.Add(vote);
         }
         DbContext.SaveChanges();
         return JsonSerializer.Serialize<IList<VoteQueue>>(voteQueueList);

      }

      public async Task<string> WriteCitizensToNode(string nodeIp)
      {
         using (HttpClient client = new HttpClient())
         {
            client.BaseAddress = new Uri($"http://{nodeIp}:80/");
            var citizensEntities = DbContext.Citizens.ToList();
            string citizensString = JsonConvert.SerializeObject(citizensEntities);
            var content = new StringContent(citizensString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("database/WriteCitizens", content);
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
         }
      }

      public async Task WriteCitizensToDatabase(IEnumerable<Citizen> citizens)
      {
         Clean("blocks");
         Clean("votes");
         Clean("votesqueue");
         Clean("citizens");

         foreach (var citizen in citizens)
         {
            DbContext.Citizens.Add(citizen);
         }
         await DbContext.SaveChangesAsync();
      }

      public async Task AddCandidatesAsync(IEnumerable<Candidate> candidates)
      {
         foreach (var candidate in candidates)
         {
            DbContext.Candidates.Add(candidate);
         }
         await DbContext.SaveChangesAsync();
      }
   }
}
