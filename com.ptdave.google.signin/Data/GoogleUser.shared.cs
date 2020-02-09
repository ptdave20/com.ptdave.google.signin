using System;
using System.Collections.Generic;
using System.Text;

namespace com.ptdave.google.signin.Data
{
    public class GoogleUser
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public string AuthCode { get; set; }

        public object Base { get; internal set; }
    }
}
