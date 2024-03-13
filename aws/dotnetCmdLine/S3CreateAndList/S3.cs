
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

public class S3
{
    internal static Boolean GetBucketName(string[] args, out String bucketName)
    {
        Boolean retval = false;
        bucketName = String.Empty;
        if (args.Length == 0)
        {
            Console.WriteLine("\nNo arguments specified. Will simply list your Amazon S3 buckets." +
              "\nIf you wish to create a bucket, supply a valid, globally unique bucket name.");
            bucketName = String.Empty;
            retval = false;
        }
        else if (args.Length == 1)
        {
            bucketName = args[0];
            retval = true;
        }
        else
        {
            Console.WriteLine("\nToo many arguments specified." +
              "\n\ndotnet_tutorials - A utility to list your Amazon S3 buckets and optionally create a new one." +
              "\n\nUsage: S3CreateAndList [bucket_name]" +
              "\n - bucket_name: A valid, globally unique bucket name." +
              "\n - If bucket_name isn't supplied, this utility simply lists your buckets.");
            Environment.Exit(1);
        }
        return retval;
    }

    //
    // Method to get SSO credentials from the information in the shared config file.
    internal static AWSCredentials LoadSsoCredentials(string profile)
    {
        var chain = new CredentialProfileStoreChain();
        if (!chain.TryGetAWSCredentials(profile, out var credentials))
            throw new Exception($"Failed to find the {profile} profile");
        return credentials;
    }
}
