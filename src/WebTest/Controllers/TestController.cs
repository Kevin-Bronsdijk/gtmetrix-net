using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebTest.Controllers
{
    public class TestController : BaseController
    {
        public ActionResult CanCreateClient()
        {
            return ReturnResults(TestLogic.Tests.CanCreateClient());
        }

        public ActionResult CanGetLocations()
        {
            return ReturnResults(TestLogic.Tests.CanGetLocations());
        }
        public async Task<ActionResult> CanSubmitTestAsync()
        {
            var testvalue = await TestLogic.Tests.CanSubmitTestAsync();

            return ReturnResults(testvalue);
        }
    }
}