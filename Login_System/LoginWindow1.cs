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
        public LoginWindow1()
        {
            InitializeComponent();
        }

        // Create Account Button
        private void button2_Click(object sender, EventArgs e)
        {
            CreateAccount(textBox2.Text, textBox1.Text);
        }

        //Create Account Function
        private void CreateAccount(String username, String password)
        {
            // a visual textbox telling username and password after creating an account
            //MessageBox.Show("Username : " + username + " Password : " + password);

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = SQLServer;
            csb.InitialCatalog = DataBase;
            csb.IntegratedSecurity = true;

            string connString = csb.ToString();

            //Be sure to replace <YourTable> with the actual name of the Table
            string queryString = "INSERT INTO [Login Stats](Username, Password, LoggedIn, AccountCreated, LastLogin) VALUES ('"+username+"', '"+password+"', 0, GETDATE(), NULL);";
            //string queryString = "select * from [Login Stats]";

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = queryString;

                connection.Open();
                //command.ExecuteReader();

                

                

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Send these to your WinForms textboxes
                        ProfileData PD = new ProfileData();
                        PD.Username = reader["Username"].ToString();
                        PD.Password = reader["Password"].ToString();
                        PD.LoggedIn = Convert.ToBoolean(reader["LoggedIn"].ToString());
                        Accounts.Add(PD);
                    }
                }
                Console.WriteLine();


               /* foreach(ProfileData PD in Accounts)
                {
                    if(PD.Username == textBox1.)
                }
                */
            }
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
