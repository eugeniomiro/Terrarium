using System;

namespace Terrarium.Server.Abstract
{
    interface IMessaging
    {
        string GetLatestVersion();
        string GetMessageOfTheDay();
        string GetWelcomeMessage();
    }
}
