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
    public partial class CreateAccount : Form
    {

        SQLControl SC;

        public CreateAccount(SQLControl SControl)
        {
            InitializeComponent();
            SC = SControl;
        }
        //create account button
        private void button1_Click(object sender, EventArgs e)
        {
            if (Globals.SC.UsernameCheck(textBox1.Text) && Functions.PasswordCheck(textBox2.Text, textBox1.Text))
            {
                SC.CreateAccount(textBox1.Text, Functions.Encrypt(textBox2.Text));
                this.Hide();
            } else
            {
                MessageBox.Show("\t\t         ERROR\nusername or password requirements have not been met");
            }
            
        }


        // Closing Out/ actually hides the page
        private void ProfileWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        //Username Availability check
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool test = Globals.SC.UsernameCheck(textBox1.Text);
            if (test)
            {
                pictureBox1.Image = Login_System.Properties.Resources.CheckMark;
            }
            else
            {
                pictureBox1.Image = Login_System.Properties.Resources.RedX;
            }

            // Ignore the blank case where they havn't started typing yet.
            if (textBox1.Text == "") { pictureBox1.Visible = false; } else { pictureBox1.Visible = true; }
        }

        //Password requirements check
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bool test = Functions.PasswordCheck(textBox2.Text, textBox1.Text);
            if (test)
            {
                pictureBox2.Image = Login_System.Properties.Resources.CheckMark;
            }
            else
            {
                pictureBox2.Image = Login_System.Properties.Resources.RedX;
            }
            // Ignore the blank case where they havn't started typing yet.
            if (textBox2.Text == "") { pictureBox2.Visible = false; } else { pictureBox2.Visible = true; }
        }
    }
}
