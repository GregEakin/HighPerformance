// High Performance C# Multithreading Programming
// Copyright(C) 2017  Greg Eakin

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HighPerformance.Messaging
{
    /// <summary>
    /// C# version Inspaired by the Simple Messaging Architecture, from
    /// Christopher, Thomas W., and George Thiruvathukal. "Chapter 11: Networking." 
    /// High Performance Java Computing: Multi-threaded and Networked Programming. 
    /// Hemel Hempstead: Prentice Hall, 2000. 241-55. Print.
    /// </summary>
    public class MessageServer
    {
        private readonly Dictionary<int, IDeliverable> _subscribers = new Dictionary<int, IDeliverable>();
        private readonly TcpListener _listener;
        private readonly CancellationToken _cancellationToken;

        public MessageServer(int port, CancellationToken cancellationToken)
        {
            Port = port;
            _cancellationToken = cancellationToken;
            var address = Address;
            _listener = new TcpListener(address, port);
        }

        public int Port { get; }

        public void Subscribe(int messageType, IDeliverable deliverable)
        {
            _subscribers[messageType] = deliverable;
        }

        public IDeliverable GetSubscriber(int messageType)
        {
            return _subscribers[messageType];
        }

        public async Task RunAsync()
        {
            var dispatchers = new List<MessageServerDispatcher>();
            _listener.Start();
            try
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    var tcpClient = await Task.Run(() => _listener.AcceptTcpClientAsync(), _cancellationToken);
                    //var tcpClient = await _listener.AcceptTcpClientAsync();
                    var dispatcher = new MessageServerDispatcher(this, tcpClient, _cancellationToken);
                    dispatchers.Add(dispatcher);
                    await dispatcher.RunAsync();
                }
            }
            finally
            {
                _listener.Stop();
            }

            foreach (var messageServerDispatcher in dispatchers)
                messageServerDispatcher.Dispose();
        }

        public static IPAddress Address
        {
            get
            {
                var hostName = Dns.GetHostName();
                var ipHostInfo = Dns.GetHostEntry(hostName);
                var ipAddress = ipHostInfo.AddressList.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork);
                if (ipAddress == null)
                    throw new Exception("No IPv4 address for server");
                return ipAddress;
            }
        }
    }
}