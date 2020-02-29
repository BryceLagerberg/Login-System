using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Login_System
{
    class SQLControl
    {
        public const string DataBase = "BrycesDB";
        public const string SQLServer = "BRYCELOGERBURG\\SQLEXPRESS";
        
        //Create Account Function
        public void CreateAccount(String username, String password, String email)
        {
            // a visual textbox telling username and password after creating an account
            //MessageBox.Show("Username : " + username + " Password : " + password);


            //setup for connection to sql server and database
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = SQLServer;
            csb.InitialCatalog = DataBase;
            csb.IntegratedSecurity = true;

            string connString = csb.ToString();

            //sql command to enter data into our sql database
            string queryString = "INSERT INTO [Login Stats](Username, Password, LoggedIn, AccountCreated, LastLogin, Email) VALUES ('" + username + "', '" + password + "', 0, GETDATE(), NULL, '" + email + "');";
            //string queryString = "select * from [Login Stats]";

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = queryString;
                //makes connection to sql server
                connection.Open();
                //execute the command you made earlier!
                command.ExecuteReader();





              /*  using (SqlDataReader reader = command.ExecuteReader())
                {
                    //reads sql server output from command execution
                    while (reader.Read())
                    {

                        ProfileData PD = new ProfileData();
                        PD.Username = reader["Username"].ToString();
                        PD.Password = reader["Password"].ToString();
                        PD.LoggedIn = Convert.ToBoolean(reader["LoggedIn"].ToString());
                        Accounts.Add(PD);
                    }
                }
                Console.WriteLine();
                */

                /* foreach(ProfileData PD in Accounts)
                 {
                     if(PD.Username == textBox1.)
                 }
                 */
            }
        }
    }
}
