using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
      public async void CleanAsync(string table)
      {
         await DbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {table}");
      }
   }
}
