using com.ptdave.google.signin.Delegates;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.ptdave.google.signin.Abstract
{
    public interface IGoogleSignIn
    {
        void Login();
        void SilentLogin();
        void Logout();
        void Revoke();

        bool HasPreviousSignIn();
        

        event OnLoginDelegate OnLogin;
        event OnErrorDelegate OnError;
        event OnLogoutDelegate OnLogout;
        event OnAuthCodeReceivedDelegate OnAuthCodeReceived;
        event OnRevokedDelegate OnRevoked;
    }
}
