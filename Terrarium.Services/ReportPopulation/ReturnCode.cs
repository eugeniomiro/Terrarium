namespace Terrarium.Server.ReportPopulation
{
    public enum ReturnCode
    {
        /// <summary>
        /// Indicates a succesful operation.
        /// </summary>
        Success = 0,
        /// <summary>
        /// Indicates an attempt to register an item that has already been registered on the Terrarium Server.
        /// </summary>
        AlreadyExists = 1,
        /// <summary>
        /// ServerDown is used when the addition of a new peer registration fails 
        /// due to an error in adding the peer registration data to the database.
        /// </summary>
        ServerDown = 2,
        /// <summary>
        /// Identifies cases where the clients state is out of date.
        /// </summary>
        NodeTimedOut = 3,
        /// <summary>
        /// Identifies cases where the clients state has become corrupted.
        /// </summary>
        NodeCorrupted = 4,
        /// <summary>
        /// Identifies an organism that has been blacklisted on the Terrarium Server.
        /// The list of black-listed species is maintained by Terrarium Server administrators.
        /// </summary>
        OrganismBlacklisted = 5
    }
}
