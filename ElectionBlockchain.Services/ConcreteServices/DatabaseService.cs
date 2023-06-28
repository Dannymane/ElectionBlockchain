using AutoMapper;
using ElectionBlockchain.Model.DataModels;
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

      public string ResetCitizensAndRelatedTables()
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
         int CandidateId;
         string signedVote;
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

            CandidateId = new Random().Next(1, 3);//1 or 2
            //must be Encoding.UTF8.GetBytes to do not change the data
            byte[] DataToSignByte = Encoding.UTF8.GetBytes(citizen.DocumentId+CandidateId.ToString()); 
            signedVote = Convert.ToBase64String(RSAalg.SignData(DataToSignByte, SHA256.Create())); //must be Convert.ToBase64String

            vote = new VoteQueue()
            {
               CitizenDocumentId = citizen.DocumentId,
               CandidateId = CandidateId,
               CitizenSignature = signedVote
            };
            voteQueueList.Add(vote);
         }
         DbContext.SaveChanges();
         return JsonSerializer.Serialize<IList<VoteQueue>>(voteQueueList);
      }
   }
}
