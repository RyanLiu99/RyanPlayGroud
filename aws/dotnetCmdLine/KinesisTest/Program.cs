using System.Text;
using Amazon;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;


var streamName = "RyanTestFromConsoleApp-KinesisTest";
var kinesisClient = new AmazonKinesisClient(RegionEndpoint.USWest2);

var r = new Random(100);

Console.WriteLine($"args.Length {streamName}");
await CreateSteam();
await Task.Delay(9000); //  seems has hard time to use right way
await AddDataToStream();
//await DeleteStream();

await ProcessStream.Process(streamName);

async Task AddDataToStream()
{
    var data = Encoding.UTF8.GetBytes("Hello Kinesis! " + r.Next(0,100)) ; 

    var putRecordRequest = new PutRecordRequest
    {
        StreamName = streamName,
        Data = new MemoryStream(data),
        PartitionKey = "partitionKey" // You can specify any partition key here
    };

    var putRecordResponse = await kinesisClient.PutRecordAsync(putRecordRequest);
 
    Console.WriteLine($"Successfully put record to shard {putRecordResponse.ShardId}");
}

async Task CreateSteam()
{
    if(await DoesStreamExist()) return;
 
    var shardCount = 2; // Specify the number of shards for your stream
    var createStreamRequest = new CreateStreamRequest
    {
        StreamName = streamName,
        ShardCount = shardCount
    };

    await kinesisClient.CreateStreamAsync(createStreamRequest);

    Console.WriteLine($"Stream '{streamName}' created successfully with {shardCount} shard(s).");
}


async Task<bool> DoesStreamExist()
{
    var describeStreamRequest = new DescribeStreamRequest
    {
        StreamName = streamName
    };

    try
    {
        var describeStreamResponse = await kinesisClient.DescribeStreamAsync(describeStreamRequest);
        var streamStatus = describeStreamResponse.StreamDescription.StreamStatus;

        Console.WriteLine($"Stream '{streamName}' exists and its status is '{streamStatus}'.");
        return true;
    }
    catch (ResourceNotFoundException)
    {
        Console.WriteLine($"Stream '{streamName}' does not exist.");
        return false;
    }
}

// delete the Kinesis stream, generated cy Amazon Q
//https://docs.aws.amazon.com/kinesisvideostreams/latest/dg/API_DeleteStream.html



async Task DeleteStream()
{

  // Create the delete stream request
  DeleteStreamRequest deleteStreamRequest = new DeleteStreamRequest
  {
    StreamName = streamName
  };

  // Delete the stream asynchronously 
  await kinesisClient.DeleteStreamAsync(deleteStreamRequest);

  Console.WriteLine($"Stream '{streamName}' deleted successfully");
}
