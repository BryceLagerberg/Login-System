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

        Profile LoggedInUser = null;
        SQLControl SC;

        public ProfileWindow()
        {
            InitializeComponent();
        }

        // having version 1 and 2 for LoadProfile is called overloading
        // Version 1
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
            //textBox6.Text = LoggedInUser.Email;
        }




        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Do nothing for Account #
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            LoggedInUser.FirstName = textBox4.Text;
            SC.Update("UPDATE [Account Information] SET FirstName='"+textBox4.Text+"' WHERE AccountNumber = "+LoggedInUser.AccountNumber);
        }


        private void ProfileWindow_FormClosing(object sender, System.Windows.Forms.FormClosingEventHandler e)
        {
            MessageBox.Show("Worked");
        }
    }
}
