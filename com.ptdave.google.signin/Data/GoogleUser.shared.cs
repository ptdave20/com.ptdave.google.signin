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

        public string[] GrantedScopes { get; internal set; }

        public object Base { get; internal set; }
        public string[] RequestedScopes { get; internal set; }
        public string Id { get; internal set; }
        public string DisplayName { get; internal set; }
        public string IdToken { get; internal set; }
        public string PhotoUrl { get; internal set; }
    }
}
