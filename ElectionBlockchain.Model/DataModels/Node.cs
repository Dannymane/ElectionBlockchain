using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Model.DataModels
{
   public class Node
   {
      public int? Id { get; set; }
      public string? IpAddress { get; set; }
      public string? PublicKey { get; set; }
   }
}
