using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Multiplayer_Snake
{
    public class Snake : GameObject
    {
        public LinkedList<Point> Body { get; private set; }
        public Point PreviousDirection { get; set; }
        public Point Direction { get; set; }
        public readonly Color color;

        public Snake(Point headPosition, Color color)
        {
            Body = new LinkedList<Point>();
            Body.AddFirst(headPosition);
            Direction = Directions.Right;
            PreviousDirection = Direction;
            this.color = color;
        }

        public Point GetUpdatedHeadPosition()
        {
            Point currentHeadPos = Body.ElementAt(0);
            return new Point(currentHeadPos.X + Direction.X, currentHeadPos.Y + Direction.Y);
        }

        public void UpdateBody(bool hasEaten)
        {
            PreviousDirection = Direction;
            Point newHeadPos = GetUpdatedHeadPosition();
            Body.AddFirst(newHeadPos);
            if (!hasEaten)
                Body.RemoveLast();
        }

        public static class Directions
        {
            public static readonly Point Up = new Point(0, -1);
            public static readonly Point Down = new Point(0, 1);
            public static readonly Point Left = new Point(-1, 0);
            public static readonly Point Right = new Point(1, 0);
        }
    }
}
