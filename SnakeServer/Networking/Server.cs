using SharedSnakeGame.Networking;
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
//            IPAddress localhost = IPAddress.Parse("145.49.59.202");
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

                    if (!CheckForPrevClient(previousClient))
                    {
                        previousClient = client;
                        continue;
                    }

                    //Eerst checken of previousClient nog leeft
                    Console.WriteLine("Found 2 clients, creating session..");
                    GameSession gameSession = new GameSession(new List<ClientHandler> { new ClientHandler(previousClient, 1), new ClientHandler(client, 2) });
                    new Thread(gameSession.Run).Start();

                    previousClient = null;
                    client = null;

                }
            }
        }

        private bool CheckForPrevClient(TcpClient prevClient)
        {
            TcpHandler.WriteMessage(prevClient, TcpProtocol.CheckForDisconnectSend());

            int waitTimeInMillis = 500;
            bool noResponsePrevClient = false;

            long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            while (prevClient.Available <= 0)
            {
                long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                if (currentTime - startTime >= waitTimeInMillis)
                {
                    noResponsePrevClient = true;
                    break;
                }
            }
            if (noResponsePrevClient)
                return false;

            TcpHandler.ReadMessage(prevClient);
            return true;
        }
    }
}
