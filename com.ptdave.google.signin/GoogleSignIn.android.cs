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

[assembly: Dependency(typeof(SignInClient))]
namespace com.ptdave.google.signin
{
    public class SignInClient : IGoogleSignIn
    {
        public event OnLoginDelegate OnLogin;
        public event OnErrorDelegate OnError;
        public event OnLogoutDelegate OnLogout;
        public event OnAuthCodeReceivedDelegate OnAuthCodeReceived;


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
                builder.RequestServerAuthCode(serverClientId);
            }
            googleSignInOptions = builder.Build();


            googleSignInClient = GoogleSignIn.GetClient(activity, googleSignInOptions);
            Activity = activity;
        }

        public void HandleSignIn(Android.Gms.Tasks.Task task)
        {
            var result = task.Result as GoogleSignInAccount;
            
            if(string.IsNullOrEmpty(result.ServerAuthCode))
            {
                OnAuthCodeReceived?.Invoke(this, result.ServerAuthCode);
            }
            
        }

        public void Login()
        {
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
