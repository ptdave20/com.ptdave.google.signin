using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using com.ptdave.google.signin;
using com.ptdave.google.signin.Abstract;
using com.ptdave.google.signin.Delegates;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

using Android.Gms.Tasks;
using System.Linq;
using Xamarin.Forms.Internals;
using Android.Gms.Common.Apis;

[assembly: Dependency(typeof(SignInClient))]
namespace com.ptdave.google.signin
{
    [Preserve(AllMembers = true)]
    public class SignInClient : IGoogleSignIn
    {
        public event OnLoginDelegate OnLogin;
        public event OnErrorDelegate OnError;
        public event OnLogoutDelegate OnLogout;
        public event OnAuthCodeReceivedDelegate OnAuthCodeReceived;
        public event OnRevokedDelegate OnRevoked;

        GoogleSignInClient googleSignInClient;
        GoogleSignInOptions googleSignInOptions;
        Activity Activity;

        public SignInClient()
        {

        }

        public void Initialize(Activity activity, string serverClientId)
        {

            var builder = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestEmail();
            
            if(!string.IsNullOrEmpty(serverClientId))
            {
               builder = builder.RequestServerAuthCode(serverClientId);
            } else
            {

            }
            googleSignInOptions = builder.Build();


            googleSignInClient = GoogleSignIn.GetClient(activity, googleSignInOptions);
            Activity = activity;
        }

        public void HandleSignIn(Android.Gms.Tasks.Task task)
        {
            try
            {
                var result = task.Result as GoogleSignInAccount;

                if (string.IsNullOrEmpty(result.ServerAuthCode))
                {
                    OnAuthCodeReceived?.Invoke(this, result.ServerAuthCode);
                }
                else
                {
                    OnLogin?.Invoke(this, new Data.GoogleUser()
                    {
                        Base = result,
                        AuthCode = result.ServerAuthCode,
                        Email = result.Email,
                        FamilyName = result.FamilyName,
                        GivenName = result.GivenName,
                        GrantedScopes = result.GrantedScopes.Select(x => x.ScopeUri).ToArray(),
                        RequestedScopes = result.RequestedScopes.Select(x => x.ScopeUri).ToArray(),
                        Id = result.Id,
                        DisplayName = result.DisplayName,
                        IdToken = result.IdToken,
                        PhotoUrl = result.PhotoUrl.ToString(),
                    });
                }
            }
            catch(ApiException ex)
            {
                OnError?.Invoke(this, new Data.Error()
                {
                    Message = ex.Message,
                });
            }
            
            
        }

        public void Login()
        {
            if (HasPreviousSignIn())
                Logout();
            Intent signInIntent = googleSignInClient.SignInIntent;
            Activity.StartActivityForResult(signInIntent, GoogleSignInAndroid.SIGNIN_RESP);
        }

        public void Logout()
        {
            googleSignInClient.SignOut().AddOnCompleteListener(Activity, new CompleteHandler(a => OnLogout?.Invoke(this)));
        }

        public void SilentLogin()
        {
            googleSignInClient.SilentSignIn().AddOnCompleteListener(Activity, new CompleteHandler(a => HandleSignIn(a)));
        }

        public void Revoke()
        {
            googleSignInClient.RevokeAccess().AddOnCompleteListener(Activity, new CompleteHandler(a => OnRevoked?.Invoke(this,null)));
        }

        public bool HasPreviousSignIn()
        {
            return GoogleSignIn.GetLastSignedInAccount(Activity) != null;
        }

        private class CompleteHandler : Java.Lang.Object, IOnCompleteListener
        {
            Action<Task> toCall;
            public CompleteHandler(Action<Task> action)
            {
                toCall = action;
            }

            public void OnComplete(Task task)
            {
                toCall(task);
            }
        }
    }

    public class GoogleSignInAndroid
    {
        public const int SIGNIN_RESP = 9897;

        private static SignInClient _client;

        public static void Initialize(Activity activity, string serverClientId = "")
        {
            _client = DependencyService.Get<IGoogleSignIn>() as SignInClient;
            _client.Initialize(activity, serverClientId);
        }

        public static void ActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == SIGNIN_RESP)
            {
                var task = GoogleSignIn.GetSignedInAccountFromIntent(data);
                _client.HandleSignIn(task);
            }
        }
    }
}
