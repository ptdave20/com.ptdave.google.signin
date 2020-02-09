using com.ptdave.google.signin.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.ptdave.google.signin.Delegates
{
    public delegate void OnLoginDelegate(object sender, GoogleUser googleUser);
    public delegate void OnErrorDelegate(object sender, Error error);
    public delegate void OnLogoutDelegate(object sender);
    public delegate void OnAuthCodeReceivedDelegate(object sender, string code);
    public delegate void OnRevokedDelegate(object sender, GoogleUser googleUser);
}
