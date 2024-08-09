
namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statuscode, string? message = null)
        {
            StatusCode = statuscode;
            Message = message ?? GetDefaultMessageResponse(statuscode);
        }

        private string? GetDefaultMessageResponse(int statuscode)
        {
            return statuscode switch
            {
                400 => "BadRequest Error",
                401 => "UnAuthorized Error",
                404 => "Not Found Error",
                500 => "Internal Server Error",
                  _ => "Error has been occured"
            };
        }
    }
}
