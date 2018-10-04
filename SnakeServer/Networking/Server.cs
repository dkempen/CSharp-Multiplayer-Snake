using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SnakeServer.Networking
{
    class Server
    {
        private TcpListener tcpListener;

        public Server()
        {
            IPAddress localhost = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(localhost, 6963);
            tcpListener.Start();

            TcpClient previousClient = null;

            while (true)
            {
                Console.WriteLine("Waiting for client..");
                TcpClient client = tcpListener.AcceptTcpClient();
                
                if (previousClient == null)
                {
                    previousClient = client;
                    Console.WriteLine("Found one client");
                }
                else
                {
                    Console.WriteLine("Found 2 clients, creating session..");
                    GameSession gameSession = new GameSession(new List<ClientHandler> {new ClientHandler(previousClient, 1), new ClientHandler(client, 2) });
                    new Thread(gameSession.Run).Start();

                    previousClient = null;
                }
            }
        }
    }
}
