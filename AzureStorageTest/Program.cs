using System;

namespace AzureStorageTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Uploading started!");
            var azureStorageService = new AzureStorageService();
            azureStorageService.UploadFile().Wait();
            Console.WriteLine("Uploading finished");

            Console.WriteLine();

            Console.WriteLine("List blobs in container.");
            azureStorageService.ListBlobs().Wait();
            Console.WriteLine("Listing blobs finished.");

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();


        }
    }
}
