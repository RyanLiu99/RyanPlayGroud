

using Amazon;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using System.Text;

public static class ProcessStream
{
    public static async Task Process(string streamName)
    {    
        var shardIteratorType = ShardIteratorType.LATEST; // Start reading from the latest record
        var startingSequenceNumber = ""; // Optional: Specify the starting sequence number

        var kinesisClient = new AmazonKinesisClient(RegionEndpoint.USWest2);

        var describeStreamRequest = new DescribeStreamRequest
        {
            StreamName = streamName
        };

        var describeStreamResponse = await kinesisClient.DescribeStreamAsync(describeStreamRequest);
        var shardIds = describeStreamResponse.StreamDescription.Shards.Select(s => s.ShardId).ToList();

        foreach (var shardId in shardIds)
        {
            var getShardIteratorRequest = new GetShardIteratorRequest
            {
                StreamName = streamName,
                ShardId = shardId,
                ShardIteratorType = shardIteratorType
            };

            if (!string.IsNullOrEmpty(startingSequenceNumber))
                getShardIteratorRequest.StartingSequenceNumber = startingSequenceNumber;

            var getShardIteratorResponse = await kinesisClient.GetShardIteratorAsync(getShardIteratorRequest);
            var shardIterator = getShardIteratorResponse.ShardIterator;

            while (!string.IsNullOrEmpty(shardIterator))
            {
                var getRecordsRequest = new GetRecordsRequest
                {
                    ShardIterator = shardIterator
                };

                var getRecordsResponse = await kinesisClient.GetRecordsAsync(getRecordsRequest);

                foreach (var record in getRecordsResponse.Records)
                {
                    // Process the record
                    var data = Encoding.UTF8.GetString(record.Data.ToArray());
                    Console.WriteLine($"Partition key: {record.PartitionKey}, Data: {data}");
                }

                shardIterator = getRecordsResponse.NextShardIterator;
            }
        }
    }
}