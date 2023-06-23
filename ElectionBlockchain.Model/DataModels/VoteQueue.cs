using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Model.DataModels
{
   //VoteQueue has information for verification, no foreign keys to other tables
   //constraints and foreign keys will be applied to Vote(from VoteQueue)
   public class VoteQueue
   {
      public string CitizenDocumentId { get; set; }
      public string CitizenSignature { get; set; }
      public int CandidateId { get; set; }

   }
}