using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.ChatServer
{
    public class ClientHandler
    {
        private TcpClient _client;
        private StreamWriter _writer;
        private string _userName;
        private int _numberOfMessagesReceived = 0;

        private SharedRessource _sharedRessource;

        public ClientHandler(TcpClient client, SharedRessource sharedRessource)
        {
            _client = client;
            _sharedRessource = sharedRessource;
        }

        public async Task HandleClientAsync()
        {
            using (var stream = _client.GetStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    _writer = new StreamWriter(stream) { AutoFlush = true };

                    // Read user name
                    await _writer.WriteLineAsync("Enter your name: ");
                    _userName = await reader.ReadLineAsync();
                    Console.WriteLine(_userName + " has joined the chat.");
                    Broadcast(_userName + " has joined the chat.");

                    try
                    {
                        string message;
                        while ((message = await reader.ReadLineAsync()) != null)
                        {
                            Console.WriteLine(_userName + ": " + message);
                            _numberOfMessagesReceived++;

                            _sharedRessource.Increment();

                            Broadcast(_userName + ": " + message);
                        }
                    }
                    catch (IOException)
                    {
                        // Handle disconnection
                    }
                    finally
                    {
                        Program.RemoveClient(this);
                        Console.WriteLine(_userName + " has left the chat. Number of messages received: " + _numberOfMessagesReceived);
                        Broadcast(_userName + " has left the chat.");
                    }
                }
            };

            Program.RemoveClient(this);
        }

        public void SendMessage(string message)
        {
            if (_writer != null)
            {
                _writer.WriteLine(message);
            }
        }

        private void Broadcast(string message)
        {
            Program.Broadcast(message, this);
        }
    }
}
