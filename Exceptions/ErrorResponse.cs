using System.Text.Json;

namespace Invoice.Exceptions
{
    public class ErrorResponse
    {
        public string Error { get; set; }


        public ErrorResponse(string error)
        {
            Error = error;
        }

        public override string ToString() => JsonSerializer.Serialize(this);        
    }
}