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
    public partial class ChatWindow : Form
    {

        Profile CurrentFriend;


        #region events

        public ChatWindow()
        {
            InitializeComponent();
        }

        //the form closing event
        private void ChatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }
        
        //message send button
        private void button1_Click(object sender, EventArgs e)
        {
            Globals.SC.SendChat(Globals.LoggedInUser.AccountNumber,CurrentFriend.AccountNumber,DateTime.Now,textBox1.Text);
        }

        #endregion


        #region methods/function
        //load chat history with current user and friend
        public void LoadChat(Profile friend)
        {
            CurrentFriend = friend;
        }
        #endregion




        
    }
}
