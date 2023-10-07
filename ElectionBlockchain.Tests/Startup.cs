using ElectionBlockchain.Services.ConcreteServices;
using ElectionBlockchain.Services.Interfaces;
using ElectionBlockchain.Tests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ElectionBlockchain.Services.Configuration.AutoMapperProfiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionBlockchain.DAL.EF;

using Microsoft.Extensions.Configuration;

namespace ElectionBlockchain.Tests;
public class Startup
{


   public void ConfigureServices(IServiceCollection services)
   {
      services.AddAutoMapper(typeof(MainProfile));

      var connectionString = "Server=(localdb)\\mssqllocaldb;Database=BlockchainDbTest;MultipleActiveResultSets=true";

      services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(connectionString));

      //services.AddEntityFrameworkInMemoryDatabase().AddDbContext<ApplicationDbContext>(options =>
      //   options.UseInMemoryDatabase("InMemoryDb")
      //);

      //services.AddDatabaseDeveloperPageExceptionFilter();
      
      
      services.AddTransient(typeof(ILogger), typeof(Logger<Startup>));
      services.AddScoped<IDatabaseService, DatabaseService>();
      services.AddScoped<ILeaderService, LeaderService>();
      services.AddScoped<IVerifierService, VerifierService>();
      services.BuildServiceProvider();
      //services.SeedData();

      //services.AddControllers();
   }
}
