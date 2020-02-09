using com.ptdave.google.signin;
using com.ptdave.google.signin.Abstract;
using com.ptdave.google.signin.Data;
using com.ptdave.google.signin.Delegates;
using Foundation;
using Google.SignIn;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SignInClient))]
namespace com.ptdave.google.signin
{
    public class SignInClient : SignInDelegate, IGoogleSignIn
    {
        public event OnLoginDelegate OnLogin;
        public event OnErrorDelegate OnError;
        public event OnLogoutDelegate OnLogout;
        public event OnAuthCodeReceivedDelegate OnAuthCodeReceived;

        public override void DidSignIn(SignIn signIn, Google.SignIn.GoogleUser user, NSError error)
        {
            if (error != null)
            {
                OnError?.Invoke(this, new Error()
                {
                    Message = error.LocalizedDescription
                });
                return;
            }


            var authCode = user.ServerAuthCode;
            if(!string.IsNullOrEmpty(authCode))
            {
                OnAuthCodeReceived?.Invoke(this, authCode);
            }
            
            //OnLogin?.Invoke(this, new Abstract.Google.GoogleUser()
            //{
            //    AuthCode = idToken,
            //    Email = user.Profile.Email,
            //    FamilyName = user.Profile.FamilyName,
            //    GivenName = user.Profile.GivenName
            //});
        }

        public void Login()
        {
            Logout();
            var window = UIApplication.SharedApplication.KeyWindow;
            var presenter = window.RootViewController;
            while (presenter.PresentedViewController != null)
                presenter = presenter.PresentedViewController;
            SignIn.SharedInstance.PresentingViewController = presenter;
            SignIn.SharedInstance.SignInUser();
        }

        public void Logout()
        {
            SignIn.SharedInstance.SignOutUser();
        }

        public void SilentLogin()
        {
            
        }
    }

    class GoogleSignInIOS
    {
        static SignIn _signIn = SignIn.SharedInstance;

        public static void Initialize(string clientId, string serverClientId = "")
        {
            var client = DependencyService.Get<IGoogleSignIn>() as SignInClient;
            _signIn.ServerClientId = serverClientId;
            _signIn.ClientId = clientId;
            _signIn.Delegate = client;

        }


        public static bool HandleOpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return _signIn.HandleUrl(url);
        }

    }
}
