using com.ptdave.google.signin;
using com.ptdave.google.signin.Abstract;
using com.ptdave.google.signin.Data;
using com.ptdave.google.signin.Delegates;
using Foundation;
using Google.SignIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;


[assembly: Dependency(typeof(SignInClient))]
namespace com.ptdave.google.signin
{
    [Preserve(AllMembers = true)]
    public class SignInClient : SignInDelegate , IGoogleSignIn
    {
        public event OnLoginDelegate OnLogin;
        public event OnErrorDelegate OnError;
        public event OnLogoutDelegate OnLogout;
        public event OnAuthCodeReceivedDelegate OnAuthCodeReceived;
        public event OnRevokedDelegate OnRevoked;

        public SignInClient()
        {
            SignIn.SharedInstance.Disconnected += SharedInstance_Disconnected;
        }

        private void SharedInstance_Disconnected(object sender, SignInDelegateEventArgs e)
        {
            if(e.Error != null)
            {
                OnError?.Invoke(this, new Error()
                {
                    Message = e.Error.LocalizedDescription
                });
            } else
            {
                OnRevoked?.Invoke(this, new Data.GoogleUser()
                {
                    Base = e.User,
                    Id= e.User?.UserId,
                    AuthCode = "",
                    DisplayName = e.User?.Profile?.Name,
                    Email = e.User?.Profile?.Email,
                    FamilyName = e.User?.Profile?.FamilyName,
                    GivenName = e.User?.Profile?.GivenName,
                    GrantedScopes = new string[] { },
                    IdToken = e.User?.Authentication?.IdToken,
                    PhotoUrl = "",
                    RequestedScopes = new string[] { }
                });
            }
        }

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
            } else
            {
                OnLogin?.Invoke(this, new Data.GoogleUser()
                {
                    Id = user.UserId,
                    DisplayName = user.Profile.Name,
                    IdToken = user.Authentication.IdToken,
                    Base = user,
                    AuthCode = user.ServerAuthCode,
                    Email = user.Profile.Email,
                    FamilyName = user.Profile.Email,
                    GivenName = user.Profile.GivenName,
                    GrantedScopes = user.GrantedScopes.ToArray(),
                    RequestedScopes = new string[] {},
                    PhotoUrl = user.Profile.GetImageUrl(512).ToString(),
                });
            }
        }

        public void Login()
        {
            SignIn.SharedInstance.PresentingViewController = GetPresenter();
            SignIn.SharedInstance.SignInUser();
        }

        private UIViewController GetPresenter()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var presenter = window.RootViewController;
            while (presenter.PresentedViewController != null)
                presenter = presenter.PresentedViewController;
            return presenter;
        }

        public void Logout()
        {
            SignIn.SharedInstance.SignOutUser();
        }

        public void Revoke()
        {
            SignIn.SharedInstance.DisconnectUser();
        }

        public void SilentLogin()
        {
            if(SignIn.SharedInstance.HasPreviousSignIn)
                SignIn.SharedInstance.RestorePreviousSignIn();
        }

        public bool HasPreviousSignIn()
        {
            return SignIn.SharedInstance.HasPreviousSignIn;
        }
    }

    public class GoogleSignInIOS
    {
        static SignIn _signIn = SignIn.SharedInstance;

        public static void Initialize(string clientId, string serverClientId = "")
        {
            var client = DependencyService.Get<IGoogleSignIn>() as SignInClient;
            if(!string.IsNullOrEmpty(serverClientId))
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
