using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceSessionAPIs.Models
{ 
    public class Session
    {
        public int id { get; set; }
        public string AccountName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string UserAccount { get; set; }

        public Session()
        {

        }

        public Session(string AccountName, string Username, string Password, string UserAccount)
        {
            this.UserAccount = UserAccount;
            this.AccountName = AccountName;
            this.Username = Username;
            this.Password = Password;
        }
    }
}