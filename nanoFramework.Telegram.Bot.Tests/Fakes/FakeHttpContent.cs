using System.IO;
using System.Net.Http;
using System.Text;

namespace nanoFramework.Telegram.Bot.Tests.Fakes
{
    public class FakeHttpContent : HttpContent
    {
        private readonly string _content;
        public FakeHttpContent(string content = "{ }")
        {
            _content = content;
        }

        protected override void SerializeToStream(Stream stream)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(_content);
            stream.Write(byteArray, 0, byteArray.Length);
        }

        protected override bool TryComputeLength(out long length)
        {
            var byteArray = Encoding.UTF8.GetBytes(_content);
            var stream = new MemoryStream(byteArray, false);
            length = stream.Length;
            return true;
        }
    }
}
