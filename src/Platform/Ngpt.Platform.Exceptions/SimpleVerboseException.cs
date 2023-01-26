using Augur.Utilities;

namespace Ngpt.Platform.Exceptions
{
    public class SimpleVerboseException : VerboseException
    {
        public SimpleVerboseException(string verboseMessage)
        {
            this.VerboseMessage = verboseMessage;
        }

        public string VerboseMessage { get; set; }

        public override object GetDto()
        {
            return new
            {
                VerboseMessage
            };
        }
    }
}