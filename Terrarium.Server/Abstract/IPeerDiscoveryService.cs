using System;
using System.Data;

namespace Terrarium.Server.Abstract
{
    interface IPeerDiscovery
    {
        int GetNumPeers(string version, string channel);
        int GetTotalNumPeers();
        bool IsVersionDisabled(string version, out string errorMessage);
        RegisterPeerResult RegisterMyPeerGetCountAndPeerList(string version, string channel, Guid guid, out DataSet peers, out int count);
        bool RegisterUser(string email);
        string ValidatePeer();
    }
}
