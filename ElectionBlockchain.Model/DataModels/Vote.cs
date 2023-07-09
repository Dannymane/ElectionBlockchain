using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElectionBlockchain.Model.DataModels
{
   public class Vote
   {
      [JsonIgnore]
      public virtual Citizen Citizen { get; set; }
      public string CitizenDocumentId { get; set; }
      public string CitizenSignature { get; set; }
      [JsonIgnore]
      public virtual Candidate Candidate { get; set; }
      public int CandidateId { get; set; }
      [JsonIgnore]
      public virtual Block Block { get; set; }
      public int BlockId { get; set; }

   }
}
