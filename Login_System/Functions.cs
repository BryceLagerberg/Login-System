using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_System
{
    class Functions
    {
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
    }

}
