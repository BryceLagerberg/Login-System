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


            // Initlize Size
            this.Size = new Size(301, 190);

            // Initilize Profile Window
            PW = new ProfileWindow(this);

            // Initilize SQL Control
            SQLControl SC = new SQLControl(textBox4.Text, textBox3.Text);
            Globals.SC = SC;

            // Initilize Verification Thread and run it
            new Thread(VerifyConnection).Start();

        }

        // Create Account Button
        private void button2_Click(object sender, EventArgs e)
        {
            if(AI == null )
            {
                AI = new CreateAccount(Globals.SC);
            }
            
            AI.Show();
            AI.Location = new Point(this.Location.X + this.Width -10, this.Location.Y);

        }
        // Login Button
        private void button1_Click(object sender, EventArgs e)
        {
            Login(textBox2.Text, textBox1.Text);
        }
        // changes the database address
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Globals.SC.DataBase = textBox3.Text;
            label5.ForeColor = Color.Gold;
            label5.Text = "Connecting...";
         
            new Thread(VerifyConnection).Start();

        }
        //changes the SQL server address
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Globals.SC.SQLServer = textBox4.Text;
            label5.ForeColor = Color.Gold;
            label5.Text = "Connecting...";

            new Thread(VerifyConnection).Start();

        }
        //adding flare!
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

        }





        // Helper Functions----------------------------------------------------------------------

        // Login Function
        private void Login(String username, String password)
        {
            // Attempt  Login
            Profile User = Globals.SC.Login(username, Functions.Encrypt(password));
            //User.Password = Functions.Decrypt(User.Password);

            // If Success
            if (User != null)
            {
                Globals.LoggedInUser = User;

                this.Visible = false;
                PW.Visible = true;
                textBox1.Text = "";
                textBox2.Text = "";
                PW.LoadProfile();
            }
            else
            {
                this.Text = "Bad Login";
                MessageBox.Show("Error Loggin In, \n\n Invalid Username / Password");
                textBox1.Text = "";
                textBox2.Text = "";
            }

        }

        // Verifies the Connection state with the SQL server / DB (LAG)
        private void VerifyConnection()
        {
            VerifyConnectionDelegate(Globals.SC.TestConnection());
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

                    // Start Slide Effect
                    Thread SlideThread = new Thread(SlideDown);
                    SlideThread.Start();

                    // Check SQL DB for required tables
                    Globals.SC.TableCheck();
                    //i have 20 min or so before i gotta get reaady
                    //say again?
                    //so much its very loud over here youd hate it through these garbage headphoens
                    //its both nice and not nice she did bring me a beer though so worth the music
                    //also all my questions are in notes now haha so i can look back
                    // 
                }
                else
                {
                    label5.ForeColor = Color.Red;
                    label5.Text = "Failed!";
                }
            }
            else
            {
                this.Invoke(new Action<bool>(VerifyConnectionDelegate), Success);
            }
        } // Delegate Function


        // Slide Functions
        private void SlideDown()
        {
            while (this.Size.Height < 425)
            {
                System.Threading.Thread.Sleep(5);

                SlideDownDelegate(new Size(this.Size.Width, this.Size.Height + 2));
                
            }
        } //(LAG)
        private void SlideDownDelegate(Size newSize)
        {
            if (this.InvokeRequired == false)
            {
                this.Size = newSize;
            }
            else
            {
                this.Invoke(new Action<Size>(SlideDownDelegate), newSize);
            }
        }

        // could we add one last grow call in the main so it never ends up too short?
        // i think that the designer doesnt include the header and footer as a size
        //hmm maybe a scale issue?
        // ive had it tell me to change my % zoom or something before 
        // i just do=nt know what that was for so i ignored
        //say that again i missed that
        //seems weird it would stop short
        // when i drag it around it stops growing. it also isnt the right size now
        //yea that makes sense i dont get how you make it change the parameters of the main threads window
        //so the side thread pauses between sending the +1?
        // so the main thread is still waiting the delay but isnt exactly paused it just hasnt gotten the new +1 becuase the side thread is delayed
    }


}
