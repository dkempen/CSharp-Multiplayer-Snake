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
        public Point Head => Body.ElementAt(0);
        public Point PreviousDirection { get; set; }
        public Point Direction { get; set; }
        public bool IsDead { get; set; }
        public readonly Color color;
        public int Id;

        public Snake(Point headPosition, Color color, int id)
        {
            Body = new LinkedList<Point>();
            Body.AddFirst(headPosition);
            Direction = Directions.Right;
            PreviousDirection = Direction;
            IsDead = false;
            this.color = color;
            this.Id = id;
        }

        public Point GetUpdatedHeadPosition()
        {
            return new Point(Head.X + Direction.X, Head.Y + Direction.Y);
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
