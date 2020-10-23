﻿
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
        #region Global Variables

        // initialize database and server but leave empty and will be overwritten later
        public string DataBase { get; set; }
        public string SQLServer { get; set; }

        public bool Connected { get; set; } = false;

        #endregion


        public SQLControl(string _SQLServer, string _DataBase)
        {
            DataBase = _DataBase;
            SQLServer = _SQLServer;
        }

        #region Functions
        public Boolean TestConnection()
        {
            //setup for connection to sql server and database
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = SQLServer;
            csb.InitialCatalog = DataBase;
            csb.IntegratedSecurity = true;

            string connString = csb.ToString();

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                connection.Open();
                connection.Close();
                Connected = true;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        // Check For Tables
        public void TableCheck()
        {
            //setup for connection to sql server and database
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = SQLServer;
            csb.InitialCatalog = DataBase;
            csb.IntegratedSecurity = true;

            string connString = csb.ToString();


            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Account Logins' or TABLE_NAME = 'Account Information'";
                //makes connection to sql server
                connection.Open();

                //Check to see what tables exist
                bool AccountLogins = false;
                bool AccountInformation = false;
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {               
                        string Table = reader["TABLE_NAME"].ToString();
                        if(Table == "Account Information")
                        {
                            //what needs to be in each table? check for that
                            AccountInformation = true;
                        }
                        else if(Table == "Account Logins")
                        {
                            AccountLogins = true;
                        }
                        
  
                         
                    }
                }

                // Create Missing Tables
                //
                if (!AccountLogins)
                {
                    command.CommandText = "CREATE TABLE "+DataBase+".dbo.[Account Logins] (Username varchar(50), Password varchar(50), AccountNumber int,);";
                    command.ExecuteReader();
                }
                if (!AccountInformation)
                {
                    // Email, FirstName, LastName, CreatedOn, LastLogin, ProfilePicture, LoggedIn
                    command.CommandText = "CREATE TABLE "+ DataBase + ".dbo.[Account Information](AccountNumber int, FirstName varchar(50), LastName varchar(50), Email varchar(50), LoggedIn bit, LastLogin datetime, CreatedOn datetime, ProfilePicture varchar(50));";
                    command.ExecuteReader();
                }

                connection.Close();
            }
        }
        //checks if a username is unique
        public bool UsernameCheck(String TestUsername)
        {
            bool IsAvailable = true;
            //setup for connection to sql server and database
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = SQLServer;
            csb.InitialCatalog = DataBase;
            csb.IntegratedSecurity = true;
            string connString = csb.ToString();
            //pull all the usernames from account logins
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    string queryString = "Select Username FROM [Account Logins]";
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

                            //compare usernames from account logins to TestUsername
                            if (UserProfile.Username == TestUsername)
                            {
                                IsAvailable = false;
                            }
                        }
                    }
                }
            }
            //return the results
            return IsAvailable;
        }

        //Create Account Function
        public void CreateAccount(String username, String password)
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
                                 "INSERT INTO[Account Information](AccountNumber, CreatedOn, LoggedIn) VALUES(" + AccountNumber + ", GETDATE(), 0);";



            using (SqlConnection connection = new SqlConnection(connString))
            {
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
        }

        //Login the user function
        public Profile Login(string Username, string Password)
        {
           

            //setup for connection to sql server and database
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = SQLServer;
            csb.InitialCatalog = DataBase;
            csb.IntegratedSecurity = true;
            string connString = csb.ToString();


            Profile LoggingInUser = null;


            // Pull Profile
            using (SqlConnection connection = new SqlConnection(connString))
            {

                // Compares Username / Password with SQL DB and grabs the profile if a match is found
                using (SqlCommand command = connection.CreateCommand())
                {
                    //sql command to pull username and password from our sql table
                    string LoginQuery = "SELECT Username, Password, AccountNumber  FROM [Account Logins]";
                    command.CommandText = LoginQuery;
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
                            UserProfile.AccountNumber = int.Parse(reader["AccountNumber"].ToString());
                            
                        

                            // Check that the user exists in the SQL DB with the correct Username / Password
                            if (UserProfile.Username == Username && UserProfile.Password == Password)
                            {
                                LoggingInUser = UserProfile;
                            } 
                        }
                    }

                }
                
                // Pull in Account Information if a matching Username & Password was found.
                if (LoggingInUser != null)
                {
                    // Pulls in the Profile data for the Logging In User
                    using (SqlCommand command = connection.CreateCommand())
                    {

                        string InformationQuery = "SELECT Email, FirstName, LastName, CreatedOn, LastLogin, ProfilePicture, LoggedIn  FROM [Account Information] WHERE AccountNumber = " + LoggingInUser.AccountNumber;
                        command.CommandText = InformationQuery;

                        //reads sql server output from command execution
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                LoggingInUser.Email = reader["Email"].ToString();
                                LoggingInUser.FirstName = reader["FirstName"].ToString();
                                LoggingInUser.LastName = reader["LastName"].ToString();
                                LoggingInUser.CreatedOn = (reader.IsDBNull(3)) ? DateTime.Now : DateTime.Parse(reader["CreatedOn"].ToString());
                                LoggingInUser.LastLogin = (reader.IsDBNull(4)) ? DateTime.Now : DateTime.Parse(reader["LastLogin"].ToString());
                                LoggingInUser.ProfilePicture = reader["ProfilePicture"].ToString();
                                if ((bool)reader["LoggedIn"] == true) { MessageBox.Show("Already Logged In","Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return null; }
                            }
                        }

                        connection.Close();
                    }

                    //reads sql server output from command execution
                    Update("UPDATE [Account Information] SET LoggedIn = 1, LastLogin = '" + DateTime.Now.ToString() + "'  WHERE AccountNumber = " + LoggingInUser.AccountNumber);

                }

                
                // Close Out Connection
                connection.Close();
            }


            // Return Profile
            return LoggingInUser;

        }

        public void Update(string queryString)
        {
            //setup for connection to sql server and database
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = SQLServer;
            csb.InitialCatalog = DataBase;
            csb.IntegratedSecurity = true;

            string connString = csb.ToString();
            


            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = queryString;
                //makes connection to sql server
                connection.Open();
                //execute the command u made above!
                command.ExecuteReader();

                connection.Close();
            }
        }
        //pulls the friends of the current user
        public List<Profile> PullFriends()
        {
            return new List<Profile>();
        }

        //Pulls all users from the Account Information Table
        public Dictionary<int, Profile> PullUsers()
        {
            //setup for connection to sql server and database
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = SQLServer;
            csb.InitialCatalog = DataBase;
            csb.IntegratedSecurity = true;

            string connString = csb.ToString();


            Dictionary<int, Profile> FriendsList = new Dictionary<int, Profile>();



            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT AccountNumber, FirstName, ProfilePicture, LoggedIn FROM [Account Information]";
                //makes connection to sql server
                connection.Open();
                //execute the command u made above!

                using(SqlDataReader reader = command.ExecuteReader()){

                    while (reader.Read())
                    {
                        Profile Friend = new Profile();
                        Friend.AccountNumber = (int)reader["AccountNumber"];
                        Friend.FirstName = reader["FirstName"] is null ? "" : reader["FirstName"].ToString();
                        Friend.ProfilePicture = reader["ProfilePicture"] is null ? "" :  reader["ProfilePicture"].ToString();
                        Friend.LoggedIn = (bool)reader["LoggedIn"];
                        FriendsList.Add(Friend.AccountNumber, Friend);
                    }
                }



                connection.Close();
            }


            return FriendsList;
        }

        //sends a chat message
        public void SendChat(int accountNumber, int friendsAccountNumber, DateTime time, string message)
        {
            string queryString = "INSERT INTO[Chat Logs](AccountNumberSender, AccountNumberReceiver, SentTime, Message) VALUES("+accountNumber+", "+friendsAccountNumber+ ", GETDATE(), '" + message+"');";
            //setup for connection to sql server and database
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = SQLServer;
            csb.InitialCatalog = DataBase;
            csb.IntegratedSecurity = true;

            string connString = csb.ToString();



            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = queryString;
                //makes connection to sql server
                connection.Open();
                //execute the command u made above!
                command.ExecuteReader();

                connection.Close();
            }
        }

        #endregion
    }

}
