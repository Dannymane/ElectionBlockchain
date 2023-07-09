using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ElectionBlockchain.Model.DataModels
{
   public class Citizen
   {
      public string DocumentId { get; set; }
      public string PublicKey { get; set; }
      public virtual Vote? Vote { get; set; }
      public int? VoteId { get; set; }
   }
}
