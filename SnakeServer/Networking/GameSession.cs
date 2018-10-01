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
            clients = clientHandlers;
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
                client.Write(TcpProtocol.IdSend(gameData, client.Id));
        }

        public void RunGame()
        {
            bool isRunning = true;

            while (isRunning)
            {
                // Ask all clients for their snakes' direction
                Broadcast(TcpProtocol.TickSend());

                // Read all client directions
                ReadAll();

                //Logique
                
                // For each snake check if next move it eats an apple and then update the snake
                foreach (Snake snake in gameData.Snakes)
                {
                    bool hasEaten = false;
                    for (int i = gameData.Apples.Count - 1; i >= 0; i--)
                    {
                        if (logic.EatsAnApple(snake, gameData.Apples[i]))
                        {
                            gameData.Apples.Remove(gameData.Apples[i]);
                            hasEaten = true;
                            break;
                        }
                    }
                    snake.UpdateBody(hasEaten);
                }

                // For each snake check if it's dead
                foreach (Snake snake in gameData.Snakes)
                    if (logic.CheckForDeath(gameData.Snakes, snake, gameData.GridSize))
                        snake.IsDead = true;

                // Remove all dead snakes
                for (int i = gameData.Snakes.Count - 1; i >= 0; i--)
                    if (gameData.Snakes[i].IsDead)
                        gameData.Snakes.Remove(gameData.Snakes[i]);

                // Add the apples back that have been eaten
                logic.AddApples(gameData, amountOfApples);
                
                // Send updated gameData to the clients
                Broadcast(TcpProtocol.DataSend(gameData));
            }
        }

        public void EndGame()
        {

        }

        public void Broadcast(JObject message)
        {
            foreach (ClientHandler client in clients)
                client.Write(message);
        }

        public void ReadAll()
        {
            foreach (ClientHandler client in clients)
                GetSnake(client.Id).Direction = TcpProtocol.DirectionReceived(client.Read());
        }

        public Snake GetSnake(int id)
        {
            foreach (Snake snake in gameData.Snakes)
                if (id == snake.Id)
                    return snake;
            return null;
        }
    }
}
