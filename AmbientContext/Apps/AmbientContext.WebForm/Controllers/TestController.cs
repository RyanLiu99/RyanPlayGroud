using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Mvc;
using AmbientContext.Shared.DotNetStandardLib;
using AmbientContextDotNetFrameworkWebLib;


namespace AmbientContextWebForm.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public async Task<ActionResult> Index()
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            return View();
        }

        public async Task<ContentResult> CheckInTask()
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);

            await Task.Run(async () =>
            {
                await AsyncActor.DoSthAsync().ConfigureAwait(false);
                TestHelper.Verify(this.HttpContext);
            });
            return Content(AuthHelper.GetCurrentStudyIdFromThread().ToString());
        }

        public async Task<ContentResult> CheckInThread()
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);

            var studyId = TestHelper.GetDataFromRequest(Request).StudyId;
            Exception ex = null;

            Thread t = new Thread((expectedStudyId) =>
            {
                try
                {
                    TestHelper.Verify(this.HttpContext);
                }
                catch (Exception e)
                {
                    ex = e;
                }
            });

            t.Start(studyId);
            t.Join();

            if (ex == null)
            {
                return Content(AuthHelper.GetCurrentStudyIdFromThread().ToString());
            }
            else
            {
                return Content(ex.ToString());
            }
        }

        [System.Web.Mvc.HttpGet]
        public async Task<ContentResult> UpdateStudyIdBy5000()
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            TestHelper.Verify(this.HttpContext);
            var queryStudyId = TestHelper.GetDataFromRequest(Request).StudyId;
            
            long newStudyId = queryStudyId + 5000;
            await AuthHelper.ChangeThreadStudyAsync(newStudyId).ConfigureAwait(false);
            
            await AsyncActor.DoSthAsync().ConfigureAwait(false);

            Verifier.VerifyThreadData(newStudyId); 
            // Verifier.VerifyStoreData(newStudyId); //Can not get study back from store!

            return Content(AuthHelper.GetCurrentStudyIdFromThread().ToString());
        }

        [System.Web.Mvc.HttpGet]
        public async Task<ContentResult> UpdateStudyIdBy3000InTask()
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            TestHelper.Verify(this.HttpContext);
            var queryStudyId = TestHelper.GetDataFromRequest(Request).StudyId;

            long newStudyId = queryStudyId + 3000;

            await AuthHelper.OverwriteThreadStudyIdInManualTask(newStudyId).ConfigureAwait(false);
           
            await AsyncActor.DoSthAsync().ConfigureAwait(false);

            Verifier.VerifyThreadData(newStudyId);
            // Verifier.VerifyStoreData(newStudyId); //Can not get study back from store!

            return Content(AuthHelper.GetCurrentStudyIdFromThread().ToString());
        }
    }
}