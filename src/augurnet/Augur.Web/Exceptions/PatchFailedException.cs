using System;

namespace Augur.Web.Exceptions
{
    public class PatchFailedException : Exception
    {
        public PatchFailedException(Type entityType, string id) : base(
            $"Patching an entity (Id: {id} ) of type {entityType.ToString()} failed on save!")
        {
        }
    }
}