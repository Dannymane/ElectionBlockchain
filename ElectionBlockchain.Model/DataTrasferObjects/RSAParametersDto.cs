using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Model.DataTrasferObjects
{
   public class RSAParametersDto
   {
      public string D { get; set; }
      public string DP { get; set; }
      public string DQ { get; set; }
      public string Exponent { get; set; }
      public string InverseQ { get; set; }
      public string Modulus { get; set; }
      public string P { get; set; }
      public string Q { get; set; }
   }
}
