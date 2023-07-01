using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using ElectionBlockchain.Model.DataTrasferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
      public static RSAParameters PublicPrivateKeyParameter { get; set; } = default;

      public BaseNodeService(ApplicationDbContext dbContext, IMapper mapper)
      {
         DbContext = dbContext;
         Mapper = mapper;
      }


      public async Task<bool> VerifyVoteAsync(VoteQueue vote)
      {
         if (await DbContext.VotesQueue.FirstOrDefaultAsync(v => v.CitizenDocumentId == vote.CitizenDocumentId) != null)//for classes default is null
            return false;

         var candidate = await DbContext.Candidates.FirstOrDefaultAsync(c => c.Id == vote.CandidateId);
         if (candidate == null)
            return false;

         var citizen = await DbContext.Citizens.FirstOrDefaultAsync(c => c.DocumentId == vote.CitizenDocumentId);
         if(citizen == null) return false;

         if (citizen.VoteId != null) return false;

         string? CitizenPublicKey = citizen.PublicKey;
         if (CitizenPublicKey == null) return false;

         RSAParameters PublicKey = JsonConvert.DeserializeObject<RSAParameters>(CitizenPublicKey);



         if (VerifySignedHash(vote.CitizenDocumentId + vote.CandidateId, vote.CitizenSignature, PublicKey))
            return true;

         return false;
      }

      //For data is used Encoding.UTF8 to keep the same data not changed
      //For signature is used Convert.ToBase64String because Encoding.UTF8 can't handle it without changing
      public async Task<string> SignVotesAsync(List<VoteQueue> voteQueues)
      {
         try
         {
            string DataToSign = JsonConvert.SerializeObject(voteQueues);
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            //RSAParameters privateKey = JsonConvert.DeserializeObject<RSAParameters>(PrivateKeyString);
            RSAalg.ImportParameters(PublicPrivateKeyParameter);

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

      public bool VerifySignedVotes(List<VoteQueue> voteQueues, string SignedData, RSAParameters PublicKey)
      {
         try
         {
            string DataToVerify = JsonConvert.SerializeObject(voteQueues);
            byte[] DataToVerifyByte = Encoding.UTF8.GetBytes(DataToVerify);
            byte[] SignedDataByte = Convert.FromBase64String(SignedData);

            // Create a new instance of RSACryptoServiceProvider using the
            // key from RSAParameters.
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            //RSAParameters PublicKey = JsonConvert.DeserializeObject<RSAParameters>(PublicKeyString);
            RSAalg.ImportParameters(PublicKey);


            return RSAalg.VerifyData(DataToVerifyByte, SHA256.Create(), SignedDataByte);
         }
         catch (CryptographicException e)
         {
            Console.WriteLine(e.Message);
            return false;
         }
      }
      public bool VerifySignedHash(string DataToVerify, string SignedData, RSAParameters PublicKey)
      {
         try
         {
            byte[] DataToVerifyByte = Encoding.UTF8.GetBytes(DataToVerify);
            byte[] SignedDataByte = Convert.FromBase64String(SignedData);

            // Create a new instance of RSACryptoServiceProvider using the
            // key from RSAParameters.
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            //RSAParameters PublicKey = JsonConvert.DeserializeObject<RSAParameters>(PublicKeyString);
            RSAalg.ImportParameters(PublicKey);


            return RSAalg.VerifyData(DataToVerifyByte, SHA256.Create(), SignedDataByte);
         }
         catch (CryptographicException e)
         {
            Console.WriteLine(e.Message);
            return false;
         }
      }
      //For start setup
      public int GetNodeId()
      {
         return NodeId;
      }

      public void SetNodeId(int id)
      {
         NodeId = id;
      }

      public async Task SetPublicPrivateKeyAsync(RSAParametersDto publicPrivateKeyDto)
      {
         var publicPrivateKeyDtoSerialized = JsonConvert.SerializeObject(publicPrivateKeyDto);
         RSAParameters publicPrivateKey = JsonConvert.DeserializeObject<RSAParameters>(publicPrivateKeyDtoSerialized);
         PublicPrivateKeyParameter = publicPrivateKey;
      }

      public async Task<string> GetPublicPrivateKeyAsync()
      {
         return JsonConvert.SerializeObject(PublicPrivateKeyParameter);
      }

      public async Task<string> GenerateNodeKeysAsync()
      {
         var keysParameters = new List<RSAParameters>();

         RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
         keysParameters.Add(RSAalg.ExportParameters(false));
         keysParameters.Add(RSAalg.ExportParameters(true));

         RSAalg = new RSACryptoServiceProvider();
         keysParameters.Add(RSAalg.ExportParameters(false));
         keysParameters.Add(RSAalg.ExportParameters(true));

         RSAalg = new RSACryptoServiceProvider();
         keysParameters.Add(RSAalg.ExportParameters(false));
         keysParameters.Add(RSAalg.ExportParameters(true));

         var keysStrings = JsonConvert.SerializeObject(keysParameters);

         //string PublicKey1 = JsonConvert.SerializeObject(RSAalg.ExportParameters(false));
         //string PrivateKey1 = JsonConvert.SerializeObject(RSAalg.ExportParameters(true));
         //string PublicKey2 = JsonConvert.SerializeObject(RSAalg.ExportParameters(false));
         //string PrivateKey2 = JsonConvert.SerializeObject(RSAalg.ExportParameters(true));
         //string PublicKey3 = JsonConvert.SerializeObject(RSAalg.ExportParameters(false));
         //string PrivateKey3 = JsonConvert.SerializeObject(RSAalg.ExportParameters(true));
         
         //var keys = new List<string> { PublicKey1, PrivateKey1, PublicKey2, PrivateKey2, PublicKey3, PrivateKey3 };

         return keysStrings;

      }

   }
}