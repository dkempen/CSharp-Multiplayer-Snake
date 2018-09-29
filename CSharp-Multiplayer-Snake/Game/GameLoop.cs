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
        private GameLogic logic;
        public Form Form { get; }
        public List<Snake> Snakes { get; }
        public List<Apple> Apples { get; }

        private Timer timer;

        public GameLoop(Form form)
        {
            // Save form and create help classes
            Form = form;
            draw = Draw.GetInstance();
            draw.GameLoop = this;
            logic = new GameLogic();

            // Create snakes
            Snakes = new List<Snake>();
            Snakes.Add(new Snake(new Point(gridSize / 2, gridSize / 2), snake1Color));

            // Create apples
            Apples = new List<Apple>();
            Apples.Add(new Apple(Snakes, Apples, gridSize));

            // Start timer
            timer = new Timer(200);
            timer.Elapsed += TimeElapsed;
            timer.Start();
        }

        private void Loop()
        {
            // For each snake check if next move it eats an apple and then update the snake
            foreach (Snake snake in Snakes)
            {
                bool hasEaten = false;
                for (int i = Apples.Count - 1; i >= 0; i--)
                {
                    if (logic.EatsAnApple(snake, Apples[i]))
                    {
                        Apples.Remove(Apples[i]);
                        hasEaten = true;
                        break;
                    }
                }
                snake.UpdateBody(hasEaten);
            }

            // For each snake check if next move results in a death
            foreach (Snake snake in Snakes)
                if (logic.CheckForDeath(Snakes, snake, gridSize))
                    snake.IsDead = true;

            // Remove all dead snakes
            for (int i = Snakes.Count - 1; i >= 0; i--)
                if (Snakes[i].IsDead)
                    Snakes.Remove(Snakes[i]);

            // Check if an apple has been eaten and has to be respawned
            if (Apples.Count <= 0)
                Apples.Add(new Apple(Snakes, Apples, gridSize));

            // Draw the game
            draw.DrawGame();
        }

        public void KeyPressedHandler(KeyEventArgs e)
        {
            logic.ChangeSnakeDirection(Snakes, e);
        }

        private void TimeElapsed(object sender, ElapsedEventArgs e)
        {
            Loop();
        }
    }
}
