using System;
using System.ComponentModel;
using Neo;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Attributes;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;

namespace DevHawk.SampleContracts
{
    [DisplayName("HelloWorld")]
    [ManifestExtra("Author", "Harry Pierson")]
    [ManifestExtra("Email", "harrypierson@hotmail.com")]
    [ManifestExtra("Description", "This is an example contract")]
    public class HelloWorldContract : SmartContract
    {
        // using strings for keys to avoid insertion of CONVERT instruction 
        // https://github.com/neo-project/neo-devpack-dotnet/issues/777
        const string Prefix_SampleValue = "S";
        const string Prefix_ContractOwner = "O";

        [Safe]
        public static ByteString Get()
        {
            return Storage.Get(Storage.CurrentContext, Prefix_SampleValue);
        }

        public static void Put(ByteString value)
        {
            Storage.Put(Storage.CurrentContext, Prefix_SampleValue, value);
        }

        public static void Delete()
        {
            Storage.Delete(Storage.CurrentContext, Prefix_SampleValue);
        }

        [DisplayName("_deploy")]
        public static void Deploy(object _, bool update)
        {
            if (update) return;

            var tx = (Transaction)Runtime.ScriptContainer;
            Storage.Put(Storage.CurrentContext, Prefix_ContractOwner, tx.Sender);
        }

        public static void Update(ByteString nefFile, string manifest)
        {
            var contractOwner = (UInt160)Storage.Get(Storage.CurrentContext, Prefix_ContractOwner);
            if (!Runtime.CheckWitness(contractOwner))
            {
                throw new Exception("Only the contract owner can update the contract");
            }
            ContractManagement.Update(nefFile, manifest, null);
        }
    }
}
