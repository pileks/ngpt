using System;

namespace Augur.Web.Exceptions
{
    public class UpdateFailedException : Exception
    {
        public UpdateFailedException(Type entityType, string id) : base(
            $"Updating an entity (Id: {id} ) of type {entityType.ToString()} failed on save!")
        {
        }
    }
}