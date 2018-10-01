using System;
using System.Windows.Forms;
using CSharp_Multiplayer_Snake.Networking;
using CSharp_Multiplayer_Snake.Visuals;

namespace CSharp_Multiplayer_Snake
{
    public partial class Form : System.Windows.Forms.Form
    {
        private NetworkHandler networkHandler;

        public Form()
        {
            networkHandler = new NetworkHandler(this); 
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
        
    }
}
