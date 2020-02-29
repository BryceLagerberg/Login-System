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
        private const string DataBase = "BrycesDB";
        private const string SQLServer = "BRYCELOGERBURG\\SQLEXPRESS";

        private List<ProfileData> Accounts = new List<ProfileData>();
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

    public class ProfileData{
        public string Username;
        public string Password;
        public bool LoggedIn;
        public DateTime LastLogin;


        public ProfileData()
        {

        }

        public ProfileData(string username, string password, bool loggedin)
        {

        }

    }
}
