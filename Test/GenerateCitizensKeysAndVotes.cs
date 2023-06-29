using ElectionBlockchain.Model.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
   public class GenerateCitizensKeysAndVotes
   {
      public static void Maine(string[] args)
      {
         Citizen citizen;
         VoteQueue vote;
         //i checked, every time i run this, it generates different keys
         for (int i = 10; i < 22; i++)
         {
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            RSAParameters PublicK = RSAalg.ExportParameters(false);
            RSAParameters PrivateK = RSAalg.ExportParameters(true);

            string PublicKString = JsonConvert.SerializeObject(PublicK);
            string PrivateKString = JsonConvert.SerializeObject(PrivateK);


            citizen = new Citizen()
            {
               PublicKey = PublicKString,
               DocumentId = "DOC0" + i.ToString() 
            };

            vote = new VoteQueue()
            {
               CitizenDocumentId = citizen.DocumentId,
               CandidateId = 1,
               CitizenSignature = "D"
            };
         }
      }
   }
}
