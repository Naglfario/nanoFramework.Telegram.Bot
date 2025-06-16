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
