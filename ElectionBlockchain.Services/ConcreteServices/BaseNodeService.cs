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
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Services.ConcreteServices
{
   public abstract class BaseNodeService
   {
      protected readonly ApplicationDbContext DbContext = null!;
      protected readonly IMapper Mapper = null!;
      public static int NodeId { get; set; } = 0;
      public static string LeaderUrl { get; set; } = null!; // http://172.28.190.154:80/
      public static string Verifier1Url { get; set; } = null!;
      public static string Verifier2Url { get; set; } = null!;
      public static RSAParameters PublicPrivateKeyParameter { get; set; } = default;
      public static RSAParameters LeaderPublicKeyParameter { get; set; } = default;
      public static RSAParameters Verifier1PublicKeyParameter { get; set; } = default;
      public static RSAParameters Verifier2PublicKeyParameter { get; set; } = default;

      public BaseNodeService(ApplicationDbContext dbContext, IMapper mapper)
      {
         DbContext = dbContext;
         Mapper = mapper;
      }


      public async Task<bool> VerifyVoteAsync(VoteQueue vote)
      {
         //if (await DbContext.VotesQueue.FirstOrDefaultAsync(v => v.CitizenDocumentId == vote.CitizenDocumentId) != null)//for classes default is null
         //   return false;

         var candidate = await DbContext.Candidates.FirstOrDefaultAsync(c => c.Id == vote.CandidateId);
         if (candidate == null)
            return false;

         var citizen = await DbContext.Citizens.FirstOrDefaultAsync(c => c.DocumentId == vote.CitizenDocumentId);
         if(citizen == null) return false;

         if (citizen.Vote != null) return false;

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

      public bool VerifySignedVotes(List<VoteQueue> voteQueues, string? SignedData, RSAParameters PublicKey)
      {
         try
         {
            if (SignedData == null) return false;

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
      public async Task SetPublicKeyAsync(RSAParametersDto publicKeyDto, string nodeRole)
      {
         var publicKeyDtoSerialized = JsonConvert.SerializeObject(publicKeyDto);
         RSAParameters publicKey = JsonConvert.DeserializeObject<RSAParameters>(publicKeyDtoSerialized);
         switch (nodeRole.ToLower())
         {
            case "leader":
               LeaderPublicKeyParameter = publicKey;
               break;
            case "verifier1":
               Verifier1PublicKeyParameter = publicKey;
               break;
            case "verifier2":
               Verifier2PublicKeyParameter = publicKey;
               break;
         }
         
      }

      public async Task<string> GetPublicKeyAsync(string nodeRole)
      {
         switch (nodeRole.ToLower())
         {
            case "leader":
               return JsonConvert.SerializeObject(LeaderPublicKeyParameter);
            case "verifier1":
               return JsonConvert.SerializeObject(Verifier1PublicKeyParameter);
            case "verifier2":
               return JsonConvert.SerializeObject(Verifier2PublicKeyParameter);
            default:
               return JsonConvert.SerializeObject("Bad node role request"); ;
         }
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
      public async Task SetNodeUrlAsync(string nodeRole, string ip)
      {
         switch(nodeRole.ToLower())
         {
            case "verifier1":
               Verifier1Url = $"http://{ip}:80/";
               break;
            case "verifier2":
               Verifier2Url = $"http://{ip}:80/";
               break;
            case "leader":
               LeaderUrl = $"http://{ip}:80/";
               break;
            default:
               break;
         }
      }

      public async Task<string> GetNodeUrlAsync(string nodeRole)
      {
         switch (nodeRole.ToLower())
         {
            case "verifier1":
               return JsonConvert.SerializeObject(Verifier1Url);
            case "verifier2":
               return JsonConvert.SerializeObject(Verifier2Url);
            case "leader":
               return JsonConvert.SerializeObject(LeaderUrl);
            default:
               return JsonConvert.SerializeObject("Bad node role in ulr");
         }
      }

      public async Task<string> GenerateBlockHash(Block block)
      {
         using (SHA256 sha256 = SHA256.Create())
         {
            block.Hash = null;

            var serializedLastBlock = JsonConvert.SerializeObject(block);
            byte[] inputBytes = Encoding.UTF8.GetBytes(serializedLastBlock);

            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            return hash;
         }
      }
      public async Task AddBlockToDatabaseAsync(SignedVotesDto signedVotesDto)
      {
         var votes = signedVotesDto.Votes.Select(voteQueue => Mapper.Map<Vote>(voteQueue)).ToList();
         Block? lastBlock = await DbContext.Blocks.OrderByDescending(b => b.Id).FirstOrDefaultAsync();
         if (lastBlock == null)
         {
            lastBlock = new Block()
            {
               Id = 0,
               Votes = null,
               LeaderSignature = null,
               FirstVerifierSignature = null,
               SecondVerifierSignature = null,
               Hash = "0F7382E7F78F4B3199547ECEBCCD348B88D49F018CE766AE626EA00A9C4B73D5",
               PreviousBlockHash = "0246051F5B47D3CDAFF1A58FE4C3DE589D6DCEFDC3AA8CF8EF0B110A2D7F41D4"
               /* PreviousBlockHash generated from
                * Id = -1,
                  Votes = null,
                  LeaderSignature = null,
                  FirstVerifierSignature = null,
                  SecondVerifierSignature = null,
                  Hash = null,
                  PreviousBlockHash = null
                */
            };
         }


         foreach (var v in votes)
         {
            v.BlockId = lastBlock.Id + 1;
         }

         Block block = new Block()
         {
            Votes = votes,
            LeaderSignature = signedVotesDto.LSignature,
            FirstVerifierSignature = signedVotesDto.V1Signature,
            SecondVerifierSignature = signedVotesDto.V2Signature,
            PreviousBlockHash = lastBlock.Hash,
            Hash = null
         };

         string hash = await GenerateBlockHash(block);
         block.Hash = hash;


         await DbContext.Blocks.AddAsync(block);
         if(NodeId == 3)
            DbContext.VotesQueue.RemoveRange(signedVotesDto.Votes);  

         await DbContext.SaveChangesAsync();
      }

   }
}