using Google.Cloud.Storage.V1;
using System;
using System.IO;

namespace HelloWorld
{
    class Storage
    {
        // Instantiates a client.
        static StorageClient storageClient = StorageClient.Create();

        public static void DownloadObject(string bucketName, string objectName,
        string localPath = null)
        {
            localPath = localPath ?? Path.GetFileName(objectName);
            using (var outputFile = File.OpenWrite(localPath))
            {
                storageClient.DownloadObject(bucketName, objectName, outputFile);
            }
            Console.WriteLine($"downloaded {objectName} to {localPath}.");
        }
    }
}