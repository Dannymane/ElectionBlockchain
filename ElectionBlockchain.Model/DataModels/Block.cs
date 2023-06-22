using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Model.DataModels
{
   public class Block
   {
      public int Id { get; set; }
      public virtual IList<Vote> Votes { get; set; }
      public string LeaderSignature { get; set; }
      public string FirstVerifierSignature { get; set; }
      public string SecondVerifierSignature { get; set; }
      public string PreviousBlockHash { get; set; }

      public string Hash { get; set; }
   }
}
