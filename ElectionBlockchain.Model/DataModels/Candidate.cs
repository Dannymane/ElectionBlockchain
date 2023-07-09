using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElectionBlockchain.Model.DataModels
{
   public class Candidate
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Surname { get; set; }
      [JsonIgnore]
      public virtual IList<Vote>? Votes { get; set; }
   }
}
