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

        public static string Cypher(string Text, int Key, bool Encrypt = true)
        {
            // First convert input string to a character array. 
            // Then convert each character to it's integer representation, subtract by the key value, and reconvert back into a character. 
            // Then recast as a string.
            return new string(Text.ToCharArray().Select(x => (char)((int)x + (Encrypt ? -1 : 1) * Key)).ToArray<char>());
        }

        // Slide Functions
        public static void Slide(Form form, Size newSize, int delay = 5, int maxChange = 5) //form could be set to a control for more broad usage
        {

            // Variable Setup
            int xDirection = 1;
            int yDirection = 1;
            int xChange, yChange;

            // Current Size = 140, 100
            // Tar5get Size = 145, 100

            while (form.Size != newSize)
            {
                // Pause 
                System.Threading.Thread.Sleep(delay);

                // Direction Determination
                if ((form.Size.Width - newSize.Width) > 0) { xDirection = -1; }
                if ((form.Size.Height - newSize.Height) > 0) { yDirection = -1; }

                //Figure out change amount
                if (Math.Abs(form.Size.Width - newSize.Width) > maxChange) { xChange = maxChange; } else { xChange = 1; }
                if (Math.Abs(form.Size.Height - newSize.Height) > maxChange) { yChange = maxChange; } else { yChange = 1; }

                // Make the Change
                if (form.Size.Height != newSize.Height && form.Size.Width != newSize.Width)
                {
                    SlideDelegate(form, new Size(form.Size.Width + xChange * xDirection, form.Size.Height + yChange * yDirection));
                } 
                else if (form.Size.Height != newSize.Height)
                {
                    SlideDelegate(form, new Size(form.Size.Width, form.Size.Height + yChange * yDirection));
                }
                else if(form.Size.Width != newSize.Width)
                {
                    SlideDelegate(form, new Size(form.Size.Width + xChange * xDirection, form.Size.Height));
                }

            }
            // change arrow here
            // new delegate call

        } //(LAG)
        private static void SlideDelegate(Control form, Size newSize)
        {
            if (form.InvokeRequired == false)
            {
                form.Size = newSize;
            }
            else
            {
                form.Invoke(new Action<Form,Size>(SlideDelegate), form, newSize);
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
    }



}
