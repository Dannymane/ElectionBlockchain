using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Program
{
   static void Main()
   {
      try
      {
         // Create a UnicodeEncoder to convert between byte array and string.
         ASCIIEncoding ByteConverter = new ASCIIEncoding();

         string dataString = "Data to Sign";

         //// Create byte arrays to hold original, encrypted, and decrypted data.
         //byte[] originalData = ByteConverter.GetBytes(dataString);
         //byte[] signedData;

         // Create a new instance of the RSACryptoServiceProvider class
         // and automatically create a new key-pair.
         RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();


         RSAParameters PublicPrivateKey2 = RSAalg.ExportParameters(true);
         string publicPrivateKeyJson = JsonConvert.SerializeObject(PublicPrivateKey2);

         RSAParameters PublicPrivateKey = JsonConvert.DeserializeObject<RSAParameters>(publicPrivateKeyJson);

         // Hash and sign the data.
         string signedData = HashAndSignBytes(dataString, PublicPrivateKey);

         RSAParameters publicKey = RSAalg.ExportParameters(false);

         // Verify the data and display the result to the
         // console.
         byte[] signedDataByte = Encoding.UTF8.GetBytes(signedData);
         signedData = Encoding.UTF8.GetString(signedDataByte);

         if (VerifySignedHash(dataString, signedData, publicKey))
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
   public static string HashAndSignBytes(string DataToSign, RSAParameters PrivateKey)
   {
      try
      {
         //byte[] DataToSignByte = ByteConverter.GetBytes(DataToSign);
         byte[] DataToSignByte = Encoding.UTF8.GetBytes(DataToSign);

         RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
         RSAalg.ImportParameters(PrivateKey);

         string signedData = Convert.ToBase64String(RSAalg.SignData(DataToSignByte, SHA256.Create()));

         return signedData;
      }
      catch (CryptographicException e)
      {
         Console.WriteLine(e.Message);

         return null;
      }
   }

   public static bool VerifySignedHash(string DataToVerify, string SignedData, RSAParameters Key)
   {
      try
      {
         ASCIIEncoding ByteConverter = new ASCIIEncoding();

         //byte[] DataToVerifyByte = ByteConverter.GetBytes(DataToVerify);
         //byte[] SignedDataByte = ByteConverter.GetBytes(SignedData);
         byte[] DataToVerifyByte = Encoding.UTF8.GetBytes(DataToVerify);
         byte[] SignedDataByte = Convert.FromBase64String(SignedData);

         // Create a new instance of RSACryptoServiceProvider using the
         // key from RSAParameters.
         RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

         RSAalg.ImportParameters(Key);


         return RSAalg.VerifyData(DataToVerifyByte, SHA256.Create(), SignedDataByte);
      }
      catch (CryptographicException e)
      {
         Console.WriteLine(e.Message);

         return false;
      }
   }

      public static void MakeHash()
   {
      using (SHA256 sha256 = SHA256.Create())
      {
         byte[] inputBytes = Convert.FromBase64String("YourDataToHash");
         Console.WriteLine("Input: YourDataToHash");

         byte[] hashBytes = sha256.ComputeHash(inputBytes);
         Console.WriteLine("hashBytes: " + hashBytes); //System.Byte[]
         string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
         Console.WriteLine("hash: " + hash); //9B5082EC0D19AF5D70881543D0D0CF5F09119448ED99194FE075970824A9C23A 
      }
   }
      


   //static void Main(string[] args)
   //{


   //   // Create a SHA256 hash
   //   using (SHA256 sha256 = SHA256.Create())
   //   {
   //      byte[] inputBytes = Convert.FromBase64String("YourDataToHash");
   //         Console.WriteLine("Input: YourDataToHash");

   //      byte[] hashBytes = sha256.ComputeHash(inputBytes);
   //         Console.WriteLine("hashBytes: " + hashBytes); //System.Byte[]
   //      string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
   //         Console.WriteLine("hash: " + hash); //9B5082EC0D19AF5D70881543D0D0CF5F09119448ED99194FE075970824A9C23A
   //   }

   //   // Generate an RSA key pair
   //   RSA rsa = RSA.Create();

   //   // Save the public and private keys
   //   RSAParameters publicKey = rsa.ExportParameters(false);
   //   RSAParameters privateKey = rsa.ExportParameters(true);
   //      Console.WriteLine("RSAParameters publicKey: " + publicKey); //System.Security.Cryptography.RSAParameters
   //      Console.WriteLine("RSAParameters privateKey: " + privateKey); //System.Security.Cryptography.RSAParameters

   //   // Encrypt data using the private key
   //   byte[] dataToEncrypt = Convert.FromBase64String("YourDataToEncrypt");
   //      Console.WriteLine("string dataToEncrypt: YourDataToEncrypt");
   //      Console.WriteLine("byte[]dataToEncrypt: " + dataToEncrypt); //System.Byte[]

   //   RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider();
   //   // Import the private key
   //   rsa1.ImportParameters(privateKey);
   //   // Encrypt the data
   //   byte[] encryptedData = rsa1.Encrypt(dataToEncrypt, true);
   //     Console.WriteLine("byte[] encryptedData: " + encryptedData); //System.Byte[]

   //   // Decrypt data using the public key
   //   // Import the public key
   //   RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();
   //   rsa2.ImportParameters(publicKey);

   //      // Decrypt the data
   //      byte[] decryptedData = rsa2.Decrypt(encryptedData, true);
   //      Console.WriteLine("byte[] decryptedData: " + decryptedData);
   //      string decryptedString = Convert.ToBase64String(decryptedData);
   //      Console.WriteLine("string decryptedString: " + decryptedString);
   //}
}