using System;
using Microsoft.AspNetCore.Http;

namespace Augur.ApiSystemEvents
{
    public interface IHeaderClientNotifier
    {
        void Notify(HttpResponse httpResponse, Enum apiSystemEvent);
    }
}