namespace MiPrimerApi.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse() { }

        public ErrorResponse(string message, string code = "") 
        {
            Message = message;
            Code = code;
        }

        public string Message { get; set; } = string.Empty;
        public string Code { get; set; }
    }
}
