using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedSnakeGame.Networking;
using System.Net.Sockets;

namespace SnakeServer.Networking
{
    class ClientHandler
    {
        private TcpClient client;
        public int Id { get; }
        public string Name { get; set; }

        public ClientHandler(TcpClient client, int id)
        {
            this.client = client;
            Id = id;
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
