using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_System
{

    public static class Functions
    {

        // Ceasar Cypher  A->B, B->C, C->D...
        public static string Encrypt(string EncryptInput)
        {
            string EncryptedOutput = string.Empty;
            //turn input string into char array
            char[] EncryptChar = EncryptInput.ToCharArray();
            int[] EncryptInt = new int[EncryptInput.Length];
            //turn char array into an array of ints using a for loop and shift the ints in the array over some set amount(in this case 1)
            for (int i= 0; i<EncryptInput.Length; i++)
            {
                EncryptInt[i] = ((int)EncryptChar[i])+1;
            }
            //turn int array back into char array and reconstruct string from the character array
            for (int i = 0; i<EncryptInput.Length; i++)
            {
                EncryptedOutput += Convert.ToChar(EncryptInt[i]);
            }
            //return the new string!
            return EncryptedOutput;
        }
        
        public static string Decrypt(string DecryptInput)
        {
            string DecryptedOutput = string.Empty;
            //turn input string into char array
            char[] DecryptChar = DecryptInput.ToCharArray();
            int[] DecryptInt = new int[DecryptInput.Length];
            //turn char array into an array of ints using a for loop and shift the ints in the array back some set amount(in this case 1)
            for (int i = 0; i < DecryptInput.Length; i++)
            {
                DecryptInt[i] = ((int)DecryptChar[i]) - 1;
            }
            //turn int array back into char array and reconstruct string from the character array
            for (int i = 0; i < DecryptInput.Length; i++)
            {
                DecryptedOutput += Convert.ToChar(DecryptInt[i]);
            }
            //return the new string!
            return DecryptedOutput;
        }
        
        //encrypt and decrypt all in one function!
        public static string Cypher(string Text, int Key, bool Encrypt = true)
        {
            // First convert input string to a character array. 
            // Then convert each character to it's integer representation, subtract by the key value, and reconvert back into a character. 
            // Then recast as a string.
            return new string(Text.ToCharArray().Select(x => (char)((int)x + (Encrypt ? -1 : 1) * Key)).ToArray<char>());
        }

        public static void Grow(Control ctrl, Size newSize, int delay = 5, int maxChange = 5)
        {

            // Variable Setup
            int xDirection = 1;
            int yDirection = 1;
            int xChange, yChange;

            // Current Size = 140, 100
            // Tar5get Size = 145, 100

            while (ctrl.Size != newSize)
            {
                // Pause 
                System.Threading.Thread.Sleep(delay);

                // Direction Determination
                if ((ctrl.Size.Width - newSize.Width) > 0) { xDirection = -1; }
                if ((ctrl.Size.Height - newSize.Height) > 0) { yDirection = -1; }

                //Figure out change amount
                if (Math.Abs(ctrl.Size.Width - newSize.Width) > maxChange) { xChange = maxChange; } else { xChange = 1; }
                if (Math.Abs(ctrl.Size.Height - newSize.Height) > maxChange) { yChange = maxChange; } else { yChange = 1; }


                // Make the Change
                if (ctrl.Size.Height != newSize.Height && ctrl.Size.Width != newSize.Width)
                {
                    GrowDelegate(ctrl, new Size(ctrl.Size.Width + xChange * xDirection, ctrl.Size.Height + yChange * yDirection));
                } 
                else if (ctrl.Size.Height != newSize.Height)
                {
                    GrowDelegate(ctrl, new Size(ctrl.Size.Width, ctrl.Size.Height + yChange * yDirection));
                }
                else if(ctrl.Size.Width != newSize.Width)
                {
                    GrowDelegate(ctrl, new Size(ctrl.Size.Width + xChange * xDirection, ctrl.Size.Height));
                }

            }
            // change arrow here
            // new delegate call

        }
        private static void GrowDelegate(Control form, Size newSize)
        {
            if (form.InvokeRequired == false)
            {
                form.Size = newSize;
            }
            else
            {
                form.Invoke(new Action<Form,Size>(GrowDelegate), form, newSize);
            }
        }

        //slide to the left.. slide to the right. but no hops
        public static void Slide(Control _ctrl, Point _newLocation, int delay = 5, int maxChange = 5)
        {
            // Variable Setup
            int xDirection = 1;
            int yDirection = 1;
            int xChange, yChange;

            // Current Size = 140, 100
            // Tar5get Size = 145, 100
            try
            {

                while (_ctrl.Location != _newLocation)
                {
                    // Pause 
                    System.Threading.Thread.Sleep(delay);

                    // Direction Determination
                    if ((_ctrl.Location.X - _newLocation.X) > 0) { xDirection = -1; }
                    if ((_ctrl.Location.Y - _newLocation.Y) > 0) { yDirection = -1; }

                    //Figure out change amount
                    if (Math.Abs(_ctrl.Location.X - _newLocation.X) > maxChange) { xChange = maxChange; } else { xChange = 1; }
                    if (Math.Abs(_ctrl.Location.Y - _newLocation.Y) > maxChange) { yChange = maxChange; } else { yChange = 1; }


                    // Make the Change
                    if (_ctrl.Location.Y != _newLocation.Y && _ctrl.Location.X != _newLocation.X)
                    {
                        SlideDelegate(_ctrl, new Point(_ctrl.Location.X + xChange * xDirection, _ctrl.Location.Y + yChange * yDirection));
                    }
                    else if (_ctrl.Location.Y != _newLocation.Y)
                    {
                        SlideDelegate(_ctrl, new Point(_ctrl.Location.Y, _ctrl.Location.Y + yChange * yDirection));
                    }
                    else if (_ctrl.Location.X != _newLocation.X)
                    {
                        SlideDelegate(_ctrl, new Point(_ctrl.Location.X + xChange * xDirection, _ctrl.Location.Y));
                    }
                }

            }catch(Exception Ex)
            {
                //MessageBox.Show(Ex.Message);
            }
        }
        public static void SlideDelegate(Control _ctrl, Point _newLocation)
        {
            try
            {
                if (_ctrl.InvokeRequired == false)
                {
                    _ctrl.Location = _newLocation;
                }
                else
                {
                    _ctrl.Invoke(new Action<Control, Point>(SlideDelegate), _ctrl, _newLocation);
                }
            }catch(Exception Ex)
            {

            }
        }


        public static void SaveSettings(string _Key, string _NewSetting)
        {
            // save the new setting to the settings dictionary
            if (Globals.Settings.ContainsKey(_Key))
            {
                Globals.Settings[_Key] = _NewSetting;
            }
            else
            {
                Globals.Settings.Add(_Key, _NewSetting);
            }

            // use the dictionary to compile a string of all the settings
            string _AllSettings = "";
            foreach (string Key in Globals.Settings.Keys)
            {
                _AllSettings += Key + ":" + Globals.Settings[Key] + "\n";
            }
            // save all the settings into the settings file on local system
            System.IO.File.WriteAllText("C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Settings.txt", _AllSettings);
        }

        //Load program settings
        public static void LoadSettings()
        {
            if (System.IO.File.Exists("C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Settings.txt"))
            {
                string FullSettings = System.IO.File.ReadAllText("C:\\Users\\" + System.Environment.UserName + "\\Documents\\Login_System\\Settings.txt");

                // split by new line '\n'
                string[] SplitSettings = FullSettings.Split('\n');

                // for each setting...
                foreach (string s in SplitSettings)
                {
                    if (s != "")
                    {


                        // split by ':'
                        string[] Setting = s.Split(':');

                        Globals.Settings.Add(Setting[0], Setting[1]);
                    }
                }
            }
        }


    }

    
    public class Message
    {
        public string Text { get; set; }
        public DateTime SentTime { get; set; }
        public int AccountNumberSender { get; set; }
        public int AccountNumberReceiver { get; set; }
    }

    public class Profile
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int AccountNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastLogin { get; set; }
        public string ProfilePicture { get; set; }
        public bool LoggedIn { get; set; }
        public List<Message> Messages { get; set; }
    }


    static class Globals
    {
        public static SQLControl SC;
        public static Profile LoggedInUser;
        public static Dictionary<string, string> Settings = new Dictionary<string, string>();
    }



}
