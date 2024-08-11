using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatClient
{
    internal class Program
    {
        private const string ServerAddress = "localhost";
        private const int ServerPort = 12345;

        static async Task Main(string[] args)
        {
            using (var client = new TcpClient(ServerAddress, ServerPort))
            {
                var cancellationTokenSource = new CancellationTokenSource();

                // Start the task for receiving messages
                var receiveTask = Task.Run(() => ReceiveMessagesAsync(client, cancellationTokenSource.Token));

                // Start the task for sending messages
                var sendTask = Task.Run(() => SendMessagesAsync(client, cancellationTokenSource.Token));

                await Task.WhenAll(receiveTask, sendTask);
            }
        }

        private static async Task ReceiveMessagesAsync(TcpClient client, CancellationToken cancellationToken)
        {
            using (var stream = client.GetStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    try
                    {
                        string message;
                        while ((message = await reader.ReadLineAsync()) != null)
                        {
                            Console.WriteLine("\r" + message);
                            Console.Write("You: ");
                        }
                    }
                    catch (IOException)
                    {
                        // Handle disconnection
                    }
                }
            }
        }

        private static async Task SendMessagesAsync(TcpClient client, CancellationToken cancellationToken)
        {
            using (var stream = client.GetStream())
            {
                using (var writer = new StreamWriter(stream) { AutoFlush = true })
                {
                    try
                    {
                        string userName = Console.ReadLine();
                        await writer.WriteLineAsync(userName);

                        string userInput;
                        while ((userInput = Console.ReadLine()) != null)
                        {
                            await writer.WriteLineAsync(userInput);
                            Console.Write("You: ");
                        }
                    }
                    catch (IOException)
                    {
                        // Handle disconnection
                    }
                }
            }
        }
    }
}
