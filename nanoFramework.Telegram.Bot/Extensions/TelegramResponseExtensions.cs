using nanoFramework.Telegram.Bot.Core.Models;
using nanoFramework.Telegram.Bot.Core.Models.Problem;

namespace nanoFramework.Telegram.Bot.Extensions
{
    internal static class TelegramResponseExtensions
    {
        public static ProblemDetails GetProblemDetails(this ITelegramResponse response)
        {
            if (response == null)
            {
                return new ProblemDetails(ErrorType.JsonConvertReadProblem);
            }
            else if (!response.ok)
            {
                var problemDetails = new ProblemDetails()
                {
                    Type = ErrorType.TelegramUpdateNotOk,
                    Message = $"Error code: {response.error_code}\r\n" +
                    $"Description: {response.description}"
                };

                return problemDetails;
            }

            return null;
        }
    }
}
