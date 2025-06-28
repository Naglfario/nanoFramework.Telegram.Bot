using nanoFramework.Telegram.Bot.Core.Models.Problem;

namespace nanoFramework.Telegram.Bot.Core.Models
{
    public class SendResult
    {
        public SendResult()
        {
            ok = true;
            details = null;
        }

        public SendResult(ProblemDetails problemDetails)
        {
            ok = false;
            details = problemDetails;
        }

        public bool ok { get; set; }

        public ProblemDetails details { get; set; }
    }
}
