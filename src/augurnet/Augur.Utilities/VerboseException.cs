using System;

namespace Augur.Utilities
{
    public abstract class VerboseException : Exception
    {
        public abstract object GetDto();
    }
}
