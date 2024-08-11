using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientsPool
{
    internal class Program
    {
        private const string ServerAddress = "localhost";
        private const int ServerPort = 12345;
        private const int NumberOfClients = 3;
        private const int MessageIntervalMs = 200;

        public static void Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var tasks = new Task[NumberOfClients];

            for (int i = 1; i <= NumberOfClients; i++)
            {
                int clientId = i;
                tasks[clientId - 1] = Task.Run(async () =>
                {
                    using (var client = new TcpClient(ServerAddress, ServerPort))
                    {
                        using (var stream = client.GetStream())
                        {
                            using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                            {
                                using (var reader = new StreamReader(stream, Encoding.UTF8))
                                {

                                    string clientName = "Client-" + clientId;
                                    await writer.WriteLineAsync(clientName);

                                    // Start a task to listen for incoming messages
                                    var receiveTask = Task.Run(async () =>
                                    {
                                        string serverMessage;
                                        while ((serverMessage = await reader.ReadLineAsync()) != null)
                                        {
                                            Console.WriteLine(clientName + " received: " + serverMessage);
                                        }
                                    });

                                    // Periodically send messages with a timestamp
                                    while (true)
                                    {
                                        string message = $"Message from {clientName} at {DateTime.Now:HH:mm:ss}";
                                        await writer.WriteLineAsync(message);
                                        await Task.Delay(MessageIntervalMs);
                                    }
                                }
                            }
                        }
                    }
                }, cancellationTokenSource.Token);
            }

            Task.WaitAll(tasks);
        }
    }
}
