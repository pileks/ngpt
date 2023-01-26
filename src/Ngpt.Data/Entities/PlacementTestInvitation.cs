using Augur.Entity.Base.Entities;

namespace Ngpt.Data.Entities
{
    public class PlacementTestInvitation : AugurEntityWithId
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public bool HasGivenMarketingPermission { get; set; }

        public Language Language { get; set; }
        public int LanguageId { get; set; }

        public PlacementTest PlacementTest { get; set; }
        public int? PlacementTestId { get; set; }
    }
}
