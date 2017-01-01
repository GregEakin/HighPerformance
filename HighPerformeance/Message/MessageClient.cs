using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HighPerformanceTests.Message
{
    public class MessageClient : IDisposable
    {
        private readonly NetworkStream _networkStream;
        private readonly StreamWriter _writer;
        private readonly StreamReader _reader;

        private readonly TcpClient _client;

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

        public async Task<Message> CallAsync(Message message)
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
            var message = new Message {Type = 0};
            message.Type = 0;
            message["disconnect"] = "EOL";
            var response = await CallAsync(message);
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