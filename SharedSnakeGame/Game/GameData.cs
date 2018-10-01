using CSharp_Multiplayer_Snake;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedSnakeGame.Game
{
    class GameData
    {
        public int GridSize { get; }

        public List<Snake> Snakes { get; set; }
        public List<Apple> Apples { get; set; }
        
        public GameData(int gridSize)
        {
            this.GridSize = gridSize;
            Snakes = new List<Snake>();
            Apples = new List<Apple>();
        }



    }
}
