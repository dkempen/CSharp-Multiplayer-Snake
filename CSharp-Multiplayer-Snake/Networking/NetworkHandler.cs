using CSharp_Multiplayer_Snake.Visuals;
using Newtonsoft.Json;
using SharedSnakeGame.Game;
using SharedSnakeGame.Networking;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace CSharp_Multiplayer_Snake.Networking
{
    class NetworkHandler
    {
        private Form form;
        private TcpClient client;
        private int id;
        private Draw draw;

        public const int amountOfApples = 2;
        public const int fps = 6;
        private GameData gameData;

        public NetworkHandler(Form form)
        {
            this.form = form;

            draw = Draw.GetInstance();
            draw.Form = form;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            client = new TcpClient(ip.ToString(), 6969);

            new Thread(Run).Start();
        }

        public void Run(object o)
        {
            StartGame();
            RunGame();
            EndGame();
        }

        public void StartGame()
        {
            Tuple<int, GameData> tuple = ReadId();
            id = tuple.Item1;
            gameData = tuple.Item2;
        }

        private Tuple<int, GameData> ReadId()
        {
            string received = TcpHandler.ReadMessage(client);
            dynamic json = JsonConvert.DeserializeObject<dynamic>(received);
            return Tuple.Create((int)json.id, json.data as GameData);
        }

        private GameData ReadData()
        {
            return JsonConvert.DeserializeObject<GameData>(TcpHandler.ReadMessage(client));
        }

        private bool ReadTick()
        {
            string message = TcpHandler.ReadMessage(client);
            dynamic json = JsonConvert.DeserializeObject<dynamic>(message);
            return json.newTick;
        }

        public void RunGame()
        {
            while (true)
            {
                if (!ReadTick())
                    continue;
                System.Console.WriteLine("Client1");
                SendDirection();
                System.Console.WriteLine("Client2");
                gameData = ReadData();
                System.Console.WriteLine("Client2");
                //Le draw
                Draw.GetInstance().DrawGame();
            }
        }

        private void SendDirection()
        {
            TcpHandler.WriteMessage(client, TcpProtocol.DirectionSend(GetSnake(id).Direction));
        }

        public void EndGame()
        {

        }

        public Snake GetSnake(int id)
        {
            foreach (Snake snake in gameData.Snakes)
                if (id == snake.Id)
                    return snake;
            return null;
        }

        public void KeyPressedHandler(KeyEventArgs e)
        {
            Snake snake = GetSnake(id);
            if (snake == null)
                return;
            
            // Change direction if the previous direction is not the opposite
            Point previousDirection = snake.PreviousDirection;
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
