using System;

namespace Augur.Web.Exceptions
{
    public class CreateFailedException : Exception
    {
        public CreateFailedException(Type entityType) : base(
            $"Creating an entity of type {entityType.ToString()} failed on save!")
        {
        }
    }
}