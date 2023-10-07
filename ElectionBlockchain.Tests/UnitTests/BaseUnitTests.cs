using ElectionBlockchain.Services.Interfaces;
using ElectionBlockchain.DAL.EF;

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Tests.UnitTests
{
   public abstract class BaseUnitTests
   {
      protected readonly ApplicationDbContext DbContext = null!;
      protected readonly IDatabaseService DatabaseService = null!;
      protected readonly ILeaderService LeaderService = null!;
      protected readonly IVerifierService VerifierService = null!;

      public BaseUnitTests(ApplicationDbContext dbContext, IDatabaseService databaseService,
         ILeaderService leaderService, IVerifierService verifierService)
      {
         DbContext = dbContext;
         DatabaseService = databaseService;
         LeaderService = leaderService;
         VerifierService = verifierService;
      }

   }
}
