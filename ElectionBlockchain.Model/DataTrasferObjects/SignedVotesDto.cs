using ElectionBlockchain.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Model.DataTrasferObjects
{
   public class SignedVotesDto
   {
      public List<VoteQueue> Votes { get; set; } = null!;
      public string Signature { get; set; } = null!;
   }
}
 