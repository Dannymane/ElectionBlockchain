using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Model.DataTrasferObjects;
using ElectionBlockchain.Services.ConcreteServices;
using ElectionBlockchain.Services.Interfaces;
using ElectionBlockchain.DAL.EF;
using Newtonsoft.Json;

namespace ElectionBlockchain.Tests.UnitTests
{
    public class DeploymentUnitTests : BaseUnitTests
    {
      public DeploymentUnitTests(ApplicationDbContext dbContext, IDatabaseService databaseService,
         ILeaderService leaderService, IVerifierService verifierService)
         : base(dbContext, databaseService, leaderService, verifierService)
      {

      }

      [Fact]
      public void ResetCitizens()
      {
         string citizensString = DatabaseService.ResetCitizensAndRelatedTables();
         Assert.NotNull(citizensString);
         List<VoteQueue>? citizens = null;
         if (citizensString != null)
            citizens = JsonConvert.DeserializeObject<List<VoteQueue>>(citizensString)!;

         Assert.Equal(24, citizens?.Count());
         Assert.Equal("DOC010", citizens?[0].CitizenDocumentId);
      }
    }
}