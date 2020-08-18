using Newtonsoft.Json;

namespace PathToMastery.Models.Web.Response
{
    public class ErrorResponse
    {
        public string Error { get; }

        public ErrorResponse(string error)
        {
            Error = error;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Utils.ConverterSettings);
        }
    }
}