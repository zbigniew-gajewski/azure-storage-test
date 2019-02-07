namespace AzureStorageTest
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class AzureStorageService
    {
        private CloudBlobContainer cloudBlobContainer;

        private BlobContainerPermissions permissions = new BlobContainerPermissions
        {
            PublicAccess = BlobContainerPublicAccessType.Blob
        };

        public AzureStorageService()
        {
            var storageAccount = CloudStorageAccount.Parse(GenerateConnStr());

            // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            // Create a container called 'quickstartblobs' and append a GUID value to it to make the name unique. 
            cloudBlobContainer = cloudBlobClient.GetContainerReference("quickstartblobs" + Guid.NewGuid().ToString());

        }

        public async Task UploadFile()
        {
            await cloudBlobContainer.CreateAsync();

            // Set the permissions so the blobs are public.         
            await cloudBlobContainer.SetPermissionsAsync(permissions);


            // Create a file in your local MyDocuments folder to upload to a blob.
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            for (int i = 1; i <= 10; i++)
            {
                string localFileName = "QuickStart_" + Guid.NewGuid().ToString() + ".txt";
                var sourceFile = Path.Combine(localPath, localFileName);
                // Write text to the file.
                File.WriteAllText(sourceFile, "Hello, World!");

                Console.WriteLine("Temp file = {0}", sourceFile);
                Console.WriteLine("Uploading to Blob storage as blob '{0}'", localFileName);

            
                // Get a reference to the blob address, then upload the file to the blob.
                // Use the value of localFileName for the blob name.
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);
                cloudBlockBlob.Metadata.Add("date", $"{DateTime.Now.ToString()} {DateTime.Now.Millisecond}");
                //cloudBlockBlob.Metadata.Add("month", "01");
                //cloudBlockBlob.Metadata.Add("day", "31");
                //cloudBlockBlob.Metadata.Add("sec", DateTime.Now.Second.ToString());

                await cloudBlockBlob.UploadFromFileAsync(sourceFile);
            }
        }

        public async Task ListBlobs()
        {
            var storageAccount = CloudStorageAccount.Parse(GenerateConnStr());

            //// Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
            //CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            //// Create a container called 'quickstartblobs' and append a GUID value to it to make the name unique. 
            //CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("quickstartblobs" + Guid.NewGuid().ToString());
            //await cloudBlobContainer.CreateAsync();

            // Set the permissions so the blobs are public. 
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await cloudBlobContainer.SetPermissionsAsync(permissions);

            await Task.Run(async () =>
            {

                BlobContinuationToken blobContinuationToken = null;
                do
                {
                    BlobResultSegment results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                    // Get the value of the continuation token returned by the listing call.
                    blobContinuationToken = results.ContinuationToken;
                    foreach (ICloudBlob item in results.Results)
                    {
                        string date = string.Empty;
                        //string year = string.Empty;
                        //string month = string.Empty;
                        //string day = string.Empty;
                        //string sec = string.Empty;

                        // var cloudBlob = item as ICloudBlob;
                        await item.FetchAttributesAsync();
                        if (item != null)
                        {
                            date = item.Metadata.FirstOrDefault(m => m.Key == "date").Value;
                            //month = item.Metadata.FirstOrDefault(m => m.Key == "month").Value;
                            //day = item.Metadata.FirstOrDefault(m => m.Key == "day").Value;
                            //sec = item.Metadata.FirstOrDefault(m => m.Key == "sec").Value;
                        }

                        Console.WriteLine();
                        Console.WriteLine($"Uri                  : {item.Uri}");              
                        Console.WriteLine($" Metadata - date     : {date}");                    
                    }
                } while (blobContinuationToken != null); // Loop while the continuation token is not null. 

              

            });
        }

        static string GenerateConnStr(string ip = "127.0.0.1", int blobport = 10000, int queueport = 10001, int tableport = 10002)
        {
            return $"DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://{ip}:{blobport}/devstoreaccount1;TableEndpoint=http://{ip}:{tableport}/devstoreaccount1;QueueEndpoint=http://{ip}:{queueport}/devstoreaccount1;";
        }
    }
}
