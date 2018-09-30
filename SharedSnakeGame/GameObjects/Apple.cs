using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Multiplayer_Snake
{
    class Apple : GameObject
    {
        public Point Position { get; set; }

        public Apple(List<Snake> snakes, List<Apple> apples, int gridSize)
        {
            Random random = new Random();
            List<int> randomCoord = Enumerable.Range(0, gridSize * gridSize).OrderBy(n => random.Next()).ToList();
            for (int i = 0; i < randomCoord.Count; i++)
            {
                int x = randomCoord[i] / gridSize;
                int y = randomCoord[i] % gridSize;
                Point p = new Point(x, y);
                bool possible = true;
                if (CheckIfPositionIsPossible(snakes, apples, p))
                {
                    Position = p;
                    Debug.WriteLine($"Position of Apple {apples.Count + 1}: {Position}");
                    return;
                }
            }
        }

        private bool CheckIfPositionIsPossible(List<Snake> snakes, List<Apple> apples, Point p)
        {
            foreach (Snake snake in snakes)
                foreach (Point body in snake.Body)
                    if (body.Equals(p))
                        return false;
            foreach (Apple apple in apples)
                if (apple.Position.Equals(p))
                    return false;
            return true;
        }

        public bool IsOnHead(Point headPosition)
        {
            return Position.Equals(headPosition);
        }
    }
}
