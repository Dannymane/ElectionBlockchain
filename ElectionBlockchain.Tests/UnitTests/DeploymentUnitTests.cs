using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Model.DataTrasferObjects;
using ElectionBlockchain.Services.ConcreteServices;
using ElectionBlockchain.Services.Interfaces;
using ElectionBlockchain.DAL.EF;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace ElectionBlockchain.Tests.UnitTests
{
    public class DeploymentUnitTests : BaseUnitTests, IDisposable
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

      [Fact]
      public async Task AddCandidatesAsync()
      {
         IEnumerable<Candidate> candidates = new List<Candidate> {
            new Candidate() {Name = "Daniel", Surname = "Yanko" },
            new Candidate() {Name = "John", Surname = "Miller" }
         };

         await DatabaseService.AddCandidatesAsync(candidates);
         List<Candidate> addedCandidates = await DbContext.Candidates.ToListAsync();
         Assert.Equal(2, addedCandidates?.Count());
         Assert.Equal("Daniel", addedCandidates?[0].Name);
         Assert.Equal("John", addedCandidates?[1].Name);

      }

      public void Dispose()
      {
         DatabaseService.Clean("blocks");
         DatabaseService.Clean("votes");
         DatabaseService.Clean("votesqueue");
         DatabaseService.Clean("citizens");
         DatabaseService.Clean("citizenprivatekeys");
         DatabaseService.Clean("candidates");
      }
   }
}