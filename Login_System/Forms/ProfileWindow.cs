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
using Utilities;

namespace Login_System
{
    public partial class ProfileWindow : Form
    {
        // Gloabl Variables 
        private LoginWindow1 LoginWindowForm;
        private ChatWindow CW;
        private Thread FriendsRefresh;
        private Thread SlideThread;
        private Thread ExpandThread;
        private Budget_Tracker.Forms.Form1 BTForm1;

        Dictionary<int, Profile> Friends = new Dictionary<int, Profile>(); // Account Number - Profile


        #region "Events"

        // Constructor
        public ProfileWindow(LoginWindow1 LoginWindow)
        {
            LoginWindowForm = LoginWindow;
            InitializeComponent();
            SlideThread = new Thread(() => Functions.Slide(pictureBox3, new Point(pictureBox3.Location.X - pictureBox3.Size.Width, pictureBox3.Location.Y), 5, 2)); // Dummy placeholder for the purpose of initilizing the thread
            ExpandThread = new Thread(() => Functions.Grow(this, new Size(0,0), 10));
        }

        //Password TB
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            if (Globals._LoggedInUser != null)
            {
                Globals._LoggedInUser.Password = Functions.Encrypt(textBox2.Text);
                Globals._SC.Update("UPDATE [Account Logins] SET Password ='" + Globals._LoggedInUser.Password + "' WHERE AccountNumber = " + Globals._LoggedInUser.AccountNumber);
            }
        }

