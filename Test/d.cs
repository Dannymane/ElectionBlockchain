using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

class RSACSPSample
{
   public static async Task wait2secAndNokAsync()
   {
      await Task.Run(() => Thread.Sleep(2000));
      await Console.Out.WriteLineAsync("Nok");
   }
   public static async Task wait2secAsync()
   {
      await Task.Run(() => Thread.Sleep(2000));
   }
   public static async Task wait1secAsync()
   {
      await Task.Run(() => Thread.Sleep(1000));
   }
   public static async Task<int> wait3secAsync()
   {
      Thread.Sleep(5000); //block the whole program
      wait2secAndNokAsync();                                       //starts simultaneously
      await Task.Run(() => Thread.Sleep(3000));                    //starts simultaneously   
      Thread.Sleep(3000); //does not block the whole program

      return 3; 
   }
   /*
   async method runs asynchronously simultaneously with other async methods
   async method runs synchronously with 'await'
   if async mehtod has synchronous method, it will run synchronously,
       but the other async methods will run asynchronously.Also async methods wait for synchronous methods to finish:
   
    1 2 and middle sync start at the same time
    3 4 and last sync start after middle sync finish
   If Main method not awaited, the middle will block program, but the last will not block program

    1 async method
    2 async method
    middle sync method
    3 async method
    4 async method
    last sync method

   Why middle sync block the whole program? The main thread startS execute synchronously non-awaited async method
   iF the first method await async -> the main thread pass tasks to other threads
   if the first method is sync -> the main thread get stuck until he finish sync method (and just after finishing 
   it checks again if the next method is async or sync. If sync - it's blocked again.)
   if the first method non-awaited async -> it doesn't make an impact, because:

    1 async method
    2 async method
    middle sync method

   run simultaneously and it is the same as 'if the first method is sync'
   */


   public static async Task Test1Async()
   {
      var watch = new System.Diagnostics.Stopwatch();
      watch.Start();
      Task<int> a = wait3secAsync();
      //Console.Out.WriteLineAsync("ok");
      //await wait1secAsync();
      //wait2secAsync();
      watch.Stop();
      Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
      Console.WriteLine(a.Result);
   }

   static async Task Main()
   {
      await Test1Async();
      Console.ReadLine();

      //await MakeTeaAsync();
      //Console.ReadLine();
   }

   public static async Task MakeTeaAsync()
   {

      var water = BoilWaterAsync();

      Console.WriteLine("Take the cups out");
      Console.WriteLine("Put tea in cups");
      //var water = await boilingWater;
      Console.WriteLine($"Pour {await water} in cups");

      //Console.WriteLine("Thread id: " + Thread.CurrentThread.ManagedThreadId); //1

      //var boilingWater = BoilWaterAsync();

      //Console.WriteLine("Take the cups out");
      //Console.WriteLine("Put tea in cups");
      //Console.WriteLine("Thread id: " + Thread.CurrentThread.ManagedThreadId); //1
      //var water = await boilingWater;
      //Console.WriteLine("Thread id: " + Thread.CurrentThread.ManagedThreadId); //6
      //Console.WriteLine($"Pour {water} in cups");
   }

   public static async Task<string> BoilWaterAsync()
   {
      await Task.Delay(1); //From now BoilWaterAsync do another thread, because the first method is await async
      //Console.WriteLine("Thread id: " + Thread.CurrentThread.ManagedThreadId); //4

      //DelaySecondAsync(); //Thread 7
      //DelaySecondAsync(); //Thread 6
      //await Task.Delay(2000); 
      //Console.WriteLine("Thread id: " + Thread.CurrentThread.ManagedThreadId); //Thread 6

      //Thread.Sleep(2000);
      int b = 0;
      for (int i = 0; i < 1000_000_000; i++)
      {
         b += i;
      }

      //Console.WriteLine(b);
      //Console.WriteLine("Thread id: " + Thread.CurrentThread.ManagedThreadId); //Thread 6
      Console.WriteLine("The kettle has turned on ");
      await Task.Delay(5000);
      //Console.WriteLine("Thread id: " + Thread.CurrentThread.ManagedThreadId); //Thread 6

      Console.WriteLine("Kettle has turned off");

      return "water";
   }

   public static async void DelaySecondAsync()
   {
      await Task.Delay(1000);
      Console.WriteLine("Delayed 1 second");
      Console.WriteLine("Thread id: " + Thread.CurrentThread.ManagedThreadId);
   }
   



   public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
   {
      try
      {
         // Create a new instance of RSACryptoServiceProvider using the
         // key from RSAParameters.
         RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

         RSAalg.ImportParameters(Key);

         // Hash and sign the data. Pass a new instance of SHA256
         // to specify the hashing algorithm.
         return RSAalg.SignData(DataToSign, SHA256.Create());
      }
      catch (CryptographicException e)
      {
         Console.WriteLine(e.Message);

         return null;
      }
   }

   public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
   {
      try
      {
         // Create a new instance of RSACryptoServiceProvider using the
         // key from RSAParameters.
         RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

         RSAalg.ImportParameters(Key);

         // Verify the data using the signature.  Pass a new instance of SHA256
         // to specify the hashing algorithm.
         return RSAalg.VerifyData(DataToVerify, SHA256.Create(), SignedData);
      }
      catch (CryptographicException e)
      {
         Console.WriteLine(e.Message);

         return false;
      }
   }
}
