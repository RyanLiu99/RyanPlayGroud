
// Class to read the caller's identity.
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;

public static class Extensions
{
    public static async Task<string> GetCallerIdentityArn(this IAmazonSecurityTokenService stsClient)
    {
        var response = await stsClient.GetCallerIdentityAsync(new GetCallerIdentityRequest());
        return response.Arn;
    }
}