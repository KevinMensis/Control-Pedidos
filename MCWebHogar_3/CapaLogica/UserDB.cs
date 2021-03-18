using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaLogica
{
    public class UserDB
    {
        public UserDB(string conectionCEM, string User)
        {
            dbCEM = conectionCEM;
            UserLogin = User;
        }
        public string dbSIAWIN { get; set; }
        public string dbCEM { get; set; }
        public string UserLogin { get; set; }
    }
}