using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HighPerformance.Messaging
{
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