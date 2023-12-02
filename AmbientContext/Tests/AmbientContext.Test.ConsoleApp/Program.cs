using static System.Net.Mime.MediaTypeNames;

await TestIISUrls();

async Task TestIISUrls()
{
    Console.WriteLine("Start test IIS");

    var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri("https://ambientcontextwebform.local.medrio.com:8443/");

    (string template, Func<int, int, bool>? studyIdVerifier)[] subUrlTemplates = new (string template, Func<int, int, bool>? studyIdVerifier)[] { 
        ("Data?userName=Ryan&studyId={0}", null),
        ("Test/Index?userName=Ryan&studyId={0}" , null),
        ("Test/CheckInTask?userName=Ryan&studyId={0}", null),
        ("Test/CheckInThread?userName=Ryan&studyId={0}", null),
        ("Test/UpdateStudyIdBy5000?userName=Ryan&studyId={0}&notVerifyAtEndRequest=", (int studyId, int studyIdResult) => studyIdResult == studyId + 5000 ),
        ("Test/UpdateStudyIdBy5000InTask?userName=Ryan&studyId={0}&notVerifyAtEndRequest=", (int studyId, int studyIdResult) => studyIdResult == studyId + 5000 )
    };

    var tests = from t in subUrlTemplates
        select TestIISUrl(httpClient, 60, t.template, t.studyIdVerifier);
    await Task.WhenAll(tests).ConfigureAwait(false);
}

async Task TestIISUrl(HttpClient httpClient, int n, string subUrlTemplate, Func<int, int, bool>? studyIdVerifier  = null)
{
    Console.WriteLine($"Start test IIS {subUrlTemplate} .........");
    studyIdVerifier ??= (int passIn, int result) => passIn == result;

    var results = new (int studyId, bool isOk, int studyIdResult, string? message)[n];
    var tasks = Enumerable.Range(0, n).Select(i =>
    {
        var studyId =  i;
        var task = httpClient.GetAsync(string.Format(subUrlTemplate, studyId));

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

    var failedCount = results.Count(r => r.isOk == false);
    if (failedCount != 0)
    {
        Console.WriteLine($"{failedCount} out of {n} API called Failed.");
        Console.WriteLine(results.First(r => r.isOk == false).message);
    }
    else
    {
        // calls are all OK

        var wrongCount = results.Count(r => r.isOk && !studyIdVerifier(r.studyId, r.studyIdResult));

        if (wrongCount != 0)
        {
            Console.WriteLine($"{wrongCount} out of {n} got wrong studyId back");
        }
        else
        {
            Console.WriteLine($"All {n} good for IIS - {subUrlTemplate} <<<<<<<<<<<<<<<< ");
        }
    }

    void PrintResults((int studyId, bool isOk, int studyIdResult, string? message)[] results)
    {
        foreach (var r in results.TakeLast(50))
        {
            Console.WriteLine(r);
        }
        Console.WriteLine( "... ...");
    }
}



