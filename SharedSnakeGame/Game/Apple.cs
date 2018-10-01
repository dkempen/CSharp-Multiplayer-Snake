using System.Drawing;

namespace CSharp_Multiplayer_Snake
{
    class Apple : GameObject
    {
        public Point Position { get; set; }

        public Apple(Point position)
        {
            Position = position;
        }
    }
}
