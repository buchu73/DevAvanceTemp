using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ChatApp.ChatServer
{
    internal class Program
    {
        private const int Port = 12345;
        private static readonly ConcurrentBag<ClientHandler> ClientHandlers = new ConcurrentBag<ClientHandler>();

        private static SharedRessource SharedRessource = new SharedRessource();

        static void Main(string[] args)
        {
            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine("Chat server started on port " + Port);

            while (true)
            {
                var client = listener.AcceptTcpClient();
                var clientHandler = new ClientHandler(client, SharedRessource);
                ClientHandlers.Add(clientHandler);
                Task.Run(() => clientHandler.HandleClientAsync());
            }
        }

        public static void Broadcast(string message, ClientHandler sender)
        {
            foreach (var clientHandler in ClientHandlers)
            {
                if (clientHandler != sender)
                {
                    clientHandler.SendMessage(message);
                }
            }
        }

        public static void RemoveClient(ClientHandler clientHandler)
        {
            ClientHandlers.TryTake(out _);
        }
    }
}
