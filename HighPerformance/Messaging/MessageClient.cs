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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HighPerformance.Messaging
{
    /// <summary>
    /// C# version Inspaired by the Simple Messaging Architecture, from
    /// Christopher, Thomas W., and George Thiruvathukal. "Chapter 11: Networking." 
    /// High Performance Java Computing: Multi-threaded and Networked Programming. 
    /// Hemel Hempstead: Prentice Hall, 2000. 241-55. Print.
    /// </summary>
    public class MessageClient : IDisposable
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _networkStream;
        private readonly StreamWriter _writer;
        private readonly StreamReader _reader;

        public MessageClient(string host, int port)
        {
            _client = new TcpClient();

            var ipAddress = GetAddress(host);
            _client.Connect(ipAddress, port);

            _networkStream = _client.GetStream();
            _writer = new StreamWriter(_networkStream);
            _reader = new StreamReader(_networkStream);
            _writer.AutoFlush = true;
        }

        public async Task<Message> SendAsync(Message message)
        {
            try
            {
                var data = message.Encode();
                await _writer.WriteLineAsync(data);
                var response = await _reader.ReadLineAsync();
                var msg = new Message();
                msg.Decode(response);
                return msg;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Message> DisconnectAsync()
        {
            var message = new Message
            {
                Type = 0,
                ["disconnect"] = "EOL"
            };
            var response = await SendAsync(message);
            return response;
        }

        public static IPAddress GetAddress(string server)
        {
            var ipHostInfo = Dns.GetHostEntry(server);
            var ipAddress = ipHostInfo.AddressList.FirstOrDefault(t => t.AddressFamily == AddressFamily.InterNetwork);
            if (ipAddress == null)
                throw new Exception("No IPv4 address for server");
            return ipAddress;
        }

        public void Dispose()
        {
            _networkStream?.Dispose();
            _writer?.Dispose();
            _reader?.Dispose();
            ((IDisposable)_client)?.Dispose();
        }
    }
}