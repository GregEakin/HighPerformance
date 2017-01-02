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
    public class MessageServerDispatcher : IDisposable
    {
        private readonly CancellationToken _cancellationToken;
        private readonly MessageServer _server;
        private readonly TcpClient _client;
        private readonly NetworkStream _networkStream;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;

        public MessageServerDispatcher(MessageServer server, TcpClient client, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _server = server;
            _client = client;

            _networkStream = client.GetStream();
            _reader = new StreamReader(_networkStream);
            _writer = new StreamWriter(_networkStream) { AutoFlush = true };
        }

        public void Dispose()
        {
            ((IDisposable)_client)?.Dispose();
            _networkStream?.Dispose();
            _reader?.Dispose();
            _writer?.Dispose();
        }

        public async Task RunAsync()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                var request = await Task.Run(() => _reader.ReadLineAsync(), _cancellationToken);
                // var request = await _reader.ReadLineAsync();
                if (request == null)
                    break;

                Console.WriteLine("Received service request: " + request);

                var message = new Message();
                message.Decode(request);

                if (message.Type == 0 && message["disconnect"] != null)
                {
                    var ack = new Message();
                    var mm = ack.Encode();
                    await _writer.WriteLineAsync(mm);
                    break;
                }

                var deliverable = _server.GetSubscriber(message.Type);
                var result = deliverable != null ? deliverable.Send(message) : new Message();
                var response = result.Encode();
                await _writer.WriteLineAsync(response);
            }
        }
    }
}