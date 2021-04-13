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
using System.Threading;
using System.Runtime.CompilerServices;
using System.IO;
using Budget_Tracker.Forms;
using Utilities;


namespace Login_System
{
    public partial class LoginWindow1 : Form
    {
        // Global Variables-------------------------------------------------------------

        private ProfileWindow PW; 
        private CreateAccount AI = null;

        

        // Required...
        public LoginWindow1()
        {
            InitializeComponent();
        }


        // events ---------------------------------------------------------------------

        private void LoginWindow1_Load(object sender, EventArgs e)
        {

            // Create Folder / File structure
            InitilizeFolderStructure();

            // Initilize Profile Window
            PW = new ProfileWindow(this);

            // Initilize SQL Control
            SQLControl SC = new SQLControl(textBox4.Text, textBox3.Text, "192.168.86.38");// update after we add a textbox for serverip input
            Globals._SC = SC;

            // Load Last SQL Settings
            Functions.LoadSettings();
            textBox3.Text = Globals._Settings["SQL Database"];
            textBox4.Text = Globals._Settings["SQL Server"];
            try
            {
                if (Globals._Settings["Remember Username"] == "true")
                {
                    checkBox1.Checked = true; 
                    textBox2.Text = Globals._Settings["SQL Username"];
                    
                }
            }
            catch(Exception ex)
            {

            }


            // Initilize Verification Thread and run it
            if (textBox3.Text != "" && textBox4.Text != "") {
                new Thread(VerifyConnection).Start();
            }

        }

        // Create Account Button
        private void button2_Click(object sender, EventArgs e)
        {
            if(AI == null )
            {
                AI = new CreateAccount(Globals._SC);
            }
            
            AI.Location = new Point((Screen.PrimaryScreen.Bounds.Width / 2)-(this.Size.Width / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (this.Size.Height / 2)); 
            AI.Show();

        }
        
        // Login Button
        private void button1_Click(object sender, EventArgs e)
        {
            Login(textBox2.Text, textBox1.Text);
        }
        
        // changes the database address
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Globals._SC.DataBase = textBox3.Text;
            label5.ForeColor = Color.Gold;
            label5.Text = "Connecting...";
            if (textBox3.Text != "" && textBox4.Text != "")
            {
                new Thread(VerifyConnection).Start();
            }

        }
        
        //changes the SQL server address
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Globals._SC.SQLServer = textBox4.Text;
            label5.ForeColor = Color.Gold;
            label5.Text = "Connecting...";
            if (textBox3.Text != "" && textBox4.Text != "")
            {
                new Thread(VerifyConnection).Start();
            }

        }

        //turns enter key into login button shortcut
        private void textBox1_KeyPressEvent(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                Login(textBox2.Text, textBox1.Text);
            }
        }

        /*adding flare! (the old shitty version)
        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.PasswordChar = '\0';
                textBox1.Text = "Bet you cant guess me";

            }

        }
        //adding flare!
        private void textBox1_MouseLeave(object sender, EventArgs e)
        {

            if(textBox1.Text == "Bet you cant guess me")
            {
                textBox1.Text = "";
                textBox1.PasswordChar = 'X';
            }

        }*/





        // Helper Functions----------------------------------------------------------------------

        // Login Function

        // Create inital folder structure
        private void InitilizeFolderStructure()
        {
            // Check to see if the folder structure already exists and if it doesnt create it.
            if(!System.IO.Directory.Exists("C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System")) {
                System.IO.Directory.CreateDirectory("C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System");
            }
            //check to see if the folder structure contains a profile picture directory, if it does not it creates one!
            if (!System.IO.Directory.Exists("C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Profile_Pictures"))
            {
                System.IO.Directory.CreateDirectory("C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Profile_Pictures");
            }
        }



        //the function names says it all
        private void Login(String username, String password)
        {
            // Attempt  Login
            Profile User = Globals._SC.Login(username, Functions.Encrypt(password));
            //User.Password = Functions.Decrypt(User.Password);

            // If Success
            if (User != null)
            {
                Functions.SaveSettings("SQL Username", textBox2.Text);
                Globals._LoggedInUser = User;

                this.Visible = false;
                PW.Size = new Size(584, 616);
                PW.Visible = true;
                textBox1.Text = "";
                if(Globals._Settings["Remember Username"] == "false") { textBox2.Text = ""; }
                PW.LoadProfile();


            }
            else
            {
                this.Text = "Bad Login";
                MessageBox.Show("Error Loggin In, \n\n Invalid Username / Password");
                textBox1.Text = "";
            }

        }

        // Verifies the Connection state with the SQL server / DB (LAG)
        private void VerifyConnection()
        {
            VerifyConnectionDelegate(Globals._SC.TestConnection());
        }
        private void VerifyConnectionDelegate(bool Success)
        {
            if(this.InvokeRequired == false)
            {
                if (Success)
                {
                    label5.ForeColor = Color.LimeGreen;
                    label5.Text = "Connected!";
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    groupBox2.Enabled = true;
                    Functions.SaveSettings("SQL Server", textBox4.Text);
                    Functions.SaveSettings("SQL Database", textBox3.Text);

                    // Start Slide Effect
                    Thread SlideThread = new Thread(() => Functions.Grow(this, new Size(this.Size.Width, 465),5,2));
                    SlideThread.Start();

                    // Check SQL DB for required tables
                    Globals._SC.TableCheck();
                }
                else
                {
                    if(!Globals._SC.Connected)
                    {
                        label5.ForeColor = Color.Red;
                        label5.Text = "Failed!";
                    }
                }
            }
            else
            {
                this.Invoke(new Action<bool>(VerifyConnectionDelegate), Success);
            }
        }

        //save or clear username for login
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //save the username to settings file 
                Functions.SaveSettings("Remember Username", "true");
            }
            else
            {
                //clear saved username from settings file
                Functions.SaveSettings("Remember Username", "false");
            }
        }
    }
}
