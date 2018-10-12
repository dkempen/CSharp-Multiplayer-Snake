using CSharp_Multiplayer_Snake.Visuals;
using Newtonsoft.Json;
using SharedSnakeGame.Game;
using SharedSnakeGame.Networking;
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Linq;
using SharedSnakeGame.Data;

namespace CSharp_Multiplayer_Snake.Networking
{
    class NetworkHandler
    {
        private Form form;
        private TcpClient client;
        private int id;
        private Draw draw;
        public bool Disconnected { get; set; }
        private GameData gameData;
        private bool waitDirectionChange;

        public NetworkHandler(Form form)
        {
            this.form = form;
            draw = Draw.GetInstance();
            draw.Form = form;
            draw.GameData = gameData;
            new Thread(Run).Start();
        }

        public void Run(object o)
        {
            while (!Disconnected)
            {
                StartGame();
                RunGame();
            }
        }

        public void StartGame()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
//            IPAddress ip = IPAddress.Parse("145.49.59.202");
            client = new TcpClient(ip.ToString(), 6963);

            while (client.Available <= 0)
            {
                if (Disconnected)
                {
                    Console.WriteLine("");
                    return;
                }
            }

            string received = TcpHandler.ReadMessage(client);
            JObject jObject = JObject.Parse(received);
            string command = (string)jObject["command"];
            Tuple<int, GameData> tuple;

            if (command != "disconnect/send")
                tuple = Tuple.Create((int)jObject["id"], JsonConvert.DeserializeObject<GameData>((string)jObject["data"]));
            else
            {
                TcpHandler.WriteMessage(client, new JObject());
                tuple = ReadId();
            }
            SendName();

            id = tuple.Item1;
            Disconnected = false;
            gameData = tuple.Item2;
            draw.GameData = gameData;
        }

        private Tuple<int, GameData> ReadId()
        {
            string received = TcpHandler.ReadMessage(client);
            JObject jObject = JObject.Parse(received);
            string data = (string)jObject["data"];
            GameData gameData = JsonConvert.DeserializeObject<GameData>(data);
            return Tuple.Create((int)jObject["id"], gameData);
        }

        private GameData ReadData()
        {
            string received = TcpHandler.ReadMessage(client);
            JObject jObject = JObject.Parse(received);
            string data = (string)jObject["data"];
            GameData gameData = JsonConvert.DeserializeObject<GameData>(data);
            // Fix bodies because for some reason it parses with an extra 0,0 coord in the beginning
            foreach (Snake snake in gameData.Snakes)
                snake.Body.RemoveFirst();
            return gameData;
        }

        private Highscore ReadHighscore()
        {
            string received = TcpHandler.ReadMessage(client);
            string data = JObject.Parse(received)["data"].ToString();
            Highscore highscore = JsonConvert.DeserializeObject<Highscore>(data);
            return highscore;
        }

        private bool ReadEndGame()
        {
            string message = TcpHandler.ReadMessage(client);
            JObject jObject = JObject.Parse(message);
            string command = (string)jObject["command"];
            switch (command)
            {
                case "tick/send":
                    return false;
                case "end/send":
                    return true;
            }
            return false;
        }

        public void RunGame()
        {
            while (true)
            {
                if (Disconnected)
                {
                    client.Close();
                    return;
                }

                // Check for end game or new tick
                if (ReadEndGame())
                {
                    EndGame();
                    return;
                }

                // Send direction
                SendDirection();
                waitDirectionChange = true;
                gameData = ReadData();
                draw.GameData = gameData;
                waitDirectionChange = false;

                //Le draw
                Draw.GetInstance().DrawGame();
            }
        }

        private void SendDirection()
        {
            TcpHandler.WriteMessage(client, TcpProtocol.DirectionSend(GetSnake(id).Direction));
        }

        private void SendName()
        {
            TcpHandler.WriteMessage(client, TcpProtocol.NameSend(form.Name, form.Color));
        }

        public void EndGame()
        {
            // Recieve highscores
            Highscore highscore = ReadHighscore();

            // Rejoin a lobby
            int score = GetSnake(id).Body.Count;
            Form.ShowHighScoreDialog(highscore, score);
            client.Close();
            draw.GameData = null;
            draw.DrawGame();
        }

        public Snake GetSnake(int id)
        {
            var list = gameData.Snakes;
            var queryable = list.AsQueryable();

            queryable =
                from snake in queryable
                where snake.Id == id
                select snake;

            Snake selectedSnake = queryable.First();
            return selectedSnake;
        }

        public void KeyPressedHandler(KeyEventArgs e)
        {
            if (gameData == null)
                return;
            while (waitDirectionChange) { }
            Snake snake = GetSnake(id);
            if (snake == null)
                return;

            // Only appect the first new direction
            Point previousDirection = snake.PreviousDirection;
            if (previousDirection != snake.Direction)
                return;

            // Change direction if the previous direction is not the opposite
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.Up:
                    if (previousDirection.Equals(Snake.Directions.Down))
                        break;
                    snake.Direction = Snake.Directions.Up;
                    break;
                case Keys.S:
                case Keys.Down:
                    if (previousDirection.Equals(Snake.Directions.Up))
                        break;
                    snake.Direction = Snake.Directions.Down;
                    break;
                case Keys.A:
                case Keys.Left:
                    if (previousDirection.Equals(Snake.Directions.Right))
                        break;
                    snake.Direction = Snake.Directions.Left;
                    break;
                case Keys.D:
                case Keys.Right:
                    if (previousDirection.Equals(Snake.Directions.Left))
                        break;
                    snake.Direction = Snake.Directions.Right;
                    break;
            }
        }
    }
}
