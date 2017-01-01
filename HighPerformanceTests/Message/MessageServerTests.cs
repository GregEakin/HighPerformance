using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Message
{
    [TestClass]
    public class MessageServerTests
    {
        class Server1 : IDeliverable
        {
            public Message Send(Message m)
            {
                return m;
            }
        }

        [TestMethod]
        public void Test1()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var server = new MessageServer(6000, cancellationTokenSource.Token);
            server.Subscribe(0x01, new Server1());

            var runner = server.RunAsync();

            var host = MessageServer.Address.ToString();
            using (var client = new MessageClient(host, 6000))
            {
                var msg = new Message {Type = 0x01};
                var response = client.CallAsync(msg);
                var data = response.Result;
                Console.WriteLine(data);

                var disconnect = client.DisconnectAsync();
                var x = disconnect.Result;
            }

            cancellationTokenSource.Cancel();
            runner.Wait();

            runner.Dispose();
        }

        [TestMethod]
        public void AddressTest()
        {
            var address = MessageServer.Address;
            Assert.AreEqual(192, address.GetAddressBytes()[0]);
            Assert.AreEqual(168, address.GetAddressBytes()[1]);
        }
    }
}