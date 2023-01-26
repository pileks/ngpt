using System;

namespace Augur.Web
{
    public class ExportToFrontendWithResponseTypeAttribute : Attribute
    {
        public ExportToFrontendWithResponseTypeAttribute(string responseType)
        {
            this.ResponseType = responseType;
        }

        public string ResponseType { get; set; }
    }
}