﻿//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Web.Services;
using Terrarium.Services;

namespace Terrarium.Server
{
    /*
        Enum:       SpeciesServiceStatus
    */
    /// <summary>
    /// This enumeration contains return codes identifying
    /// the results of an creature insertion into the Terrarium.
    /// Success identifies an inserted creature.  AlreadyExists means
    /// the creatures name has already been taken and a new name must be given.
    /// ServerDown means that a database error occured and the creature
    /// was not submitted.  VersionIncompatible is used when the required
    /// parameters aren't sent to the function.  FiveMinuteThrottle is
    /// returned whenever a user tries to submit more than one animal
    /// per 5 minutes.  TwentyFourHourThrottle occurs whenever a user
    /// tries to submit more than 30 creatures in a 24 hour period.			
    /// The last three are used whenever a passed string fails policheck.	
    /// Each enum value identifies a failure in a different string, whether
    /// the species name, author name, or the email has an questionable value.
    /// </summary>
    public enum SpeciesServiceStatus
    {
        Success,
        AlreadyExists,
        ServerDown,
        VersionIncompatible,
        FiveMinuteThrottle,
        TwentyFourHourThrottle,
        PoliCheckSpeciesNameFailure,
        PoliCheckAuthorNameFailure,
        PoliCheckEmailFailure
    }

    /*
        Class:      SpeciesService
    */
    /// <summary>
    /// The SpeciesService class encapsulates the functions
    /// required to insert new creatures into the EcoSystem, and get
    /// creatures from the server during a reintroduction.
    /// </summary>
    public class SpeciesService : WebService, ISpeciesService
    {
        private static PerformanceCounter speciesAllPerformanceCounter;
        private static PerformanceCounter speciesAllFailedPerformanceCounter;

        /*
            Method:     SpeciesService
        */
        /// <summary>
        /// This function creates the performance counters used	to display performance results.
        /// </summary>
        static SpeciesService()
        {
            try
            {
                speciesAllPerformanceCounter = InstallerInfo.CreatePerformanceCounter("AllSpecies");
                speciesAllFailedPerformanceCounter = InstallerInfo.CreatePerformanceCounter("AllFailedSpecies");
            }
            catch (Exception e)
            {
                InstallerInfo.WriteEventLog("PerformanceCounters", "Could not create Performance Counter: " + e.ToString());
            }
        }

