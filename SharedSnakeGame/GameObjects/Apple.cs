using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Multiplayer_Snake
{
    class Apple : GameObject
    {
        public Point Position { get; set; }

        public Apple(List<Snake> snakes, int gridSize)
        {
            Random random = new Random();
            List<int> randomX = Enumerable.Range(0, gridSize).OrderBy(n => random.Next()).ToList();
            List<int> randomY = Enumerable.Range(0, gridSize).OrderBy(n => random.Next()).ToList();
            for (int xx = 0; xx < gridSize; xx++)
            {
                for (int yy = 0; yy < gridSize; yy++)
                {
                    int x = randomX[xx];
                    int y = randomY[yy];
                    Point p = new Point(x, y);
                    bool possible = true;
                    if (CheckIfPositionIsPossible(snakes, p))
                    {
                        Position = p;
                        return;
                    }
                }
            }
        }

        private bool CheckIfPositionIsPossible(List<Snake> snakes, Point p)
        {
            foreach (Snake snake in snakes)
                foreach (Point body in snake.Body)
                    if (body.Equals(p))
                        return false;
            return true;
        }

        public bool IsOnHead(Point headPosition)
        {
            return Position.Equals(headPosition);
        }
    }
}
