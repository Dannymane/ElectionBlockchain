using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Services.ConcreteServices
{
   public abstract class BaseNodeService
   {
      protected readonly ApplicationDbContext DbContext = null!;
      protected readonly IMapper Mapper = null!;
      public static int NodeId { get; set; } = 0;

      public BaseNodeService(ApplicationDbContext dbContext, IMapper mapper)
      {
         DbContext = dbContext;
         Mapper = mapper;
      }

      public async Task<string> SignVotesAsync(List<VoteQueue> voteQueues, string privateKey)
      {

         return " ";
      }

      public async Task<bool> VerifyVoteAsync(VoteQueue vote)
      {
         string CitizenPublicKey = await DbContext.Citizens.Where(c => c.DocumentId == vote.CitizenDocumentId)
            .Select(c => c.PublicKey).FirstOrDefaultAsync();

         if (VerifySignedHash(vote.CitizenDocumentId + vote.CandidateId, vote.CitizenSignature, CitizenPublicKey))
            return true;

      return false;
      }
      public int GetNodeId()
      {
         return NodeId;
      }

      public void SetNodeId(int id)
      {
         NodeId = id;
      }
      //For data is used Encoding.UTF8 to keep the same data not changed
      //For signature is used Convert.ToBase64String because Encoding.UTF8 can't handle it without changing
      public string HashAndSignBytes(string DataToSign, string PrivateKeyString)
      {
         try
         {
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            RSAParameters PrivateKey = JsonConvert.DeserializeObject<RSAParameters>(PrivateKeyString);
            RSAalg.ImportParameters(PrivateKey);

            byte[] DataToSignByte = Encoding.UTF8.GetBytes(DataToSign);

            string signedData = Convert.ToBase64String(RSAalg.SignData(DataToSignByte, SHA256.Create()));

            return signedData;
         }
         catch (CryptographicException e)
         {
            Console.WriteLine(e.Message);
            return null;
         }
      }

      public bool VerifySignedHash(string DataToVerify, string SignedData, string PublicKeyString)
      {
         try
         {
            byte[] DataToVerifyByte = Encoding.UTF8.GetBytes(DataToVerify);
            byte[] SignedDataByte = Convert.FromBase64String(SignedData);

            // Create a new instance of RSACryptoServiceProvider using the
            // key from RSAParameters.
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            RSAParameters PublicKey = JsonConvert.DeserializeObject<RSAParameters>(PublicKeyString);
            RSAalg.ImportParameters(PublicKey);


            return RSAalg.VerifyData(DataToVerifyByte, SHA256.Create(), SignedDataByte);
         }
         catch (CryptographicException e)
         {
            Console.WriteLine(e.Message);
            return false;
         }
      }
   }
}