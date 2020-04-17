using System;
using System.Data;
using Terrarium.Server.Species;

namespace Terrarium.Server.Abstract
{
    interface ISpeciesService
    {
        SpeciesServiceStatus Add(string name, string version, string type, string author, string email, string assemblyFullName, byte[] assemblyCode);
        DataSet GetAllSpecies(string version, string filter);
        string[] GetBlacklistedSpecies();
        DataSet GetExtinctSpecies(string version, string filter);
        byte[] GetSpeciesAssembly(string name, string version);
        byte[] LoadAssembly(string version, string assemblyFileName);
        byte[] ReintroduceSpecies(string name, string version, Guid peerGuid);
        void RemoveAssembly(string version, string assemblyFileName);
        void SaveAssembly(byte[] assemblyCode, string version, string assemblyFileName);
    }
}