        //FirstName TB
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (Globals._LoggedInUser != null)
            {
                Globals._LoggedInUser.FirstName = textBox4.Text;
                Globals._SC.Update("UPDATE [Account Information] SET FirstName='" + textBox4.Text + "' WHERE AccountNumber = " + Globals._LoggedInUser.AccountNumber);
            }
        }

        //LastName TB
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (Globals._LoggedInUser != null)
            {
                Globals._LoggedInUser.LastName = textBox5.Text;
                Globals._SC.Update("UPDATE [Account Information] SET LastName='" + textBox5.Text + "' WHERE AccountNumber = " + Globals._LoggedInUser.AccountNumber);
            }
        }
        
        //Email TB
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (Globals._LoggedInUser != null)
            {
                Globals._LoggedInUser.Email = textBox6.Text;
                Globals._SC.Update("UPDATE [Account Information] SET Email='" + textBox6.Text + "' WHERE AccountNumber = " + Globals._LoggedInUser.AccountNumber);
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
            
            Globals._SC.Update("UPDATE [Account Information] SET ProfilePicture ='" + FileName + "' WHERE AccountNumber = " + Globals._LoggedInUser.AccountNumber);

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

        //profile window expand button
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Size OriginalSize = new Size(584, 616);
            Size TargetSize = new Size(843, 616);
            //Size Test = this.Size; // for testing

            // Do the expand thing
            if (this.Size == OriginalSize)
            {
                this.pictureBox3.Image = Login_System.Properties.Resources.Arrow_Right;
                ExpandThread = new Thread(() => Functions.Grow(this, TargetSize, 10));
                ExpandThread.Start();
                
            }
            else
            {
                this.pictureBox3.Image = Login_System.Properties.Resources.Arrow_Left;
                ExpandThread = new Thread(() => Functions.Grow(this, OriginalSize, 10));
                ExpandThread.Start();

            }
        }

        //makes the expand button hide and show depending on mouse location
        private void timer1_Tick(object sender, EventArgs e)
        {
            // calculate the location of the forms right side
            int mx = MousePosition.X;
            int my = MousePosition.Y;

            Point bottomRight = new Point((this.Location.X + this.Size.Width - 8), (this.Location.Y + this.Size.Height - 8));//the minus 8 is for border discrepancy

            //if mouse is located within 50 pixels of right side of the form then trigger event
            if (bottomRight.X - mx <= 50 && bottomRight.X - mx >= 0 && bottomRight.Y - my <= this.Size.Height && bottomRight.Y - my >= 0)
            {
                //mouse enter event

                //if statement so only 1 thread goes at a time
                if (SlideThread.IsAlive == false && ExpandThread.IsAlive == false)
                {
                    SlideThread = new Thread(() => Functions.Slide(pictureBox3, new Point(this.Size.Width - pictureBox3.Size.Width - 16, pictureBox3.Location.Y), 25, 3));
                    SlideThread.Start();
                }
            }
            else
            {
                //mouse leave event

                //if statement so only 1 thread goes at a time
                if (SlideThread.IsAlive)
                {
                    SlideThread.Abort();
                }
                if (ExpandThread.IsAlive == false)
                {
                    SlideThread = new Thread(() => Functions.Slide(pictureBox3, new Point(this.Size.Width, pictureBox3.Location.Y), 25, 3));
                    SlideThread.Start();
                }
            }

        }

        // open the budget tracker application
        private void button2_Click(object sender, EventArgs e)
        {
            if (BTForm1 == null)
            {
                BTForm1 = new Budget_Tracker.Forms.Form1();
                BTForm1.Show();
                BTForm1.Visible = true;
            }
            else
            {
                BTForm1.Show();
                BTForm1.Visible = true;
            }
        }

        #endregion



        #region "Functions / Methods"

        // load profile 
        public void LoadProfile()
        {

            // Populate the fields
            textBox1.Text = Globals._LoggedInUser.Username;
            textBox2.Text = Functions.Decrypt(Globals._LoggedInUser.Password);
            textBox3.Text = Globals._LoggedInUser.AccountNumber.ToString();
            textBox4.Text = Globals._LoggedInUser.FirstName;
            textBox5.Text = Globals._LoggedInUser.LastName;
            textBox6.Text = Globals._LoggedInUser.Email;
            textBox7.Text = Globals._LoggedInUser.LastLogin.ToString();
            textBox8.Text = Globals._LoggedInUser.CreatedOn.ToString();
            pictureBox1.Image = LoadImage(Globals._LoggedInUser.ProfilePicture);

            // Load Friends
            Friends = Globals._SC.PullUsers();
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
                Friends = Globals._SC.PullUsers();
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
            // abort threads
            this.FriendsRefresh.Abort();
            this.SlideThread.Abort();
            this.ExpandThread.Abort();
            BTForm1.AbortThread();

            // Log out the user (SQL)
            Globals._SC.Update("UPDATE [Account Information] SET LoggedIn = 0 WHERE AccountNumber = " + Globals._LoggedInUser.AccountNumber);
            Globals._LoggedInUser = null;

            // Clear all field values
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";

            // reset the profile window dimensions to starting size
            this.Size = new Size(584, 616);
            this.pictureBox3.Image = Login_System.Properties.Resources.Arrow_Left;

            //clear friends list
            flowLayoutPanel1.Controls.Clear();

            // Hide this form / Show login form/ close any other forms
            this.Visible = false;
            LoginWindowForm.Visible = true;
            CW.Dispose();
            CW = null;
            if (BTForm1 != null) 
            {
                BTForm1.Dispose();
                BTForm1 = null;
            }

            // reset piechartstats to zero
            Utilities.PieChartStats.PayCheck = 0;
            Utilities.PieChartStats.Refund = 0;
            Utilities.PieChartStats.OtherGain = 0;
            Utilities.PieChartStats.Rent = 0;
            Utilities.PieChartStats.Transportation = 0;
            Utilities.PieChartStats.Entertainment = 0;
            Utilities.PieChartStats.OtherExpense = 0;
            Utilities.PieChartStats.Groceries = 0;

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
                Globals._SC.Update("UPDATE [Account Information] SET ProfilePicture ='" + OFD.SafeFileName + "' WHERE AccountNumber = " + Globals._LoggedInUser.AccountNumber);

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
                FriendPanel.Size = new System.Drawing.Size(272, 81);
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
