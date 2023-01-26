using System;

namespace Augur.Web.Exceptions
{
    public class DeleteFailedException : Exception
    {
        public DeleteFailedException(Type entityType) : base(
            $"Deleting an entity of type {entityType.ToString()} failed on save!")
        {
        }
    }
}