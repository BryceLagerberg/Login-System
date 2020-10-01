﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_System
{
    public partial class CreateAccount : Form
    {

        SQLControl SC;

        public CreateAccount(SQLControl SControl)
        {
            InitializeComponent();
            SC = SControl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SC.CreateAccount(textBox1.Text, Functions.Encrypt(textBox2.Text));
            this.Hide();
        }

        private void AccountInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
