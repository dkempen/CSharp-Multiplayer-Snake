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

        public Apple(Point position)
        {
            Position = position;
        }
    }
}
