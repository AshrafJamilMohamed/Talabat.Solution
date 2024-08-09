using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
    [Route("errors/{Code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class errorsController : ControllerBase
    {
        public ActionResult Error(int Code)
        {
            if (Code == 400)
                return BadRequest(new ApiResponse(Code));

            else if (Code == 401)
                return Unauthorized(new ApiResponse(Code));

            else if (Code == 404)
                return NotFound(new ApiResponse(Code));

            else
                return StatusCode(Code);
        }
    }
}
