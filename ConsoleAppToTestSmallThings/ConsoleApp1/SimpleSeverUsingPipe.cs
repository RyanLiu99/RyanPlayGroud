using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


// Usage
// var server = new SimpleServer();
// await server.RunAsync(tcpClient);


public class SimpleSeverUsingPipe
{
    public async Task RunAsync(TcpClient client)
    {
        var pipe = new Pipe();

        // Start reading and writing concurrently
        var readTask = FillPipeAsync(client, pipe.Writer);
        var writeTask = ReadPipeAsync(pipe.Reader, client);

        await Task.WhenAll(readTask, writeTask);
    }

    private async Task FillPipeAsync(TcpClient client, PipeWriter writer)
    {
        const int minimumBufferSize = 512;

        while (true)
        {
            // Request a buffer from the PipeWriter
            Memory<byte> memory = writer.GetMemory(minimumBufferSize);

            // Read from the network stream
            int bytesRead = await client.GetStream().ReadAsync(memory);

            if (bytesRead == 0)
            {
                break;
            }

            // Tell the PipeWriter how much was read
            writer.Advance(bytesRead);

            // Make the data available to the reader
            var result = await writer.FlushAsync();

            if (result.IsCompleted)
            {
                break;
            }
        }

        // Signal that there's no more data to write
        writer.Complete();
    }

    private async Task ReadPipeAsync(PipeReader reader, TcpClient client)
    {
        while (true)
        {
            // Read data from the PipeReader
            ReadResult result = await reader.ReadAsync();
            System.Buffers.ReadOnlySequence<byte> buffer = result.Buffer;

            // Process the buffer (e.g., converting to a string)
            foreach (ReadOnlyMemory<byte> segment in buffer)
            {
                var data = Encoding.UTF8.GetString(segment.Span);
                Console.WriteLine($"Received: {data}");

                // Echo the data back to the client
                var response = Encoding.UTF8.GetBytes($"Echo: {data}");
                await client.GetStream().WriteAsync(response);
            }

            // Indicate how much of the buffer has been consumed
            reader.AdvanceTo(buffer.End);

            // Stop reading if there's no more data
            if (result.IsCompleted)
            {
                break;
            }
        }

        reader.Complete();
    }
}

