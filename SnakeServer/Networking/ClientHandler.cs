using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedSnakeGame.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeServer.Networking
{
    class ClientHandler
    {
        private TcpClient client;
        public int Id { get; }

        public ClientHandler(TcpClient client, int id)
        {
            this.client = client;
            this.Id = id;

        }

        public void Write(JObject message)
        {
            TcpHandler.WriteMessage(client, message);
        }

        public JObject Read()
        {
            return JsonConvert.DeserializeObject<dynamic>(TcpHandler.ReadMessage(client));
        }


        
    }
}
