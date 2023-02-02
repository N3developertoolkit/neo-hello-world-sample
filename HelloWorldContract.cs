using System.ComponentModel;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Attributes;
using Neo.SmartContract.Framework.Services;

namespace DevHawk.SampleContracts
{
    public class HelloWorldContract : SmartContract
    {
        const byte Prefix_ContractOwner = 0xFF;

        [Safe]
        public static string SayHello(string name)
        {
            return $"Hello, {name}!";
        }

        [DisplayName("_deploy")]
        public void Deploy(object data, bool update)
        {
            if (update) return;

            var tx = (Transaction)Runtime.ScriptContainer;
            var key = new byte[] { Prefix_ContractOwner };
            Storage.Put(Storage.CurrentContext, key, tx.Sender);
        }
    }
}
