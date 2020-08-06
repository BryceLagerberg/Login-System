
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Login_System
{
    public class SQLControl
    {
        // initialize database and server but leave empty and will be overwritten later
        public string DataBase { get; set; }
        public string SQLServer { get; set; }


        public SQLControl(string _DataBase, string _SQLServer)
        {
            DataBase = _DataBase;
            SQLServer = _SQLServer;
        }

        //Create Account Function
        public void CreateAccount(String username, String password, String email, String firstname, String lastname)
        {


            //setup for connection to sql server and database
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = SQLServer;
            csb.InitialCatalog = DataBase;
            csb.IntegratedSecurity = true;

            string connString = csb.ToString();

            //sql command to enter data into our sql database
            Random rnd = new Random();
            int AccountNumber = rnd.Next(1, 10000);
            string queryString = "INSERT INTO [Account Logins](Username, Password, AccountNumber) VALUES ('" + username + "', '" + password + "', '" + AccountNumber + "' );" +
                                 "INSERT INTO[Account Information](AccountNumber, CreatedOn, LoggedIn) VALUES("+ AccountNumber +", GETDATE(), 0);";


            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = queryString;
                //makes connection to sql server
                connection.Open();
                //execute the command you made earlier!
                command.ExecuteReader();
            }
            MessageBox.Show("Account Created!");
        }

        public Profile Login(string Username, string Password)
        {
           

            //setup for connection to sql server and database
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = SQLServer;
            csb.InitialCatalog = DataBase;
            csb.IntegratedSecurity = true;

            string connString = csb.ToString();

            //sql command to pull username and password from our sql table
            string queryString = "SELECT Username, Password, FirstName FROM [Login Stats]";


            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = queryString;
                //makes connection to sql server
                connection.Open();

                //reads sql server output from command execution
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        Profile UserProfile = new Profile();
                        UserProfile.Username = reader["Username"].ToString();
                        UserProfile.Password = reader["Password"].ToString();
                        UserProfile.Name = reader["FirstName"].ToString();

                        // Check that the user exists in the SQL DB with the correct Username / Password
                       if (UserProfile.Username == Username && UserProfile.Password == Password)
                        {
                            connection.Close();
                            return UserProfile;
                        } 
                    }
                }

                connection.Close();
            }

            // If they dont
            return null;

        }

    }
}
