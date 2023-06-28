using ElectionBlockchain.Model.DataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Services.Interfaces
{
   public interface IDatabaseService
   {
      Task<Node> AddNodeAsync(Node node);
      int Clean(string table);
      Task<string> GetTableAsync(string table);
      string ResetCitizensAndRelatedTables();
   }
}
