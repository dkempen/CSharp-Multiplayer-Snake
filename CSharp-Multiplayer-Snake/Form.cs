using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using CSharp_Multiplayer_Snake.Networking;
using CSharp_Multiplayer_Snake.Visuals;
using SharedSnakeGame.Data;

namespace CSharp_Multiplayer_Snake
{
    public partial class Form : System.Windows.Forms.Form
    {
        private NetworkHandler networkHandler;
        private string name;

        public Form()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(Form_FormClosed);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            string input = "POO";
            new Thread(ee =>
            {
                Thread.Sleep(1000);
                name = ShowNameInputDialog(ref input);
                networkHandler = new NetworkHandler(this);
            }).Start();
        }

        private void GamePanelPaint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            var g = e.Graphics;
            Draw.GetInstance().DrawGame(g, panel);
        }

        private void KeyPressedHandler(object sender, KeyEventArgs e)
        {
            networkHandler.KeyPressedHandler(e);
        }
        
        

        void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            networkHandler.Disconnected = true;
        }

        private static string ShowNameInputDialog(ref string input)
        {
            Size size = new Size(420, 150);
            System.Windows.Forms.Form inputBox = new System.Windows.Forms.Form();

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.StartPosition = FormStartPosition.CenterParent;
            inputBox.Text = "Name";
            inputBox.MaximizeBox = false;
            inputBox.MinimizeBox = false;

            Label label = new Label();
            label.Size = new Size(size.Width - 10, 23);
            label.Location = new Point(5, 5);
            label.Text = "Enter a 3 character name";
            label.Font = new Font("Courier New", 20);
            inputBox.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Size = new Size(size.Width - 10, 23);
            textBox.Location = new Point(5, 50);
            textBox.Text = input;
            textBox.Font = new Font("Courier New", 20);
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 40);
            okButton.Text = "&OK";
            okButton.Font = new Font("Courier New", 20);
            okButton.Location = new Point((int) (size.Width * 0.5 - 75 * 0.5), 100);
            inputBox.Controls.Add(okButton);
            inputBox.AcceptButton = okButton;

            while (true)
            {
                inputBox.ShowDialog();
                input = textBox.Text;
                if (input.Length == 3)
                    return input.ToUpper();
            }
        }

        public static void ShowHighScoreDialog(Highscore highScore, int score)
        {
            Size size = new Size(300, 480);
            System.Windows.Forms.Form inputBox = new System.Windows.Forms.Form();

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.StartPosition = FormStartPosition.CenterParent;
            inputBox.Text = "Highscores";
            inputBox.MaximizeBox = false;
            inputBox.MinimizeBox = false;

            Label label = new Label();
            label.Size = new Size(size.Width - 10, 23);
            label.Location = new Point(20, 5);
            label.Font = new Font("Courier New", 20);
            label.Text = $"Your score: {score}";
            inputBox.Controls.Add(label);

            Label highScores = new Label();
            highScores.Size = new Size(size.Width - 10, 370);
            highScores.Location = new Point(20, 50);
            highScores.Font = new Font("Courier New", 20);
            highScores.Text = $"Highscores:\n\n{highScore.ToString()}";
            inputBox.Controls.Add(highScores);

            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 40);
            okButton.Text = "&OK";
            okButton.Font = new Font("Courier New", 20);
            okButton.Location = new Point((int)(size.Width - 80 - 75 * 1.5), 420);
            inputBox.Controls.Add(okButton);
            inputBox.AcceptButton = okButton;
            
            inputBox.ShowDialog();
        }
    }
}
