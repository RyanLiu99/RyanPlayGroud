using System.Buffers;
using static System.Net.Mime.MediaTypeNames;

var BigStringContent = new StringContent(new string('a', 1_000_000));

//await TestIISUrls().ConfigureAwait(false);
await TestDotNet6Urls().ConfigureAwait(false);


async Task TestIISUrls()
{
    Console.Clear();
    Console.WriteLine(" =========================== Start test IIS ... ==========================");

    var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri("https://ambientcontextwebform.local.medrio.com:8443/");

    var subUrlTemplates = new (string httpMethod, string template, Func<int, int, bool>? studyIdVerifier)[]{ 
        ("GET", "Data?userName=Ryan&studyId={0}", null),
        ("GET", "Test/Index?userName=Ryan&studyId={0}" , null),
        ("GET", "Test/CheckInTask?userName=Ryan&studyId={0}", null),
        ("GET", "Test/CheckInThread?userName=Ryan&studyId={0}", null),
        ("GET", "Test/UpdateStudyIdBy5000?userName=Ryan&studyId={0}&notVerifyAtEndRequest=", (int studyId, int studyIdResult) => studyIdResult == studyId + 5000 ),
        ("GET", "Test/UpdateStudyIdBy3000InTask?userName=Ryan&studyId={0}&notVerifyAtEndRequest=", (int studyId, int studyIdResult) => studyIdResult == studyId + 3000 )
    };

    var tests = from t in subUrlTemplates
        select TestEndpoint(httpClient, 40, t.httpMethod, t.template, t.studyIdVerifier);
    var results = await Task.WhenAll(tests).ConfigureAwait(false);
    Console.WriteLine($" ----------------------- {results.Count(x => x)} out of {results.Length} IIS endpoints succeeded... ------------------------");
}

async Task TestDotNet6Urls()
{
    Console.WriteLine(" ==========================  Start test .NET 6 endpoints ... ==========================");

    var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri("https://localhost:7062/api/");

    var subUrlTemplates = new (string httpMethod, string template, Func<int, int, bool>? studyIdVerifier)[]{
      //  ("GET", "Values?userName=Ryan&StudyId={0}", null),
      //  ("GET", "Values/135?userName=Ryan&StudyId={0}&notVerifyAtEndRequest=",  (int studyId, int studyIdResult) => studyIdResult == 135),
        ("POST","Values?userName=Ryan&StudyId={0}",  null),
    };

    var tests = from t in subUrlTemplates
        select TestEndpoint(httpClient, 50, t.httpMethod,  t.template, t.studyIdVerifier);
    var results = await Task.WhenAll(tests).ConfigureAwait(false);
    Console.WriteLine($" ----------------------- {results.Count(x => x)} out of {results.Length}  .NET 6 endpoints succeeded... ------------------------");
}

async Task<bool> TestEndpoint(HttpClient httpClient, int n, string httpMethod, string subUrlTemplate, Func<int, int, bool>? studyIdVerifier = null)
{
    //using HttpClient httpClient2 = new HttpClient((HttpMessageHandler)new HttpClientHandler(), false) { BaseAddress = httpClient.BaseAddress };
    //httpClient.Dispose();

    Console.WriteLine($"Start test {httpMethod} ep {subUrlTemplate} .........");
    studyIdVerifier ??= (int passIn, int result) => passIn == result;

    var results = new (int studyId, bool isOk, int studyIdResult, string? message)[n];
    var tasks = Enumerable.Range(0, n).Select(i =>
    {
        var studyId = i;
        string uri = string.Format(subUrlTemplate, studyId);

        Task<HttpResponseMessage> task = null;

        if (httpMethod.Equals("POST"))
        {
            task = httpClient.PostAsync(uri, BigStringContent);
        }
        else
        {
            task = httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(httpMethod), uri));
        }

        var taskTask = task.ContinueWith(async (t, studyIdState) =>
        {
            int studyIdPassIn = (int)studyIdState!;
            if (t.IsCompletedSuccessfully)
            {
                results[studyIdPassIn] = (studyIdPassIn, true, int.Parse(await t.Result.Content.ReadAsStringAsync()), null);
            }
            else
            {
                results[studyIdPassIn] = (studyIdPassIn, false, -1, t.Exception?.GetBaseException()?.Message);
            }

        }, studyId);

        return taskTask.Unwrap();

    });

    await Task.WhenAll(tasks).ConfigureAwait(false);

    PrintResults(results);
    var end = $" For ep - {httpClient.BaseAddress}{subUrlTemplate} <<<<<<<<<<<<<<<< \r\n\r\n";

    var failedCount = results.Count(r => r.isOk == false);
    if (failedCount != 0)
    {
        Console.WriteLine($"{failedCount} out of {n} API called Failed. {end}");
        Console.WriteLine(results.First(r => r.isOk == false).message);
        return false;
    }
    else
    {
        // calls are all made, check result

        var wrongCount = results.Count(r => r.isOk && !studyIdVerifier(r.studyId, r.studyIdResult));

        if (wrongCount != 0)
        {
            Console.WriteLine($"{wrongCount} out of {n} got wrong studyId back. {end}");
            return false;
        }
        else
        {
            Console.WriteLine($"All {n} good. {end}");
            return true;
        }

    }
}

void PrintResults((int studyId, bool isOk, int studyIdResult, string? message)[] results)
{
    foreach (var r in results.TakeLast(50))
    {
        Console.WriteLine(r);
    }

    Console.WriteLine("... ...");
}

//ResiliencePipeline Retry()
//{
//    ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
//        .AddRetry(new RetryStrategyOptions() { Delay = TimeSpan.FromMilliseconds(50)}) 
//        .Build();
//    return pipeline;
//}

