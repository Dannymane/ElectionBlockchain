using System;
using System.Security.Cryptography;
using System.Text;

class Program2
{
   static void Maine()
   {
      try
      {
         ASCIIEncoding ByteConverter = new ASCIIEncoding();

         string dataString = "Data to Sign";

         byte[] originalData = ByteConverter.GetBytes(dataString);
         byte[] signedData;


         RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();


         RSAParameters Key = RSAalg.ExportParameters(true);

         signedData = HashAndSignBytes(originalData, Key);


         if (VerifySignedHash(originalData, signedData, Key))
         {
            Console.WriteLine("The data was verified.");
         }
         else
         {
            Console.WriteLine("The data does not match the signature.");
         }
      }
      catch (ArgumentNullException)
      {
         Console.WriteLine("The data was not signed or verified");
      }
   }
   public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
   {
      RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

      RSAalg.ImportParameters(Key);


      string signedData = Convert.ToBase64String(DataToSign);
        Console.WriteLine(signedData);
        byte[] DataToSignByte = Convert.FromBase64String(signedData);


      return RSAalg.SignData(DataToSignByte, SHA256.Create());
   }

   public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
   {
      RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
      RSAalg.ImportParameters(Key);

      string DataToVerify2 = Convert.ToBase64String(DataToVerify);
      string SignedData2 = Convert.ToBase64String(SignedData);
        byte[] DataToVerify3 = Convert.FromBase64String(DataToVerify2);
      byte[] SignedData3 = Convert.FromBase64String(SignedData2);


      return RSAalg.VerifyData(DataToVerify3, SHA256.Create(), SignedData3);
   }
}