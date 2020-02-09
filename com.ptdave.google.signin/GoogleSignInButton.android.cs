using Android.Content;
using Android.Gms.Common;
using com.ptdave.google.signin;
using com.ptdave.google.signin.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GoogleSignInButton), typeof(GoogleSignInButtonAndroid))]
namespace com.ptdave.google.signin
{
    [Preserve(AllMembers = true)]
    public class GoogleSignInButtonAndroid : ViewRenderer<GoogleSignInButton, SignInButton>
    {
        public GoogleSignInButtonAndroid(Context context) : base(context)
        {

        }
        private SignInButton button;
        protected override void OnElementChanged(ElementChangedEventArgs<GoogleSignInButton> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                button.Click -= SignInButton_Click;
                Element.ColorSchemeChanged -= Element_ColorSchemeChanged;
                Element.ButtonSizeChanged -= Element_ButtonSizeChanged;
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    button = new SignInButton(Context);
                    SetNativeControl(button);
                }
                button.Click += SignInButton_Click;
                Element.ColorSchemeChanged += Element_ColorSchemeChanged;
                Element.ButtonSizeChanged += Element_ButtonSizeChanged;
                switch (Element.ColorScheme)
                {
                    case GoogleSignInButton.ColorSchemeEnum.Auto:
                        button.SetColorScheme(SignInButton.ColorAuto);
                        break;
                    case GoogleSignInButton.ColorSchemeEnum.Dark:
                        button.SetColorScheme(SignInButton.ColorDark);
                        break;
                    case GoogleSignInButton.ColorSchemeEnum.Light:
                        button.SetColorScheme(SignInButton.ColorLight);
                        break;
                }
                switch (Element.ButtonSize)
                {
                    case GoogleSignInButton.ButtonSizeEnum.IconOnly:
                        button.SetSize(SignInButton.SizeIconOnly);
                        break;
                    case GoogleSignInButton.ButtonSizeEnum.Standard:
                        button.SetSize(SignInButton.SizeStandard);
                        break;
                    case GoogleSignInButton.ButtonSizeEnum.Wide:
                        button.SetSize(SignInButton.SizeWide);
                        break;
                }
            }

        }

        private void Element_ButtonSizeChanged(object sender, GoogleSignInButton.ButtonSizeEnum e)
        {
            switch (e)
            {
                case GoogleSignInButton.ButtonSizeEnum.IconOnly:
                    button.SetSize(SignInButton.SizeIconOnly);
                    break;
                case GoogleSignInButton.ButtonSizeEnum.Standard:
                    button.SetSize(SignInButton.SizeStandard);
                    break;
                case GoogleSignInButton.ButtonSizeEnum.Wide:
                    button.SetSize(SignInButton.SizeWide);
                    break;
            }
        }

        private void Element_ColorSchemeChanged(object sender, GoogleSignInButton.ColorSchemeEnum e)
        {
            switch (e)
            {
                case GoogleSignInButton.ColorSchemeEnum.Auto:
                    button.SetColorScheme(SignInButton.ColorAuto);
                    break;
                case GoogleSignInButton.ColorSchemeEnum.Dark:
                    button.SetColorScheme(SignInButton.ColorDark);
                    break;
                case GoogleSignInButton.ColorSchemeEnum.Light:
                    button.SetColorScheme(SignInButton.ColorLight);
                    break;
            }
        }

        private void SignInButton_Click(object sender, EventArgs e)
        {
            this.Element.Click();
        }
    }
}
