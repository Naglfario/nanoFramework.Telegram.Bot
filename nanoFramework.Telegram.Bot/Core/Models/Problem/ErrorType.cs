namespace nanoFramework.Telegram.Bot.Core.Models.Problem
{
    public enum ErrorType
    {
        /// <summary>
        /// Telegram "getUpdates" method returned state "ok" = false
        /// </summary>
        TelegramUpdateNotOk,

        /// <summary>
        /// System.Net.Http.HttpResponseMessage.StatusCode was not in the range 200-299
        /// </summary>
        HttpResponseInNotSuccessfull,

        /// <summary>
        /// An error occurred while trying to deserialize json
        /// </summary>
        JsonConvertReadProblem,

        /// <summary>
        /// An exception has occured
        /// </summary>
        Exception,

        /// <summary>
        /// Wrong value for poll dalay
        /// </summary>
        IncorrectPollDelay,
    }
}
