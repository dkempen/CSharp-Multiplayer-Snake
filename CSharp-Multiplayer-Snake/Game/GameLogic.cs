using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharp_Multiplayer_Snake
{
    class GameLogic
    {
        public bool CheckForDeath(List<Snake> snakes, Snake snake, int gridSize)
        {
            Point head = snake.Head;

            // Check if off the grid
            if (head.X < 0 || head.X >= gridSize ||
                head.Y < 0 || head.Y >= gridSize)
                return true;

            // Check if the head is on a body that is not it's own head
            foreach (Snake s in snakes)
            {
                int index = -1;
                foreach (Point body in s.Body)
                {
                    index++;
                    if (ReferenceEquals(snake, s) && index == 0)
                        continue;
                    if (head.Equals(body))
                        return true;
                }
            }
            return false;
        }

        public void ChangeSnakeDirection(List<Snake> snakes, KeyEventArgs e)
        {
            if (snakes.Count <= 0)
                return;
            // Change direction if the previous direction is not the opposite
            Snake snake = snakes[0];
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

        public bool EatsAnApple(Snake snake, Apple apple)
        {
            return snake.GetUpdatedHeadPosition().Equals(apple.Position);
        }

        public void AddApples(GameLoop gameLoop)
        {
            while (gameLoop.Apples.Count < GameLoop.amountOfApples)
            {
                Apple apple = new Apple(gameLoop.Snakes, gameLoop.Apples, GameLoop.gridSize);
                gameLoop.Apples.Add(apple);     
            }
        }
    }
}
