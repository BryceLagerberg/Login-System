using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Utilities;

namespace Login_System
{
    public partial class ChatWindow : Form
    {

        // Friend window that you currently have open
        Profile CurrentFriend;

        // Other Friend Tabs
        Dictionary<int,Profile> Tabs = new Dictionary<int,Profile>();

        Thread ChatRefresh;
        DateTime LastRefresh;




        #region Events

        public ChatWindow()
        {
            InitializeComponent();
        }

        // On Load
        private void ChatWindow_Load(object sender, EventArgs e)
        {
            // Clears out the Base Tab
            tabControl1.TabPages.Clear();

            // start your threads. 3.2.1 go!
            ChatRefresh = new Thread(RefreshMessages);
            ChatRefresh.IsBackground = true;
            ChatRefresh.Start();
        }

        //the form closing event
        private void ChatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            listBox1.Items.Clear();
            this.Visible = false;

        }
        
        //message send button
        private void button1_Click(object sender, EventArgs e)
        {
            Globals._SC.SendChat(Globals._LoggedInUser.AccountNumber,CurrentFriend.AccountNumber,textBox1.Text);

            textBox1.Text = "";
        }

        //sets max message length
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            // Limit message length
            
            string message = (textBox1.Text.Length > 100) ? textBox1.Text.Substring(0, 100) : textBox1.Text;
            textBox1.Text = message;


            // Update Label Text
            label1.Text = "(" + textBox1.Text.Length + "/100)";
            if(textBox1.Text.Length >= 100)
            {
                label1.ForeColor = Color.Red;
            }else if(textBox1.Text.Length >= 61)
            {
                label1.ForeColor = Color.Orange;
            }else
            {
                label1.ForeColor = Color.Black;
            }
        }

        //enter key pressed event
        private void textBox1_EnterKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\n')
            {
                button1_Click(sender, new EventArgs());
            }
        }
        
        // tab changed event
        private void tabcontrol1_SelectedIndexChange(object sender, EventArgs e)
        {
            if(tabControl1.SelectedTab != null)
            {
                // get the friend id for the current tab
                int friendID = Int32.Parse(tabControl1.SelectedTab.Name);

                // load the messages with that friend
                Tabs[friendID].Messages = Globals._SC.GetMessages(Globals._LoggedInUser.AccountNumber, CurrentFriend.AccountNumber);

                // set that friend as the target friend
                CurrentFriend = Tabs[friendID];
            }  
        }

        #endregion


        #region methods/function

        //load chat history with current user and friend
        public void LoadChat(Profile friend)
        {
            // Set Current Friend
            CurrentFriend = friend;

            // load the messages with that friend
            CurrentFriend.Messages = Globals._SC.GetMessages(Globals._LoggedInUser.AccountNumber, CurrentFriend.AccountNumber);


            // Tabs
            if (!Tabs.ContainsKey(CurrentFriend.AccountNumber))
            {
                // Create New Tab
                Tabs.Add(CurrentFriend.AccountNumber, CurrentFriend);
                tabControl1.TabPages.Add(CurrentFriend.AccountNumber.ToString(), CurrentFriend.FirstName);
                
                // Switch to Tab
                tabControl1.SelectedTab = tabControl1.TabPages[CurrentFriend.AccountNumber.ToString()];

                //create text area in new tab
                ListBox LB = new ListBox();
                LB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
                LB.Location = new System.Drawing.Point(7, 7);
                LB.Name = "listBox" +CurrentFriend.AccountNumber.ToString();
                LB.HorizontalScrollbar = true;
                LB.Size = new System.Drawing.Size(530, 396);
                LB.TabIndex = 0;

                tabControl1.TabPages[CurrentFriend.AccountNumber.ToString()].Controls.Add(LB);

            }
            else
            {
                // Switch to Tab
                tabControl1.SelectedTab = tabControl1.TabPages[CurrentFriend.AccountNumber.ToString()];
            }

            tabPage2.Text = CurrentFriend.FirstName; // will have to change at some point

            // Clear Old Text
            textBox1.Text = "";

            LastRefresh = DateTime.MinValue;

        }

        //refreshes the chatwindow messages
        private void RefreshMessages()
        {
            try
            {
                while (true)
                {
                    if (CurrentFriend != null && Globals._LoggedInUser != null)
                    {
                        // Load Messages Between You and Current friend
                        CurrentFriend.Messages = Globals._SC.GetMessages(Globals._LoggedInUser.AccountNumber, CurrentFriend.AccountNumber);
                        CurrentFriend.Messages = Globals._SC.GetMessages(Globals._LoggedInUser.AccountNumber, CurrentFriend.AccountNumber);
                       
                        // Refresh Messages
                        RefreshMessagesDelegate();

                        Thread.Sleep(1000);
                    }

                }
            }catch(Exception ex)
            {      
            }
        }

        private void RefreshMessagesDelegate()
        {
            try
            {
                if (this.InvokeRequired == false)
                {
                    ListBox LB = (ListBox)tabControl1.SelectedTab.Controls["listBox" + CurrentFriend.AccountNumber.ToString()];


                    // Display Messages 
                    foreach (Utilities.ChatMessage m in CurrentFriend.Messages)
                    {
                        //  new message check
                        if (m.SentTime > LastRefresh) 
                        { 
                            string senderName = (m.AccountNumberSender == CurrentFriend.AccountNumber) ? CurrentFriend.FirstName : Globals._LoggedInUser.FirstName;

                            LB.Text = senderName + " - " + m.SentTime.ToString() + Environment.NewLine + " - " + m.Text + Environment.NewLine + Environment.NewLine;//change textbox2 to the correct textbox
                            LB.Items.Add(LB.Text);
                            LastRefresh = m.SentTime;
                        }
                    }
                }
                else
                {
                    this.Invoke(new Action(RefreshMessagesDelegate));

                }
            }catch(Exception ex)
            {

            }
           
        }


        #endregion

    }
}
