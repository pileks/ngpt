using System;

namespace Augur.Web.Exceptions
{
    public class CreateCollectionFailedException : Exception
    {
        public CreateCollectionFailedException(Type entityType) : base(
            $"Creating a collection of entities of type {entityType.ToString()} failed on save!")
        {
        }
    }
}