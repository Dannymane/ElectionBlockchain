using ElectionBlockchain.Services.ConcreteServices;
using ElectionBlockchain.DAL.EF;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Tests
{
   public static class Extensions
   {
     

      // Create sample data
      public static async void SeedData(this IServiceCollection services)
      {
         
         var serviceProvider = services.BuildServiceProvider();
         var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
         DatabaseService databaseService = new DatabaseService(dbContext);

         databaseService.ResetCitizensAndRelatedTables();


         await dbContext.SaveChangesAsync();
      }
   }
}
