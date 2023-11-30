# This demo is to show Thread and AsyncLocal works in all environments.  

In all kinds of applications, in httpModules (.NET framework) or middleware (.NET/Core), it get studyId 
and user name from query string, and set them into Thread using the shared library.

After various async calls, finally it can still return studyId.

On the server side, at various points, it will also verify the Thread has correct studyId.If expected
threadId is not found in the current threrad, an exception will be thrown and propagate to the caller 
(e.g. browser or Postman).  To see that, you can temporariliy change this line in   

> AmbientContext\Libs\AmbientContext.Shared.DotNetStandardLib\Verifier.cs  

>> Assert(medrioPrincipal!.Study!.ID == expectedStudyId, $"StudyId expected {expectedStudyId}, but got {medrioPrincipal.Study.ID}");  

  to  
>> Assert(medrioPrincipal!.Study!.ID == expectedStudyId + 1, $"StudyId expected {expectedStudyId}, but got {medrioPrincipal.Study.ID}");  
