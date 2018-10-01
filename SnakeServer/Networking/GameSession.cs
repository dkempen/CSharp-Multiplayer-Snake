using CSharp_Multiplayer_Snake;
using Newtonsoft.Json.Linq;
using SharedSnakeGame.Game;
using SharedSnakeGame.Networking;
using System.Collections.Generic;
using System.Drawing;

namespace SnakeServer.Networking
{
    class GameSession
    {
        private List<ClientHandler> clients;
        private GameData gameData;
        private GameLogic logic;
        private int amountOfApples = 2;

        public GameSession(List<ClientHandler> clientHandlers)
        {
            this.clients = clientHandlers;
        }

        public void Run(object o)
        {
            StartGame();
            RunGame();
            EndGame();
        }

        public void StartGame()
        {
            logic = new GameLogic();

            gameData = new GameData(16);
            gameData.Snakes.Add(new Snake(new Point(gameData.GridSize / 2, gameData.GridSize / 2), Color.Ivory, 1));
            gameData.Snakes.Add(new Snake(new Point(gameData.GridSize / 2, gameData.GridSize / 2), Color.Beige, 2));
            logic.AddApples(gameData, amountOfApples);
            

            foreach (ClientHandler client in clients)
            {
                client.Write(TcpProtocol.IdSend(gameData, client.Id));
            }

        }

        public void RunGame()
        {
            bool isRunning = true;

            while (isRunning)
            {
                System.Console.WriteLine("Thicc");
                Broadcast(TcpProtocol.TickSend());
                System.Console.WriteLine("Thicc2");
                ReadAll();
                //Logique
                System.Console.WriteLine("Thicc3");
                Broadcast(TcpProtocol.DataSend(gameData));

                System.Console.WriteLine("Thicc4");

            }
        }

        public void EndGame()
        {

        }

        public void Broadcast(JObject message)
        {
            foreach (ClientHandler client in clients)
            {
                client.Write(message);
            }

        }

        public void ReadAll()
        {
            foreach(ClientHandler client in clients)
            {
                GetSnake(client.Id).Direction = TcpProtocol.DirectionReceived(client.Read());
            }
        }

        public Snake GetSnake(int id)
        {
            foreach(Snake snake in gameData.Snakes)
            {
                if (id == snake.Id)
                    return snake;
            }
            return null;
        }
    }
}
