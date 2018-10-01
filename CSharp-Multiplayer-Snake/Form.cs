using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharp_Multiplayer_Snake.Networking;
using CSharp_Multiplayer_Snake.Visuals;

namespace CSharp_Multiplayer_Snake
{
    public partial class Form : System.Windows.Forms.Form
    {
     //   private GameLoop gameLoop;

        public Form()
        {
            InitializeComponent();
            NetworkHandler networkHandler = new NetworkHandler(this); 


         //   gameLoop = new GameLoop(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void GamePanelPaint(object sender, PaintEventArgs e)
        {
          //  var panel = sender as Panel;
          //  var g = e.Graphics;
          //  Draw.GetInstance().DrawGame(g, panel);
        }
        
        private void KeyPressedHandler(object sender, KeyEventArgs e)
        {
          //  gameLoop.KeyPressedHandler(e);
        }
        
    }
}
