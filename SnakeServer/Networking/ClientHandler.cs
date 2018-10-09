using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedSnakeGame.Networking;
using System;
using System.Net.Sockets;

namespace SnakeServer.Networking
{
    class ClientHandler
    {
        private TcpClient client;
        public int Id { get; }
        public bool Disconnected { get; set; }
        public string Name { get; set; }

        public ClientHandler(TcpClient client, int id)
        {
            this.client = client;
            Id = id;
            Disconnected = false;
        }

        public void Write(JObject message)
        {
            TcpHandler.WriteMessage(client, message);
        }

        public JObject Read()
        {
            try
            {
                return JsonConvert.DeserializeObject<dynamic>(TcpHandler.ReadMessage(client));
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception");
                Disconnected = true;
                return null;
            }
        }
    }
}
