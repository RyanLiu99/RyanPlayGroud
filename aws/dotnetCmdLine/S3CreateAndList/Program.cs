// See https://aka.ms/new-console-template for more information
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecurityToken;

Console.WriteLine("Hello, World!");

// Get SSO credentials from the information in the shared config file.
// For this tutorial, the information is in the [default] profile.
var ssoCreds = S3.LoadSsoCredentials("ryanadmin_accesskey_for_local_code");

// Display the caller's identity.
var ssoProfileClient = new AmazonSecurityTokenServiceClient(ssoCreds);
Console.WriteLine($"\nSSO Profile:\n {await ssoProfileClient.GetCallerIdentityArn()}");

// Create the S3 client is by using the SSO credentials obtained earlier.
var s3Client = new AmazonS3Client(ssoCreds);

// Parse the command line arguments for the bucket name.
if (S3.GetBucketName(args, out String bucketName))
{
    // If a bucket name was supplied, create the bucket.
    // Call the API method directly
    try
    {
        Console.WriteLine($"\nCreating bucket {bucketName}...");
        var createResponse = await s3Client.PutBucketAsync(bucketName);
        Console.WriteLine($"Result: {createResponse.HttpStatusCode.ToString()}");
    }
    catch (Exception e)
    {
        Console.WriteLine("Caught exception when creating a bucket:");
        Console.WriteLine(e.Message);
    }
}


// Display a list of the account's S3 buckets.
Console.WriteLine("\nGetting a list of your buckets...");
var listResponse = await s3Client.ListBucketsAsync();
Console.WriteLine($"Number of buckets: {listResponse.Buckets.Count}");
foreach (S3Bucket b in listResponse.Buckets)
{
    Console.WriteLine(b.BucketName);
}
Console.WriteLine();