        /*
                Method:     Add
        */
        /// <summary>
        ///  This function takes a creature assembly, author information,
        ///  and a species name and attempts to insert the creature into the EcoSystem.
        ///  This involves adding the creature to the database and saving the assembly
        ///  on the server so it can later be used for reintroductions.
        ///  All strings are checked for inflammatory words and the insertion is not
        ///  performed if any are found.  In addition the 5 minute rule is checked to
        ///  make sure the user isn't spamming the server.  Only 1 upload is allowed
        ///  per 5 minutes.  An additional constraint of only 30 uploads per day is
        ///  also enforced through the 24 hour rule. 
        ///  
        ///  The creature is then inserted into the database.  If the creature already
        ///  exists the function tells the client the creature is preexisting and the
        ///  insert fails.  If the insert is successful the creature is then saved to disk on the server.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="author"></param>
        /// <param name="email"></param>
        /// <param name="assemblyFullName"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [WebMethod]
        public SpeciesServiceStatus Add(string name, string version, string type, string author, string email, string assemblyFullName, byte[] assemblyCode)
        {
            if (name == null || version == null || type == null || author == null || email == null || assemblyFullName == null || assemblyCode == null)
            {
                // Special versioning case, if all parameters are not specified then we return an appropriate error.
                InstallerInfo.WriteEventLog("AddSpecies", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString());
                return SpeciesServiceStatus.VersionIncompatible;
            }

            version = new Version(version).ToString(3);

            bool nameInappropriate = WordFilter.RunQuickWordFilter(name);
            bool authInappropriate = WordFilter.RunQuickWordFilter(author);
            bool emailInappropriate = WordFilter.RunQuickWordFilter(email);
            bool inappropriate = nameInappropriate | authInappropriate | emailInappropriate;
            bool insertComplete = false;

            bool allow = !Throttle.Throttled(
                Context.Request.ServerVariables["REMOTE_ADDR"].ToString(),
                "AddSpecies5MinuteThrottle"
                );


            if (allow)
            {
                allow = !Throttle.Throttled(
                    Context.Request.ServerVariables["REMOTE_ADDR"].ToString(),
                    "AddSpecies24HourThrottle"
                    );
                if (!allow)
                    return SpeciesServiceStatus.TwentyFourHourThrottle;
            }
            else
                return SpeciesServiceStatus.FiveMinuteThrottle;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn))
                {
                    myConnection.Open();
                    SqlTransaction transaction = myConnection.BeginTransaction();

                    SqlCommand mySqlCommand = new SqlCommand("TerrariumInsertSpecies", myConnection, transaction);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameterCollection cmdParms = mySqlCommand.Parameters;
                    SqlParameter parmName = cmdParms.Add("@Name", SqlDbType.VarChar, 255); parmName.Value = name;
                    SqlParameter parmVersion = cmdParms.Add("@Version", SqlDbType.VarChar, 255); parmVersion.Value = version;
                    SqlParameter parmType = cmdParms.Add("@Type", SqlDbType.VarChar, 50); parmType.Value = type;
                    SqlParameter parmAuthor = cmdParms.Add("@Author", SqlDbType.VarChar, 255); parmAuthor.Value = author;
                    SqlParameter parmAuthorEmail = cmdParms.Add("@AuthorEmail", SqlDbType.VarChar, 255); parmAuthorEmail.Value = email;
                    SqlParameter parmExtinct = cmdParms.Add("@Extinct", SqlDbType.TinyInt, 1); parmExtinct.Value = 0;
                    SqlParameter parmDateAdded = cmdParms.Add("@DateAdded", SqlDbType.DateTime, 8); parmDateAdded.Value = DateTime.Now;
                    SqlParameter parmAssembly = cmdParms.Add("@AssemblyFullName", SqlDbType.Text, Int32.MaxValue); parmAssembly.Value = assemblyFullName;
                    SqlParameter parmBlackListed = cmdParms.Add("@BlackListed", SqlDbType.Bit, 1); parmBlackListed.Value = inappropriate;

                    try
                    {
                        mySqlCommand.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        // 2627 is Primary key violation
                        if (e.Number == 2627)
                            return SpeciesServiceStatus.AlreadyExists;
                        else
                            throw;
                    }

                    int introductionWait = (int)ServerSettings.IntroductionWait;

                    Throttle.AddThrottle(
                        Context.Request.ServerVariables["REMOTE_ADDR"].ToString(),
                        "AddSpecies5MinuteThrottle",
                        1,
                        DateTime.Now.AddMinutes(introductionWait)
                        );

                    int introductionDailyLimit = (int)ServerSettings.IntroductionDailyLimit;

                    Throttle.AddThrottle(
                        Context.Request.ServerVariables["REMOTE_ADDR"].ToString(),
                        "AddSpecies24HourThrottle",
                        introductionDailyLimit,
                        DateTime.Now.AddHours(24)
                        );
                    insertComplete = true;
                    SaveAssembly(assemblyCode, version, name + ".dll");
                    transaction.Commit();
                }
            }
            catch (ApplicationException e)
            {
                InstallerInfo.WriteEventLog("AddSpecies", e.ToString());

                return SpeciesServiceStatus.AlreadyExists;
            }
            catch (Exception e)
            {
                InstallerInfo.WriteEventLog("AddSpecies", e.ToString());

                if (insertComplete)
                    RemoveAssembly(version, name);

                return SpeciesServiceStatus.ServerDown;
            }

            if (inappropriate)
            {
                if (nameInappropriate)
                    return SpeciesServiceStatus.PoliCheckSpeciesNameFailure;
                if (authInappropriate)
                    return SpeciesServiceStatus.PoliCheckAuthorNameFailure;
                if (emailInappropriate)
                    return SpeciesServiceStatus.PoliCheckEmailFailure;

                return SpeciesServiceStatus.AlreadyExists;
            }
            else
                return SpeciesServiceStatus.Success;
        }

        /*
            Method:     LoadAssembly
            Purpose:    Grabs an assembly off the disk and returns it as a
            byte array.
        */
        public byte[] LoadAssembly(string version, string assemblyFileName)
        {
            String assemblyRoot = ServerSettings.AssemblyPath;
            version = new Version(version).ToString(3);

            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, new string[] { assemblyRoot + "\\" + version });
            byte[] bytes = null;
            try {
                permission.PermitOnly();

                using (FileStream sourceStream = File.OpenRead(assemblyRoot + "\\" + version + "\\" + assemblyFileName)) {
                    bytes = new byte[sourceStream.Length];
                    sourceStream.Read(bytes, 0, (int) sourceStream.Length);
                }
            } finally {
                CodeAccessPermission.RevertPermitOnly();
            }

            return bytes;
        }

        /*
            Method:     RemoveAssembly
            Purpose:    Attempts to delete an assembly from the servers disk
            cache.
        */
        public void RemoveAssembly(string version, string assemblyFileName)
        {
            String assemblyRoot = ServerSettings.AssemblyPath;
            version = new Version(version).ToString(3);

            DirectoryInfo path = new DirectoryInfo(assemblyRoot + "\\" + version);
            if (!path.Exists)
                return;

            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, new string[] { assemblyRoot + "\\" + version });
            try {
                permission.PermitOnly();
                if (File.Exists(assemblyRoot + "\\" + version + "\\" + assemblyFileName))
                    File.Delete(assemblyRoot + "\\" + version + "\\" + assemblyFileName);
            } finally {
                CodeAccessPermission.RevertPermitOnly();
            }
        }

        /*
            Method:     SaveAssembly
            Purpose:    Attemps to save a byte array as an assembly on the
                        servers disk cache.
        */
        public void SaveAssembly(byte[] assemblyCode, string version, string assemblyFileName)
        {
            String assemblyRoot = ServerSettings.AssemblyPath;
            version = new Version(version).ToString(3);

            DirectoryInfo path = new DirectoryInfo(assemblyRoot + "\\" + version);
            if (!path.Exists)
                path.Create();

            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, new string[] { assemblyRoot + "\\" + version });
            try {
                permission.PermitOnly();

                // Use CreateNew to create so we get an exception if the file already exists -- it never should
                string pathToAssembly = assemblyRoot + "\\" + version + "\\" + assemblyFileName;
                if (File.Exists(pathToAssembly)) {
                    throw new ApplicationException("an assembly with the same filename already exists in the server");
                }
                using (FileStream targetStream = File.Open(pathToAssembly, FileMode.CreateNew)) {
                    try {
                        targetStream.Write(assemblyCode, 0, assemblyCode.Length);
                        targetStream.Close();
                    } catch {
                        targetStream.Close();

                        // If something happens, delete the file so we don't have
                        // a corrupted file hanging around
                        File.Delete(pathToAssembly);

                        throw;
                    }
                }
            } finally {
                CodeAccessPermission.RevertPermitOnly();
            }
        }

        /*
            Method:     GetExtinctSpecies
            Purpose:    Returns a dataset of all species whose population
                        has reached 0 and therefore can be reintroduced.
        */
        [WebMethod]
        public DataSet GetExtinctSpecies(string version, string filter)
        {
            if (version == null) {
                // Special versioning case, if all parameters are not specified then we return an appropriate error.
                InstallerInfo.WriteEventLog("GetExtinctSpecies", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString());
                return null;
            }

            if (filter == null)
                filter = string.Empty;

            version = new Version(version).ToString(3);

            try {
                using (SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn)) {
                    myConnection.Open();

                    SqlCommand mySqlCommand = null;
                    switch (filter) {
                        case "All":
                            mySqlCommand = new SqlCommand("TerrariumGrabExtinctSpecies", myConnection);
                            break;
                        default:
                            mySqlCommand = new SqlCommand("TerrariumGrabExtinctRecentSpecies", myConnection);
                            break;
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(mySqlCommand)) {
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.Add("@Version", SqlDbType.VarChar, 255).Value = version;

                        DataSet data = new DataSet();
                        adapter.Fill(data);
                        return data;
                    }
                }
            } catch (Exception e) {
                InstallerInfo.WriteEventLog("GetExtinctSpecies", e.ToString());
                return null;
            }

        }

        /*
            Method:     GetAllSpecies
            Purpose:    Returns a dataset of all species given a
            specific version and filter criterion.
        */
        [WebMethod]
        public DataSet GetAllSpecies(string version, string filter)
        {
            if (version == null) {
                // Special versioning case, if all parameters are not specified then we return an appropriate error.
                InstallerInfo.WriteEventLog("GetAllSpecies", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString());
                return null;
            }

            // Let's verify that this version is even allowed.
            string errorMessage = "";
            PeerDiscoveryService discoveryService = new PeerDiscoveryService();
            if (discoveryService.IsVersionDisabled(version, out errorMessage) == true) {
                return null;
            }

            if (filter == null)
                filter = string.Empty;

            version = new Version(version).ToString(3);

            try {
                using (SqlConnection conn = new SqlConnection(ServerSettings.SpeciesDsn)) {
                    conn.Open();
                    String storedProcedure;

                    switch (filter) {
                        case "All":
                            storedProcedure = "TerrariumGrabAllSpecies";
                            break;
                        default:
                            storedProcedure = "TerrariumGrabAllRecentSpecies";
                            break;
                    }
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd)) {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Version", SqlDbType.VarChar, 255).Value = version;

                        DataSet data = new DataSet();
                        adapter.Fill(data);
                        return data;
                    }
                }
            } catch (Exception e) {
                InstallerInfo.WriteEventLog("GetAllSpecies", e.ToString());
                return null;
            }

        }

        /*
            Method:     GetSpeciesAssembly
            Purpose:    Gets the assembly as a byte array for the species
            with a given name and version number.
        */
        [WebMethod]
        public Byte[] GetSpeciesAssembly(string name, string version)
        {
            if (name == null || version == null) {
                // Special versioning case, if all parameters are not specified then we return an appropriate error.
                InstallerInfo.WriteEventLog("GetSpeciesAssembly", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString());
                return null;
            }

            version = new Version(version).ToString(3);

            try {
                byte[] species = LoadAssembly(version, name + ".dll");
                return species;
            } catch (Exception e) {
                InstallerInfo.WriteEventLog("GetSpeciesAssembly", e.ToString());
                return null;
            }
        }

        /*
            Method:     ReintroduceSpecies
            Purpose:    Introduces a previously extinct creature
            back into the EcoSystem and marks it as not extinct.
        */
        [WebMethod]
        public Byte[] ReintroduceSpecies(string name, string version, Guid peerGuid)
        {
            if (name == null || version == null || peerGuid == Guid.Empty) {
                // Special versioning case, if all parameters are not specified then we return an appropriate error.
                InstallerInfo.WriteEventLog("ReintroduceSpecies", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString());
                return null;
            }

            version = new Version(version).ToString(3);

            try {
                using (SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn)) {
                    myConnection.Open();
                    using (SqlTransaction transaction = myConnection.BeginTransaction()) {
                        using (SqlCommand mySqlCommand = new SqlCommand("TerrariumCheckSpeciesExtinct", myConnection, transaction)) {
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = name;

                            Object returnValue = mySqlCommand.ExecuteScalar();
                            if (Convert.IsDBNull(returnValue) || ((int) returnValue) == 0) {
                                // the species has already been reintroduced
                                transaction.Rollback();
                                return null;
                            }
                        }
                        using (SqlCommand mySqlCommand = new SqlCommand("TerrariumReintroduceSpecies", myConnection, transaction)) {
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.Add("@ReintroductionNode", SqlDbType.UniqueIdentifier, 16).Value = peerGuid;
                            mySqlCommand.Parameters.Add("@LastReintroduction", SqlDbType.DateTime, 8).Value = DateTime.UtcNow;
                            mySqlCommand.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = name;

                            mySqlCommand.ExecuteNonQuery();

                            byte[] species = LoadAssembly(version, name + ".dll");
                            transaction.Commit();
                            return species;
                        }
                    }
                }
            } catch (Exception e) {
                InstallerInfo.WriteEventLog("ReintroduceSpecies", "Species Name: " + name + "\r\n" + e.ToString());
                return null;
            }
        }

        /*
            Method:     GetBlacklistedSpecies
            Purpose:    Returns a list of all species that have been blacklisted.
        */
        [WebMethod]
        public string[] GetBlacklistedSpecies()
        {
            try {
                using (SqlConnection cnn = new SqlConnection(ServerSettings.SpeciesDsn)) {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand("Select AssemblyFullName From Species Where BlackListed = 1", cnn))
                    using (SqlDataReader dr = cmd.ExecuteReader()) {
                        List<String> blackListedSpecies = new List<String>();
                        while (dr.Read()) {
                            blackListedSpecies.Add(dr["AssemblyFullName"].ToString());
                        }

                        if (blackListedSpecies.Count > 0)
                            return blackListedSpecies.ToArray();
                    }
                }
            } catch (Exception e) {
                InstallerInfo.WriteEventLog("GetBlacklistedSpecies", e.ToString());
                return null;
            }

            return null;
        }
    }
}
