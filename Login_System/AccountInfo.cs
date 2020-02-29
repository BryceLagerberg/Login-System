using System;
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
    public partial class AccountInfo : Form
    {
        SQLControl SC = new SQLControl();
        public AccountInfo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SC.CreateAccount(textBox1.Text, textBox2.Text, textBox3.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
