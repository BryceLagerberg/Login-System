using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_System
{
    public partial class ProfileWindow : Form
    {
        // Gloabl Variables 
        private LoginWindow1 LoginWindowForm;

        // Constructor
        public ProfileWindow(LoginWindow1 LoginWindow)
        {
            LoginWindowForm = LoginWindow;
            InitializeComponent();

        }


        #region "Events"

        //Password TB
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (Globals.LoggedInUser != null)
            {
                Globals.LoggedInUser.Password = Functions.Encrypt(textBox2.Text);
                Globals.SC.Update("UPDATE [Account Logins] SET Password='" + Globals.LoggedInUser.Password + "' WHERE AccountNumber = " + Globals.LoggedInUser.AccountNumber);
            }
        }

        //FirstName TB
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (Globals.LoggedInUser != null)
            {
                Globals.LoggedInUser.FirstName = textBox4.Text;
                Globals.SC.Update("UPDATE [Account Information] SET FirstName='" + textBox4.Text + "' WHERE AccountNumber = " + Globals.LoggedInUser.AccountNumber);
            }
        }

        //LastName TB
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (Globals.LoggedInUser != null)
            {
                Globals.LoggedInUser.LastName = textBox5.Text;
                Globals.SC.Update("UPDATE [Account Information] SET LastName='" + textBox5.Text + "' WHERE AccountNumber = " + Globals.LoggedInUser.AccountNumber);
            }
        }
        
        //Email TB
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (Globals.LoggedInUser != null)
            {
                Globals.LoggedInUser.Email = textBox6.Text;
                Globals.SC.Update("UPDATE [Account Information] SET Email='" + textBox6.Text + "' WHERE AccountNumber = " + Globals.LoggedInUser.AccountNumber);
            }
        }
        
        //Log Out button
        private void button1_Click(object sender, EventArgs e)
        {
            Logout();
        }

        // Image Browse
        private void button2_Click(object sender, EventArgs e)
        {
            ImageBrowse();
        }

        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Logout();
        }
        private void browseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageBrowse();
        }

        //edit picture click event that makes password editable and readable or vice versa
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (textBox2.ReadOnly)
            {
                textBox2.ReadOnly = false;
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.ReadOnly = true;
                textBox2.PasswordChar = 'X';

            }

        }

        

        // Closing Out
        private void ProfileWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logout();
            LoginWindowForm.Close();
        }

        #endregion


        #region "Functions / Methods"

        // having version 1 and 2 for LoadProfile is called overloading
        // Version 1 - Not Used
        public void LoadProfile(string UsernameValue, string Password)
        {
            textBox1.Text = UsernameValue;
            textBox2.Text = Password;
        }

        // Version 2
        public void LoadProfile()
        {

            // Populate the fields
            textBox1.Text = Globals.LoggedInUser.Username;
            textBox2.Text = Functions.Decrypt(Globals.LoggedInUser.Password);
            textBox3.Text = Globals.LoggedInUser.AccountNumber.ToString();
            textBox4.Text = Globals.LoggedInUser.FirstName;
            textBox5.Text = Globals.LoggedInUser.LastName;
            textBox6.Text = Globals.LoggedInUser.Email;
            textBox7.Text = Globals.LoggedInUser.LastLogin.ToString();
            textBox8.Text = Globals.LoggedInUser.CreatedOn.ToString();

        }

        // Logs the user out 
        public void Logout()
        {
            // Log out the user (SQL)
            Globals.SC.Update("UPDATE [Account Information] SET LoggedIn = 0 WHERE AccountNumber = " + Globals.LoggedInUser.AccountNumber);
            Globals.LoggedInUser = null;

            // Clear all field values
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";


            // Hide this form / Show login form
            this.Visible = false;
            LoginWindowForm.Visible = true;
            
        }
        // browse for profile picture
        public void ImageBrowse()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = "Profile Image Browser";
            OFD.Multiselect = false;
            OFD.InitialDirectory = "C:\\Users\\bryce\\Desktop\\Login System Project\\Login-System\\Images";
            OFD.Filter = "JPG files|*.jpg|All files|*.*|Aaron|Aaron*.*";

            // Open file dialog and if result is OK NOT Cancle 
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                // Set the new image
                pictureBox1.ImageLocation = OFD.FileName;
                Globals.SC.Update("UPDATE [Account Information] SET ProfilePicture ='" + OFD.SafeFileName + "' WHERE AccountNumber = " + Globals.LoggedInUser.AccountNumber);

                // Add a copy of the image to the images folder for quicker reload
                if (!System.IO.File.Exists("C:\\Users\\bryce\\Desktop\\Login System Project\\Login-System\\Images\\" + OFD.SafeFileName))
                {
                    System.IO.File.Copy(OFD.FileName, "C:\\Users\\bryce\\Desktop\\Login System Project\\Login-System\\Images\\" + OFD.SafeFileName, false);
                }

            }
        }




        #endregion

        private void ProfileWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
