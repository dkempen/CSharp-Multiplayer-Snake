using SharedSnakeGame.Game;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CSharp_Multiplayer_Snake.Visuals
{
    class Draw
    {
        private static Draw instance;

        public GameData GameData { get; set; }
        public Form Form { get; set; }
        private int gridSize;

        private readonly Color AppleColor = Color.Firebrick;
        private readonly Color backgroundColor = Color.DimGray;
        private readonly int padding = 5;

        public static Draw GetInstance()
        {
            return instance ?? (instance = new Draw());
        }

        internal void DrawGame(Graphics g, Panel panel)
        {
            gridSize = panel.Size.Height / GameData.GridSize;
            DrawBackgroud(panel);
            foreach (Snake snake in GameData.Snakes)
                DrawSnake(g, snake);
            foreach (Apple apple in GameData.Apples)
                DrawApple(g, apple);
        }

        public void DrawGame()
        {
            try { Form.Invoke((MethodInvoker)delegate { Form.Refresh(); }); }
            catch (ObjectDisposedException e) { }
        }

        private void DrawSnake(Graphics g, Snake snake)
        {
            foreach (Point bodyPart in snake.Body)
                DrawBodyPart(g, bodyPart, snake.color);
        }

        private void DrawBodyPart(Graphics g, Point bodyPart, Color snakeColor)
        {
            DrawRectangle(g, bodyPart, snakeColor);
        }

        private void DrawApple(Graphics g, Apple apple)
        {
            DrawRectangle(g, apple.Position, AppleColor);
        }

        private void DrawRectangle(Graphics g, Point position, Color color)
        {
            g.FillRectangle(new SolidBrush(color), position.X * gridSize + padding,
                position.Y * gridSize + padding, gridSize - padding * 2, gridSize - padding * 2);
        }

        private void DrawBackgroud(Panel panel)
        {
            panel.BackColor = backgroundColor;
        }
    }
}
