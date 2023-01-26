using Augur.Entity.Interfaces.Base;

namespace Augur.Entity.Base.Entities
{
    public class AugurEntityWithId : AugurEntity, IAugurEntityWithId
    {
        public int Id { get; set; }
    }
}