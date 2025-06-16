using nanoFramework.Telegram.Bot.Core.Models.Problem;
using System.Net.Http;

namespace nanoFramework.Telegram.Bot.Extensions
{
    internal static class HttpResponseMessageExtensions
    {
        public static ProblemDetails GetProblemDetails(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return null;

            var problemDetails = new ProblemDetails()
            {
                Type = ErrorType.HttpResponseInNotSuccessfull,
                Message = $"Status code: {response.StatusCode}\r\n" +
                        $"ReasonPhrase: {response.ReasonPhrase}"
            };

            return problemDetails;
        }
    }
}
