# This demo is to test Thread and AsyncLoca in all environments.  
* Thread works everywhere in any environments in any case.
* AsyncLocal works in .NET Core/.NET 5+, but does not in all cases in classic ASP.NET 
  - For web forms/http modules, cannot get data back from AsyncLocal after async life event handler
  - For mix of async MVC action + HttpModule, Context_EndRequest in http module cannot get data from AsyncLocal

# How it works
In all kinds of applications/ environments, in httpModules (.NET framework) or middleware (.NET/Core), its get studyId 
and user name from query string, and set them into Thread using the shared library.

After various async calls, finally it can still return studyId.  So you can verify it on the client side.

For each call, it is also compare data in Thread and AsyncLocal againt data in query string on the server side at various points:

	* .NET framework
	  - OnAuthenticateRequest
	  - OnEndRequest  (Exceptions apply)
	  - On Page_Load (.aspx only) 
	* .NET / Core
	  - In a custom Middleware, before and after (exceptions apply) call next middleware.
    * In Both
	  - In Applicaton code, in a manually created Task or Thread

If the code further change data in Thread or AsyncLocal, then no verification at OnEndRequest or after calling next middle ware.

Along the way, .ConfigureAwait(false) is used at many places.  
If expected data is not found in the current thread, an exception will be thrown and propagate to the caller 
(e.g. browser or Postman).  To see that, you can temporariliy change this line in   

> AmbientContext\Libs\AmbientContext.Shared.DotNetStandardLib\Verifier.cs  

>> Assert(medrioPrincipal!.Study!.ID == expectedStudyId, $"StudyId expected {expectedStudyId}, but got {medrioPrincipal.Study.ID}");  

  to  
>> Assert(medrioPrincipal!.Study!.ID == expectedStudyId + 1, $"StudyId expected {expectedStudyId}, but got {medrioPrincipal.Study.ID}");  

# Hosting environemnts
1. IIS on Windows (.NET framework)  
2. Linux Docker (.NET 6 )

Two main hosting models we have for production.

# Sloution structure

## Sites/Projects

* Apps\AmbientContext.WebForm    (.NET framework)
* Apps\AmbientContext.AspNetCore (.NET 6)

## Libraries
* Libs\AmbientContext.Shared.DotNetStandardLib -- main lib, used by both .NET framework and .NET/Core apps
* Libs\AmbientContext.DotNetFrameworkWebLib -- .NET framework lib, mainly contains httpModule
* Libs\AmbientContext.AspNetLibInDotNetStandard -- written in .NET standard, by used by .NET/core app only, mainly contains middleware
 
## Tests

	 
# End Points 

## .NET framework - Apps/AmbientContext.WebForm

### .aspx page
https://ambientcontextwebform.local.medrio.com:8443/?userName=Ryan&studyId=100  
Home page, it should can be load correctly, menas all server side verification should pass (so are other endpoints).

https://ambientcontextwebform.local.medrio.com:8443/Data?userName=Ryan&studyId=1500  
Data aspx page, it should return studyId set in the query string.

https://ambientcontextwebform.local.medrio.com:8443/Test/Index?userName=Ryan&studyId=200  
MVC controller returns view, it should return studyId set in the query string.

https://ambientcontextwebform.local.medrio.com:8443/Test/CheckInTask?userName=Ryan&studyId=200
MVC controller returns content result, it should return studyId set in the query string.  
It also verify studyId in a manually created task.

https://ambientcontextwebform.local.medrio.com:8443/Test/CheckInThread?userName=Ryan&studyId=277
MVC controller returns content result, it should return studyId set in the query string.  
It also verify studyId in a manually created Thread.

https://ambientcontextwebform.local.medrio.com:8443/Test/UpdateStudyIdBy5000?userName=Ryan&studyId=277&notVerifyAtEndRequest=
Update current studyId asynchronously. The chagne is seen afterwards.


https://ambientcontextwebform.local.medrio.com:8443/Test/UpdateStudyIdBy5000InTask?userName=Ryan&studyId=277&notVerifyAtEndRequest=
Update current studyId asynchronously in a subroutin in which it manually creates a task to update studyId. The chagne is seen everywhere afterwards.

## .NET 6 - Apps\AmbientContext.AspNetCore
https://localhost:7062/api/Values?userName=Ryan&StudyId=130  
API controller, it should return studyId set in the query string.

https://localhost:7062/api/Values/135?userName=Ryan&StudyId=130  
API controller, it sets studyId set in the query string (130), but then overitten by route data and return 135 .
