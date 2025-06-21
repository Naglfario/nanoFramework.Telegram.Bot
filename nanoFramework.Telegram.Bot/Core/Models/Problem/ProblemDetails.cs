using System;

namespace nanoFramework.Telegram.Bot.Core.Models.Problem
{
    public class ProblemDetails
    {
        public ProblemDetails() { }

        public ProblemDetails(ErrorType type)
        {
            Type = type;
            
            if(type == ErrorType.JsonConvertReadProblem)
            {
                Message = "Cannot deserialize getUpdates response to valid response object";
            }
            else if(type == ErrorType.IncorrectPollDelay)
            {
                Message = "Poll delay must be >= 0";
            }
            else if(type == ErrorType.NothingToReceive)
            {
                Message = "Both available update types(messages & callback data) are disabled. " +
                    "The bot will not receive anything from Telegram, " +
                    "it will only be able to send messages.";
            }
            else if(type == ErrorType.IncorrectUpdatesLimit)
            {
                Message = "Updates limit must be from 1 to 100 (default 1). " +
                    "It is not recommended to set this parameter > 10, " +
                    "remember that there is very little RAM on embedded devices.";
            }
        }

        public ProblemDetails(Exception ex)
        {
            Type = ErrorType.Exception;
            Exception = ex;
            Message = ex.Message;
        }

        public ErrorType Type { get; set; }
        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}
