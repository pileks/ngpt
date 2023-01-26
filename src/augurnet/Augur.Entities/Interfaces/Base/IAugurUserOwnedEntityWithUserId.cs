namespace Augur.Entity.Interfaces.Base
{
    public interface IAugurUserOwnedEntityWithUserId : IAugurUserOwnedEntity
    {
        int UserId { get; set; }
    }
}