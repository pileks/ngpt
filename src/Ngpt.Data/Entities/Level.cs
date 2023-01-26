using Augur.Entity.Base.Entities;

namespace Ngpt.Data.Entities
{
    public class Level : AugurEntityWithId
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public int FromRating { get; set; }
        public int Rating { get; set; }
        public int ToRating { get; set; }
    }
}