using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using CSharp_Multiplayer_Snake.Visuals;
using Timer = System.Timers.Timer;

namespace CSharp_Multiplayer_Snake
{
    class GameLoop
    {
        public const int gridSize = 16;
        private readonly Color snake1Color = Color.Ivory;
        private readonly Color snake2Color = Color.BurlyWood;

        private Draw draw;
        public Form Form { get; }
        public List<Snake> Snakes { get; }
        public List<Apple> Apples { get; }

        private Timer timer;

        public GameLoop(Form form)
        {
            Form = form;
            draw = Draw.GetInstance();
            draw.GameLoop = this;
            Snakes = new List<Snake>();
            Snakes.Add(new Snake(new Point(gridSize / 2, gridSize / 2), snake1Color));
            Apples = new List<Apple>();
            Apples.Add(new Apple(Snakes, gridSize));
            timer = new Timer(200);
            timer.Elapsed += TimeElapsed;
            timer.Start();
            Loop();
        }

        private void Loop()
        {
            foreach (Snake snake in Snakes)
            {
                bool hasEaten = false;
                foreach (Apple apple in Apples)
                {
                    if (apple.IsOnHead(snake.GetUpdatedHeadPosition()))
                    {
                        hasEaten = true;
                        Apples.Remove(apple);
                        break;
                    }
                }
                snake.UpdateBody(hasEaten);
            }

            // Check if an apple has been eaten and has to be respawned
            if (Apples.Count <= 0)
                Apples.Add(new Apple(Snakes, gridSize));
            draw.DrawGame();
        }

        public void KeyPressedHandler(KeyEventArgs e)
        {
            Snake snake = Snakes[0];
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

        private void TimeElapsed(object sender, ElapsedEventArgs e)
        {
            Loop();
        }
    }
}
