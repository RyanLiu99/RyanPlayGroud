using System;
using System.Collections.Generic;
using Elasticsearch.Net;
using Nest;

namespace ElasticsearchCRUD
{
    class Program
    {

        static  async Task Main(string[] args)
        {
            try
            {
                //Main1();
                // await Program2.Main2();
                await LocalProgram3.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex}");
            }
            
        }

        //kibana_system/xpnxx9nrQfuFpnE2m+Jc
        static void Main1()
        {
            var url = "https://localhost:9200/";
            var settings = new ConnectionSettings(new Uri(url))
                .BasicAuthentication("elastic", "DYSt-rYpiasj27pFWHHp")  //localhost in docker, works
                                                                         //.BasicAuthentication("ryanuser", "ryan_pwd") // I created this admin user by POST locally, works, it can also log into Kibana
                .DefaultIndex("ryanindex1")
                .EnableApiVersioningHeader()
                .ServerCertificateValidationCallback((o, certificate, chain, errors) => true); // Bypass SSL certificate validation;

            //// //curl -u ryanuser1:qqbZ2nMm9kRRiRR https://a68c38bed0da42aea85d5854ea2d11df.us-central1.gcp.cloud.es.io:443 works
            //// organization key: essu_TWtZNWFWTlpORUpFUTBnelNXbEtiVmxMWjJZNlVuTmFSRkl0TFhGUmQxZGhWRXhJVmpSbE0wODRadz09AAAAABBHUeo=
            //var url = "https://a68c38bed0da42aea85d5854ea2d11df.us-central1.gcp.cloud.es.io:443";
            //var settings = new ConnectionSettings(new Uri(url))
            //   //.ApiKeyAuthentication("ryancode", "emZ3UFE0NEJmcFE1cEpBMUxlR3k6MWZqM0I0RnhRTHF5cFd2QjRBTDBJZw==") // not working, api key for org owner with all permissions
            //   .ApiKeyAuthentication("ryabapikey2", "bFB4MFNZNEJmcFE1cEpBMUplS1I6WDM5ck41Wi1RMFNrWlpMZHN4a0tGdw==") // not working either, api key for org owner with all permissions
            //                                                                                                        // .BasicAuthentication("ryanuser1", "qqbZ2nMm9kRRiRR") // docker cloud, works
            //    .DefaultIndex("ryanindex1")
            //    .EnableApiVersioningHeader()
            //    .ServerCertificateValidationCallback((o, certificate, chain, errors) => true); // Bypass SSL certificate validation;

            var client = new ElasticClient(settings);

            // Indexing a document
            var document = new YourDocumentModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Ryan Document from VS code",
                Description = "CRUD operations in Elasticsearch from Program.cs."
            };

            var indexResponse = client.IndexDocument(document);
            if (indexResponse.IsValid)
            {
                Console.WriteLine($"Document indexed successfully with ID: {indexResponse.Id}");
            }
            else
            {
                Console.WriteLine($"Error indexing document: {indexResponse.DebugInformation}");
            }

            // Retrieving a document
            var getResponse = client.Get<YourDocumentModel>(document.Id);
            if (getResponse.Found)
            {
                var retrievedDocument = getResponse.Source;
                Console.WriteLine($"Retrieved Document: {retrievedDocument.Name}");
            }
            else
            {
                Console.WriteLine($"Document not found with ID: {document.Id}");
            }

            // Updating a document
            document.Description = "Updated description";
            var updateResponse = client.Update<YourDocumentModel, object>(document.Id, u => u
                .Doc(document)
                .RetryOnConflict(3));

            if (updateResponse.IsValid)
            {
                Console.WriteLine($"Document updated successfully");
            }
            else
            {
                Console.WriteLine($"Error updating document: {updateResponse.DebugInformation}");
            }

            // // Deleting a document
            // var deleteResponse = client.Delete<YourDocumentModel>(document.Id);
            // if (deleteResponse.IsValid)
            // {
            //     Console.WriteLine($"Document deleted successfully");
            // }
            // else
            // {
            //     Console.WriteLine($"Error deleting document: {deleteResponse.DebugInformation}");
            // }
        }
    }

    public class YourDocumentModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // Add other properties as needed
    }
}
