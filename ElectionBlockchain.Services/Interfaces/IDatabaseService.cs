using ElectionBlockchain.Model.DataModels;
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
      void CleanAsync(string table);
      Task<string> GetTableAsync(string table);
   }
}
