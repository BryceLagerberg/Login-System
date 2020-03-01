using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Login_System
{
    public partial class LoginWindow1 : Form
    {

        private ProfileWindow PW = new ProfileWindow();
        private AccountInfo AI = new AccountInfo();
        public LoginWindow1()
        {
            InitializeComponent();
        }

        // Create Account Button
        private void button2_Click(object sender, EventArgs e)
        {
            AI.Show();   
        }
        // Login Button
        private void button1_Click(object sender, EventArgs e)
        {
            Login(textBox1.Text, textBox2.Text);
        }

        // Login Function
        private void Login(String username, String password)
        {
            PW.Visible = true;
            this.Visible = false; 
        }

    }


}
