using System;
using System.Data;

namespace Terrarium.Server
{
    public enum RegisterPeerResult
    {
        /// <summary>
        /// Indicates successful registration of a peer connection.
        /// </summary>
        Success,
        /// <summary>
        /// Indicates an unsuccessful attempt to register a peer connection.
        /// </summary>
        Failure,
        /// <summary>
        /// Indicates an unsuccessful attempt to register a peer connection.
        /// </summary>
        GlobalFailure
    }
}
