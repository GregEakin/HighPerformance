﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HighPerformance.Messaging
{
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