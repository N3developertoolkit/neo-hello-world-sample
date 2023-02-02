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
        const byte Prefix_SampleValue = 0x00;
        const byte Prefix_ContractOwner = 0xFF;

        [Safe]
        public static ByteString Get()
        {
            var context = Storage.CurrentContext;
            var key = new byte[] { Prefix_SampleValue };
            return Storage.Get(context, key);
        }

        public static void Put(ByteString value)
        {
            var context = Storage.CurrentContext;
            var key = new byte[] { Prefix_SampleValue };
            Storage.Put(context, key, value);
        }

        public static void Delete()
        {
            var context = Storage.CurrentContext;
            var key = new byte[] { Prefix_SampleValue };
            Storage.Delete(context, key);
        }

        [DisplayName("_deploy")]
        public static void Deploy(object _, bool update)
        {
            if (update) return;

            var tx = (Transaction)Runtime.ScriptContainer;
            var key = new byte[] { Prefix_ContractOwner };
            Storage.Put(Storage.CurrentContext, key, tx.Sender);
        }

        public static void Update(ByteString nefFile, string manifest)
        {
            var key = new byte[] { Prefix_ContractOwner };
            var contractOwner = (UInt160)Storage.Get(Storage.CurrentContext, key);
            var tx = (Transaction)Runtime.ScriptContainer;

            if (!contractOwner.Equals(tx.Sender))
            {
                throw new Exception("Only the contract owner can update the contract");
            }

            ContractManagement.Update(nefFile, manifest, null);
        }
    }
}
