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
      public string LSignature { get; set; } = null!;
      public string? V1Signature { get; set; } = null;
      public string? V2Signature { get; set; } = null;
   }
}
 