using System;
using Microsoft.AspNetCore.Http;

namespace Augur.ApiSystemEvents
{
    public class AugurHeaderClientNotifier : IHeaderClientNotifier
    {
        public void Notify(HttpResponse httpResponse, Enum apiSystemEvent)
        {
            httpResponse.Headers.Add("ApiSystemEventNofication", Convert.ToInt32(apiSystemEvent).ToString());
        }
    }
}