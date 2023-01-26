using System;

namespace Augur.Entity.Interfaces.Base
{
    public interface IChangeTrackable
    {
        int? CreatedById { get; set; }

        DateTime? CreatedOn { get; set; }

        int? UpdatedById { get; set; }

        DateTime? UpdatedOn { get; set; }
    }
}