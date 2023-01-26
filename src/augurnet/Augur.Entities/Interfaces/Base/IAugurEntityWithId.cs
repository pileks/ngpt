namespace Augur.Entity.Interfaces.Base
{
    public interface IAugurEntityWithId : IAugurEntityWithId<int>
    {
    }

    public interface IAugurEntityWithId<TId> : IAugurEntity
    {
        TId Id { get; set; }
    }
}