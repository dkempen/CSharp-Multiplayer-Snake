using System;
using CSharp_Multiplayer_Snake;
using Newtonsoft.Json.Linq;
using SharedSnakeGame.Game;
using SharedSnakeGame.Networking;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SnakeServer.Networking
{
    class GameSession
    {
        private List<ClientHandler> clients;
        private GameData gameData;
        private GameLogic logic;
        private int amountOfApples = 2;
        private Timer timer;
        private readonly int fps = 6;
        private bool isRunning;

        public GameSession(List<ClientHandler> clientHandlers)
        {
            clients = clientHandlers;
        }

        public void Run(object o)
        {
            StartGame();
//            RunGameOneTick();
//            EndGame();
        }

        public void StartGame()
        {
            logic = new GameLogic();

            gameData = new GameData(16);
            gameData.Snakes.Add(new Snake(new Point((int)(gameData.GridSize * 0.25), (int)(gameData.GridSize * 0.75)), 
                Snake.Directions.Right, Color.ForestGreen, 1));
            gameData.Snakes.Add(new Snake(new Point((int)(gameData.GridSize * 0.75), (int)(gameData.GridSize * 0.25)), 
                Snake.Directions.Left, Color.DeepSkyBlue, 2));
            logic.AddApples(gameData, amountOfApples);

            foreach (ClientHandler client in clients)
                client.Write(TcpProtocol.IdSend(gameData, client.Id));

            // Start timer that calls a game tick update
            timer = new Timer();
            timer.Elapsed += OnTimedEvent;
            timer.Interval = 1000.0 / fps;

            RunGameOneTick();
            // Wait 3 seconds
            Thread.Sleep(3000);

            timer.Start();
        }

        public void RunGameOneTick()
        {
            isRunning = true;

            // Ask all clients for their snakes' direction
            Broadcast(TcpProtocol.TickSend());
            Console.WriteLine("Send Tick");

            // Read all client directions
            ReadAll();

            //Logique

            // For each snake check if next move it eats an apple and then update the snake
            foreach (Snake snake in GetAliveSnakes())
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
            foreach (Snake snake in GetAliveSnakes())
                if (logic.CheckForDeath(GetAliveSnakes(), snake, gameData.GridSize))
                    snake.IsDead = true;

            // Check for end game
            if (GameHasEnded())
            {
                EndGame();
                return;
            }

            // Remove all dead snakes
//            for (int i = gameData.Snakes.Count - 1; i >= 0; i--)
//                if (gameData.Snakes[i].IsDead)
//                    gameData.Snakes.Remove(gameData.Snakes[i]);

            // Add the apples back that have been eaten
            logic.AddApples(gameData, amountOfApples);

            // Send updated gameData to the clients
            Broadcast(TcpProtocol.DataSend(gameData));

            isRunning = false;
        }

        private bool GameHasEnded()
        {
            return GetAliveSnakes().Count <= 0;
        }

        public void EndGame()
        {
            // End game logic
        }

        public void Broadcast(JObject message)
        {
            foreach (ClientHandler client in clients)
                client.Write(message);
        }

        public void ReadAll()
        {
            foreach (ClientHandler client in clients)
            {
                Snake snake = GetSnake(client.Id);
                snake.Direction = TcpProtocol.DirectionReceived(client.Read());
                snake.PreviousDirection = snake.Direction;
            }
        }

        public Snake GetSnake(int id)
        {
            foreach (Snake snake in gameData.Snakes)
                if (id == snake.Id)
                    return snake;
            return null;
        }

        public List<Snake> GetAliveSnakes()
        {
            List<Snake> snakes = new List<Snake>();
            foreach (Snake snake in gameData.Snakes)
                if (!snake.IsDead)
                    snakes.Add(snake);
            return snakes;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (isRunning)
                return;
            RunGameOneTick();
        }
    }
}
