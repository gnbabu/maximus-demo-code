using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaFileUpload
{
    public class Function
    {
        IAmazonS3 S3Client { get; set; }
        private string AWSS3BucketName = string.Empty;
        private string AWSArchiveS3BucketName = string.Empty;
        private string FileName = string.Empty;

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            S3Client = new AmazonS3Client();
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        /// <param name="s3Client"></param>
        public Function(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }

        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(S3Event evnt, ILambdaContext context)
        {
            //Event should never be null, if the file input triggered the Lambda function.
            if (evnt != null)
            {
                AWSS3BucketName = evnt.Records[0].S3.Bucket.Name;
                AWSArchiveS3BucketName = evnt.Records[0].S3.Bucket.Name.Replace("promote", "archive", StringComparison.OrdinalIgnoreCase);
                FileName = evnt.Records[0].S3.Object.Key;
                await ProcessFiles(FileName);
            }
            else
            {
                Console.WriteLine($" Couldn't read the bucket name from the functional handler.");
            }
        }

        /// <summary>
        /// Process files in S3 Bucket
        /// </summary>
        /// <returns></returns>
        private async Task ProcessFiles(string controlFileName)
        {
            Console.WriteLine("Fetching files from the AWS S3 bucket");
            List<string> s3Keys = FetchS3ObjectsAsync(AWSS3BucketName, string.Empty).Result;
            List<string> s3MainFiles = s3Keys.Where(name => !name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)).ToList();
            s3MainFiles = s3MainFiles.Where(name => !name.EndsWith("--CFAT--MMISODJFS.zip", StringComparison.OrdinalIgnoreCase)).ToList();

            //Lambda function works based on placing xml file
            string controlFileNoExtension = Path.GetFileNameWithoutExtension(controlFileName);

            string actualFileNameNoExtension = controlFileNoExtension.Replace("--CF--", "--AT--");

            //Only control file and the actual file has the matching names, so it is pretty safe to assume and pattern match the file name.
            string zippableFileName = s3MainFiles.Where(name => name.StartsWith(actualFileNameNoExtension, StringComparison.OrdinalIgnoreCase)).First();

            //These files were previously zipped, so safe to delete. Don't delete the latest zipped file!
            List<string> deletableFileNames = s3MainFiles.Where(name => !name.Equals(zippableFileName)).ToList();

            string zipFileName = Path.GetFileNameWithoutExtension(zippableFileName).Replace("--AT--", "--CFAT--") + ".zip";
            Console.WriteLine($"Creating a zip file for {zippableFileName} ");
            List<string> lstZipFile = FetchS3ObjectsAsync(AWSArchiveS3BucketName, zipFileName).Result;

            if (lstZipFile.Count == 0)
            {
                await CreateZipFile(zippableFileName);

                //Delete previously uploaded files. Retain the current file set.
                foreach (string file in deletableFileNames)
                {
                    DeleteFile(file);
                }
            }
        }

        private async Task CreateZipFile(string fileName)
        {
            Console.WriteLine("Creating a zip file in Promote Bucket.");
            string actualFileName = Path.GetFileNameWithoutExtension(fileName);
            string controlFileName = actualFileName.Replace("--AT--", "--CF--") + ".xml";
            Console.WriteLine("actualFileName : {0} and controlFileName : {1} ", actualFileName, controlFileName);

            using var zipStream = new MemoryStream();
            using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                //Add main file
                await CreateZipEntry(zip, fileName);
                //Add control file
                await CreateZipEntry(zip, controlFileName);
                zip.Dispose();
            }
            zipStream.Seek(0, SeekOrigin.Begin);
            var transferUtility = new TransferUtility(S3Client);
            string zipFileName = actualFileName.Replace("--AT--", "--CFAT--");
            transferUtility.Upload(zipStream, AWSS3BucketName, $"{zipFileName}.zip");
        }

        private async Task CreateZipEntry(ZipArchive zip, string fileName)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = AWSS3BucketName,
                    Key = fileName
                };
                using GetObjectResponse response = await S3Client.GetObjectAsync(request);
                using Stream responseStream = response.ResponseStream;
                var zipItem = zip.CreateEntry(fileName);
                using (var entryStream = zipItem.Open())
                    await responseStream.CopyToAsync(entryStream);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Cannot able to create zip file {fileName} - {ex.Message}");
            }
        }

        private bool DeleteFile(string fileName)
        {
            string controlFileName = Path.GetFileNameWithoutExtension(fileName).Replace("--AT--", "--CF--") + ".xml";
            string zipFileName = Path.GetFileNameWithoutExtension(fileName).Replace("--AT--", "--CFAT--") + ".zip";

            List<string> lstZipFile = FetchS3ObjectsAsync(AWSS3BucketName, zipFileName).Result;

            if (lstZipFile.Count > 0)
            {
                S3Client.DeleteObjectAsync(AWSS3BucketName, controlFileName).Wait();
                S3Client.DeleteObjectAsync(AWSS3BucketName, fileName).Wait();
                return true;
            }
            return false;
        }

        private void PrintMemoryStreamContents(MemoryStream memoryStream)
        {
            Console.WriteLine("In PrintMemoryStreamContents:");
            Console.WriteLine(Encoding.ASCII.GetString(memoryStream.ToArray()));
        }

        /// <summary>
        /// Fetch all s3 objects in a given AWS S3 bucket.
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<List<string>> FetchS3ObjectsAsync(string bucket, string key)
        {
            Console.WriteLine(AWSS3BucketName);
            ListObjectsResponse response = null;
            List<string> s3Objects = new List<string>();
            ListObjectsRequest request = new ListObjectsRequest();

            if (key == string.Empty)
            {
                request.BucketName = bucket;
            }
            else
            {
                request.BucketName = bucket;
                request.Prefix = key;
            }

            do
            {
                try
                {
                    response = await S3Client.ListObjectsAsync(request);

                    foreach (S3Object obj in response.S3Objects)
                    {
                        s3Objects.Add(obj.Key);
                    }
                    if (response.IsTruncated ?? false)
                    {
                        request.Marker = response.NextMarker;
                    }
                    else
                    {
                        request = null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"In FetchS3ObjectsAsync :{ex.Message}");
                }
            } while (request != null);
            return s3Objects;
        }
    }
}