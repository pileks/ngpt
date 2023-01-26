using Augur.Security.Permissions.Components;

namespace Ngpt.Platform.AccessControl
{
    public class Components
    {
        public class Email : EmptyComponent<Email>
        {
            public static Activity BulkSend { get; set; }
        }
    }
}
