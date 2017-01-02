// Greg Eakin
// December 31, 2016
// Copyright (c) 2016

//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Library General Public
//License as published by the Free Software Foundation; either
//version 2 of the License, or(at your option) any later version.

//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU
//Library General Public License for more details.

//You should have received a copy of the GNU Library General Public
//License along with this library; if not, write to the
//Free Software Foundation, Inc., 59 Temple Place - Suite 330,
//Boston, MA  02111-1307, USA.

using System;
using System.Threading;
using System.Threading.Tasks;
using HighPerformance.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Messaging
{
    [TestClass]
    public class MessageServerTests
    {
        private class Server1 : IDeliverable
        {
            public Message Send(Message m)
            {
                return m;
            }
        }

        [Ignore]
        [TestMethod]
        public void Test1()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var server = new MessageServer(6000, cancellationTokenSource.Token);
            server.Subscribe(0x01, new Server1());

            var runner = server.RunAsync();

            var host = MessageServer.Address.ToString();
            var client = new MessageClient(host, 6000);

            var msg = new Message {Type = 0x01};
            var response = client.SendAsync(msg);
            var data = response.Result;
            Console.WriteLine($"Send: {data}");

            var disconnect = client.DisconnectAsync();
            var x = disconnect.Result;
            Console.WriteLine($"disconnect: {x}");

            var delay = Task.Delay(500, cancellationTokenSource.Token);
            delay.Wait();

            cancellationTokenSource.Cancel();
            runner.Wait();
        }

        [TestMethod]
        public void AddressTest()
        {
            var address = MessageServer.Address;
            Assert.AreEqual<byte>(192, address.GetAddressBytes()[0]);
            Assert.AreEqual<byte>(168, address.GetAddressBytes()[1]);
            Assert.AreEqual<byte>(42, address.GetAddressBytes()[2]);
        }
    }
}