﻿using System;
using System.Windows.Forms;

namespace Dummy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public static Form1 operator ++(Form1 b)
        {
            Form1 form = new Form1();
            return form;
        }
    }
}
