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
        SQLControl SC;
        private AccountInfo AI;
        
        // When the form loads
        private void LoginWindow1_Load(object sender, EventArgs e)
        {
            SC = new SQLControl("BrycesDB", "MSI");
        }


        public LoginWindow1()
        {
            InitializeComponent();
        }

        // Create Account Button
        private void button2_Click(object sender, EventArgs e)
        {
            AI = new AccountInfo(SC);
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
            // Attempt  Login
            Profile User = SC.Login(username, password);


            // If Success
            if (User != null)
            {
                this.Visible = false;
                PW.Visible = true;
                PW.LoadProfile(User);
            }
            else
            {
                this.Text = "Bad Login";
                MessageBox.Show("Error Loggin In, \n\n Invalid Username / Password");

            }

        }


    }


}
