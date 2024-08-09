namespace Talabat.APIs.Errors
{
    public class ApiValidationErrorRepsonse:ApiResponse
    {
        public ApiValidationErrorRepsonse() : base(400)
        {
        }

        public IEnumerable<string> Errors { get; set; }

    }
}
