using System;
using System.Collections.Generic;
using Aerospike.Client;
namespace AeroSpikeXUnit;

public class CopyBins
{
    [Fact]
    public void CopyPayConBins()
    {
        // Configure the _client
        ClientPolicy clientPolicy = new ClientPolicy();
        using AerospikeClient client = new AerospikeClient(clientPolicy, "127.0.0.1", 3000);

        // Define namespaces and sets
        string namespaceName = "test";
        string originalSet = "UsageRepo";
        string payRepoSet = "PayRepo2";
        string conRepoSet = "ConRepo2";

        // Define the bins to copy
        string[] payBins = { "key", "PAYc", "PAYs", "PAYcc", "PAYsc"};
        string[] conBins = { "key", "CONc", "CONs", "CONcc", "CONsc" };

        // Create a scan policy
        ScanPolicy scanPolicy = new ScanPolicy
        {
            concurrentNodes = true,
            includeBinData = true
        };

        // Define the scan callback
        void ScanCallback(Key key, Aerospike.Client.Record record)
        {
            List<Bin> payBinList = new List<Bin>();
            List<Bin> conBinList = new List<Bin>();

            foreach (string bin in payBins)
            {
                if (record.bins.ContainsKey(bin))
                {
                    payBinList.Add(new Bin(bin, record.GetValue(bin)));
                }
            }

            foreach (string bin in conBins)
            {
                if (record.bins.ContainsKey(bin))
                {
                    conBinList.Add(new Bin(bin, record.GetValue(bin)));
                }
            }

            if (payBinList.Count > 0)
            {
                client.Put(null, new Key(namespaceName, payRepoSet, record.GetString("key")), payBinList.ToArray());
            }

            if (conBinList.Count > 0)
            {
                client.Put(null, new Key(namespaceName, conRepoSet, record.GetString("key")), conBinList.ToArray());
            }
        }

        // Scan the original set and apply the callback
        client.ScanAll(scanPolicy, namespaceName, originalSet, ScanCallback, "key", "PAYc", "PAYs", "PAYcc", "PAYsc", "CONc", "CONs", "CONcc", "CONsc");

    }
}
