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
        public LoginWindow1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateAccount(textBox2.Text,textBox1.Text);
        }
        
        private void CreateAccount(String username, String password)
        {
            MessageBox.Show("Username : " + username + " Password : " + password);

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = "BURRYSSP6";
            csb.InitialCatalog = "BrycesDB";
            csb.IntegratedSecurity = true;

            string connString = csb.ToString();

            //Be sure to replace <YourTable> with the actual name of the Table
            string queryString = "INSERT INTO [Login Stats](Username, Password,LoggedIn, LastLogin) VALUES ('"+username+"', '"+password+"', 0, GETDATE());";

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = queryString;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Send these to your WinForms textboxes
                        string nameValue = reader["Name"].ToString();
                        string classValue = reader["Class"].ToString();
                        string schoolValue = reader["School"].ToString();
                    }
                }
            }
        }

        private void Login(String username, String password)
        {

        }

        private void LoginWindow1_Load(object sender, EventArgs e)
        {

        }
    }
}
