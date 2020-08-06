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
        public void LoadProfile(Profile User)
        {
            textBox1.Text = User.Username;
            textBox2.Text = User.Password;
        }

    }
}
