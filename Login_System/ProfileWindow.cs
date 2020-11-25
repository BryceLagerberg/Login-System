using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Login_System
{
    public partial class ProfileWindow : Form
    {
        // Gloabl Variables 
        private LoginWindow1 LoginWindowForm;
        private ChatWindow CW;
        private Thread FriendsRefresh;

        Dictionary<int, Profile> Friends = new Dictionary<int, Profile>(); // Account Number - Profile


        #region "Events"

        // Constructor
        public ProfileWindow(LoginWindow1 LoginWindow)
        {
            LoginWindowForm = LoginWindow;
            InitializeComponent();

        }

        private void ProfileWindow_Load(object sender, EventArgs e)
        {
            
        }

        //Password TB
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            if (Globals.LoggedInUser != null)
            {
                Globals.LoggedInUser.Password = Functions.Encrypt(textBox2.Text);
                Globals.SC.Update("UPDATE [Account Logins] SET Password ='" + Globals.LoggedInUser.Password + "' WHERE AccountNumber = " + Globals.LoggedInUser.AccountNumber);
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

        //right click menu browse button for profile pictures
        private void browseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ImageBrowse();
        }

        //right click menu clear button for profile pictures
        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Login_System.Properties.Resources.Deafult_Profile_Pitcher;
        }

        // Closing Out
        private void ProfileWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logout();
            LoginWindowForm.Close();
        }

        //Chat window open event
        private void FriendDoubleClick(object sender, EventArgs e)
        {

            CW.Show();

            // Load friend profile
            Control control = (Control)sender;
            int AccountNumber = Int32.Parse(control.Name.Replace("panel", "").Replace("label", "").Replace("pictureBox", ""));
            Profile friend = Friends[AccountNumber];
            CW.LoadChat(friend);


            CW.Visible = true;
        }

        //drag and drop picture event
        private void pictureBox1_DragDropEvent(object sender, DragEventArgs e)
        {
            String[] DroppedFiles = (String[])e.Data.GetData(DataFormats.FileDrop);
            string FilePath = DroppedFiles[0];
            string FileName = FilePath.Split('\\')[FilePath.Split('\\').Length - 1];

            // C:Users\bryce\Pictures\water1.jpg

            // Set the new image
            pictureBox1.ImageLocation = FilePath;
            
            Globals.SC.Update("UPDATE [Account Information] SET ProfilePicture ='" + FileName + "' WHERE AccountNumber = " + Globals.LoggedInUser.AccountNumber);

            // Add a copy of the image to the images folder for quicker reload
            if (!System.IO.File.Exists("C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Profile_Pictures\\" + FileName))
            {
                System.IO.File.Copy(FilePath, "C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Profile_Pictures\\" + FileName, false);
            }
        }
        
        //drag enter event
        private void PictureBox1_DragEnterEvent(object sender, DragEventArgs e)
        {

            // See if this is a copy and the data includes an image.
            if (e.Data.GetDataPresent(DataFormats.FileDrop) &&
                (e.AllowedEffect & DragDropEffects.Copy) != 0)
            {
                // Allow this.
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                // Don't allow any other drop.
                e.Effect = DragDropEffects.None;
            }
        }

        //chat window expand button
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Size OriginalSize = new Size(624, 616);
            Size TargetSize = new Size(923, 616);
            //Size Test = this.Size; // for testing


            // Do the expand thing
            if (this.Size == OriginalSize)
            {
                this.pictureBox3.Image = Login_System.Properties.Resources.Arrow_Right;
                Thread SlideThread = new Thread(() => Functions.Slide(this, TargetSize, 10));
                SlideThread.Start();
                
            }
            else
            {
                this.pictureBox3.Image = Login_System.Properties.Resources.Arrow_Left;
                Thread SlideThread = new Thread(() => Functions.Slide(this, OriginalSize, 10));
                SlideThread.Start();
            }
            
            
            //then make it slide
            //then make it more cool
        }

        #endregion



        #region "Functions / Methods"

        // load profile 
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
            pictureBox1.Image = LoadImage(Globals.LoggedInUser.ProfilePicture);

            // Load Friends
            Friends = Globals.SC.PullUsers();
            PopulateFriendsList();

            // Create a new chat window instance
            CW = new ChatWindow();

            // Create instance of friends list refresh thread
            FriendsRefresh = new Thread(FriendsStatus);
            FriendsRefresh.IsBackground = true;
            FriendsRefresh.Start();

        }

        // gets the current status of your friends
        public void FriendsStatus()
        {
            
            while (true)
            {
                // make a call to check the LoggedIn status of the current users friends
                Friends = Globals.SC.PullUsers();
                Thread.Sleep(2500);

                // Update their status 
                FriendsStatusDelegate();
            }
        }

        // the thread version of friendsstatus or whatever... blah bhalllllllllllllllll 
        private void FriendsStatusDelegate()
        {
            try
            {
                if (this.InvokeRequired == false)
                {
                    // Update LoggedIn Labels
                    foreach (Profile friend in Friends.Values)
                    {

                        if (flowLayoutPanel1.Controls.Count > 0)
                        {
                            Label L = (Label)flowLayoutPanel1.Controls["panel" + friend.AccountNumber].Controls["LoggedIn" + friend.AccountNumber];
                            L.Text = friend.LoggedIn ? "online" : "offline";
                            L.ForeColor = friend.LoggedIn ? Color.LimeGreen : Color.Red;
                        }             
                    }
                }
                else
                {
                    this.Invoke(new Action(FriendsStatusDelegate));
                }
            }catch(Exception ex)
            {

            }
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

            //clear friends list
            flowLayoutPanel1.Controls.Clear();

            // Hide this form / Show login form
            this.Visible = false;
            LoginWindowForm.Visible = true;
            CW.Dispose();
            CW = null;
            
        }
        // browse for profile picture
        public void ImageBrowse()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = "Profile Image Browser";
            OFD.Multiselect = false;
            OFD.InitialDirectory = "C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Profile_Pictures";
            OFD.Filter = "JPG files|*.jpg|All files|*.*|Aaron|Aaron*.*";

            // Open file dialog and if result is OK NOT Cancle 
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                // Set the new image
                pictureBox1.ImageLocation = OFD.FileName;
                Globals.SC.Update("UPDATE [Account Information] SET ProfilePicture ='" + OFD.SafeFileName + "' WHERE AccountNumber = " + Globals.LoggedInUser.AccountNumber);

                // Add a copy of the image to the images folder for quicker reload
                if (!System.IO.File.Exists("C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Profile_Pictures\\" + OFD.SafeFileName))
                {
                    System.IO.File.Copy(OFD.FileName, "C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Profile_Pictures\\" + OFD.SafeFileName, false);
                }

            }
        }

        // Populates the friends list
        private void PopulateFriendsList()
        {

            foreach (Profile friend in Friends.Values)
            {
                Panel FriendPanel = new Panel();
                Label FirstName = new Label();
                Label LoggedIn = new Label();
                PictureBox ProfilePicture = new PictureBox();



                flowLayoutPanel1.Controls.Add(FriendPanel);

                FriendPanel.BackColor = System.Drawing.Color.Gray;
                FriendPanel.Controls.Add(LoggedIn);
                FriendPanel.Controls.Add(FirstName);
                FriendPanel.Controls.Add(ProfilePicture);
                FriendPanel.Location = new System.Drawing.Point(20, 400);
                FriendPanel.Name = "panel" + friend.AccountNumber;  //should be changed to accountnumber instead of firstname
                FriendPanel.Size = new System.Drawing.Size(253, 81);
                FriendPanel.TabIndex = 1;
                FriendPanel.Visible = true;
                FriendPanel.DoubleClick += new System.EventHandler(this.FriendDoubleClick);



                ProfilePicture.Image = LoadImage(friend.ProfilePicture);
                ProfilePicture.Location = new System.Drawing.Point(8, 8);
                ProfilePicture.Name = "pictureBox" + friend.AccountNumber;  //should be changed to accountnumber instead of firstname
                ProfilePicture.Size = new System.Drawing.Size(60, 65);
                ProfilePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                ProfilePicture.TabIndex = 0;
                ProfilePicture.TabStop = false;

                FirstName.AutoSize = true;
                FirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
                FirstName.Location = new System.Drawing.Point(84, 8);
                FirstName.Name = "FirstName" + friend.AccountNumber;  //should be changed to accountnumber instead of firstname
                FirstName.Size = new System.Drawing.Size(51, 20);
                FirstName.TabIndex = 1;
                FirstName.Text = friend.FirstName + " (" + friend.AccountNumber + ")";

                LoggedIn.AutoSize = true;
                LoggedIn.Location = new System.Drawing.Point(86, 49);
                LoggedIn.Name = "LoggedIn" + friend.AccountNumber; //should be changed to accountnumber instead of firstname
                LoggedIn.Size = new System.Drawing.Size(52, 13);
                LoggedIn.TabIndex = 2;
                LoggedIn.Text = friend.LoggedIn ? "online" : "offline";
                LoggedIn.ForeColor = friend.LoggedIn ? Color.LimeGreen : Color.Red;


            }
        }

        // Loads Profile Image
        private Image LoadImage(string imageName)
        {
            Image ProfileImage;

            if(System.IO.File.Exists("C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Profile_Pictures\\" + imageName))
            {
               ProfileImage = Image.FromFile("C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Profile_Pictures\\" + imageName);
            }
            else
            {
                ProfileImage = Login_System.Properties.Resources.Deafult_Profile_Pitcher;
            }
            return ProfileImage;
        }



        #endregion
    }
}
