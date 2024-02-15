using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungerSolutions.Profile.Models
{
    internal class UserModel
    {
        public string emailAddress { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string username { get; set; }
        public int userAccountID { get; set; }
    }
}
