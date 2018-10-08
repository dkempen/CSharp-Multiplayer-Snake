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
using System.Threading.Tasks;
using SharedSnakeGame.Data;

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

            // Add the apples back that have been eaten
            logic.AddApples(gameData, amountOfApples);

            // Send updated gameData to the clients
            Broadcast(TcpProtocol.DataSend(gameData));

            // Check for end game
            if (GameHasEnded())
            {
                EndGame();
                return;
            }

            isRunning = false;
        }

        private bool GameHasEnded()
        {
            return GetAliveSnakes().Count <= 0;
        }

        public void EndGame()
        {
            // Stop the game timer
            timer.Stop();

            // Get and send previous highscores
            //            Highscore highscore = Highscore.ReadHighScores();
            //            Broadcast(TcpProtocol.HighscoresSend(highscore));

            // Read all names

            // Update highscores
            Highscore highscore = new Highscore();
            highscore.CheckScores(gameData.Snakes[0].Body.Count, "DVK");
            highscore.PrintHighScores();
            highscore.WriteHighScores();
            Highscore newHighscore = Highscore.ReadHighScores();
            newHighscore.PrintHighScores();

            // Send Endpacket and stop timer
            Broadcast(TcpProtocol.EndSend());
        }

        public void Broadcast(JObject message)
        {
            foreach (ClientHandler client in clients)
                client.Write(message);
        }

        public async Task ReadAll()
        {
            Task[] tasks = new Task[clients.Count];
            for (int i = 0; i < tasks.Length; i++)
                tasks[i] = ReadClient(clients[i]);
            await Task.WhenAll(tasks);
        }

        private Task ReadClient(ClientHandler client)
        {
            JObject jObject = client.Read();
            lock (client)
            {
                Snake snake = GetSnake(client.Id);
                snake.Direction = TcpProtocol.DirectionReceived(jObject);
                snake.PreviousDirection = snake.Direction;
            }
            return Task.FromResult<object>(null);
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
