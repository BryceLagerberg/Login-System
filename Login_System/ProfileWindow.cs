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
        private Profile LoggedInUser = null;
        private SQLControl SC;
        private LoginWindow1 LoginWindowForm;

        public ProfileWindow(LoginWindow1 LoginWindow)
        {
            LoginWindowForm = LoginWindow;
            InitializeComponent();
        }


        #region "Events"


        //FirstName TB
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            LoggedInUser.FirstName = textBox4.Text;
            SC.Update("UPDATE [Account Information] SET FirstName='"+textBox4.Text+"' WHERE AccountNumber = "+LoggedInUser.AccountNumber);
        }

        //LastName TB
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            LoggedInUser.LastName = textBox5.Text;
            SC.Update("UPDATE [Account Information] SET LastName='" + textBox5.Text + "' WHERE AccountNumber = " + LoggedInUser.AccountNumber);
        }
        
        //Email TB
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            LoggedInUser.Email = textBox6.Text;
            SC.Update("UPDATE [Account Information] SET Email='" + textBox6.Text + "' WHERE AccountNumber = " + LoggedInUser.AccountNumber);
        }
        
        //Log Out button
        private void button1_Click(object sender, EventArgs e)
        {
            LoginWindowForm.Logout();

        }

        private void ProfileWindow_FormClosing(object sender, System.Windows.Forms.FormClosingEventHandler e)
        {
            MessageBox.Show("Worked");
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
        public void LoadProfile(Profile User, SQLControl SC)
        {
            // Set the logged in user
            LoggedInUser = User;
            this.SC = SC;

            // Populate the fields
            textBox1.Text = LoggedInUser.Username;
            textBox2.Text = LoggedInUser.Password;
            textBox3.Text = LoggedInUser.AccountNumber.ToString();
            textBox4.Text = LoggedInUser.FirstName;
            textBox5.Text = LoggedInUser.LastName;
            textBox6.Text = LoggedInUser.Email;
            textBox7.Text = LoggedInUser.LastLogin.ToString();
            textBox8.Text = LoggedInUser.CreatedOn.ToString();

        }



        #endregion


    }
}
