using com.ptdave.google.signin;
using com.ptdave.google.signin.Controls;
using Google.SignIn;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GoogleSignInButton), typeof(GoogleSignInButtonIOS))]
namespace com.ptdave.google.signin
{
    [Preserve(AllMembers = true)]
    public class GoogleSignInButtonIOS : ViewRenderer<GoogleSignInButton, SignInButton>
    {
        SignInButton button;
        protected override void OnElementChanged(ElementChangedEventArgs<GoogleSignInButton> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                try
                {
                    button.TouchUpInside -= Button_TouchUpInside;
                    Element.ColorSchemeChanged -= Element_ColorSchemeChanged;
                    Element.ButtonSizeChanged -= Element_ButtonSizeChanged;
                }
                catch (Exception)
                {

                }

            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    button = new SignInButton();
                    try
                    {
                        var window = UIApplication.SharedApplication.KeyWindow;
                        var presenter = window.RootViewController;
                        while (presenter.PresentedViewController != null)
                            presenter = presenter.PresentedViewController;
                        SignIn.SharedInstance.PresentingViewController = presenter;
                    }
                    catch (Exception)
                    {

                    }
                    SetNativeControl(button);
                }
                button.TouchUpInside += Button_TouchUpInside;
                Element.ColorSchemeChanged += Element_ColorSchemeChanged;
                Element.ButtonSizeChanged += Element_ButtonSizeChanged;
            }

            switch (Element?.ColorScheme)
            {
                case GoogleSignInButton.ColorSchemeEnum.Auto:
                    button.ColorScheme = ButtonColorScheme.Light;
                    break;
                case GoogleSignInButton.ColorSchemeEnum.Dark:
                    button.ColorScheme = ButtonColorScheme.Dark;
                    break;
                case GoogleSignInButton.ColorSchemeEnum.Light:
                    button.ColorScheme = ButtonColorScheme.Light;
                    break;
            }
            switch (Element?.ButtonSize)
            {
                case GoogleSignInButton.ButtonSizeEnum.IconOnly:
                    button.Style = ButtonStyle.IconOnly;
                    break;
                case GoogleSignInButton.ButtonSizeEnum.Standard:
                    button.Style = ButtonStyle.Standard;
                    break;
                case GoogleSignInButton.ButtonSizeEnum.Wide:
                    button.Style = ButtonStyle.Wide;
                    break;
            }
        }

        private void Element_ButtonSizeChanged(object sender, GoogleSignInButton.ButtonSizeEnum e)
        {

            switch (e)
            {
                case GoogleSignInButton.ButtonSizeEnum.IconOnly:
                    button.Style = ButtonStyle.IconOnly;
                    break;
                case GoogleSignInButton.ButtonSizeEnum.Standard:
                    button.Style = ButtonStyle.Standard;
                    break;
                case GoogleSignInButton.ButtonSizeEnum.Wide:
                    button.Style = ButtonStyle.Wide;
                    break;
            }
        }

        private void Element_ColorSchemeChanged(object sender, GoogleSignInButton.ColorSchemeEnum e)
        {
            switch (e)
            {
                case GoogleSignInButton.ColorSchemeEnum.Auto:
                    button.ColorScheme = ButtonColorScheme.Light;
                    break;
                case GoogleSignInButton.ColorSchemeEnum.Dark:
                    button.ColorScheme = ButtonColorScheme.Dark;
                    break;
                case GoogleSignInButton.ColorSchemeEnum.Light:
                    button.ColorScheme = ButtonColorScheme.Light;
                    break;
            }
        }

        private void Button_TouchUpInside(object sender, EventArgs e)
        {

        }
    }
}
