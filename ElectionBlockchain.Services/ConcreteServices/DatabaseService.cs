using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
      public  void Clean(string table)
      {
         DbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE [{table}]");
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
         }

         return "Table not found";
      }
   }
}
