using System;
using Augur.BackendToFrontendExporter;
using Augur.Entity.Base.Entities;

namespace Ngpt.Platform.Data.Entities
{
    [DisableExportToFrontend]
    public class SentEmail : AugurEntityWithId
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string PlainTextBody { get; set; }
        public DateTime Date { get; set; }
    }
}
