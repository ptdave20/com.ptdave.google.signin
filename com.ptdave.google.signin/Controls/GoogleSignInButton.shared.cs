using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace com.ptdave.google.signin.Controls
{
    public class GoogleSignInButton : View
    {
        public event EventHandler Clicked;

        public void Click()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        public ColorSchemeEnum ColorScheme
        {
            get => (ColorSchemeEnum)GetValue(ColorSchemeProperty);
            set => SetValue(ColorSchemeProperty, value);
        }

        public ButtonSizeEnum ButtonSize
        {
            get => (ButtonSizeEnum)GetValue(ButtonSizeProperty);
            set => SetValue(ButtonSizeProperty, value);
        }

        public static readonly BindableProperty ColorSchemeProperty = BindableProperty.Create(
            nameof(ColorScheme),
            typeof(ColorSchemeEnum),
            typeof(GoogleSignInButton),
            defaultValue: ColorSchemeEnum.Auto,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var t = bindable as GoogleSignInButton;
                t.ColorSchemeChanged?.Invoke(t, (ColorSchemeEnum)newValue);
            });


        public static readonly BindableProperty ButtonSizeProperty = BindableProperty.Create(
            nameof(ButtonSize),
            typeof(ButtonSizeEnum),
            typeof(GoogleSignInButton),
            defaultValue: ButtonSizeEnum.Standard,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var t = bindable as GoogleSignInButton;
                t.ButtonSizeChanged?.Invoke(t, (ButtonSizeEnum)newValue);
            });


        public event EventHandler<ColorSchemeEnum> ColorSchemeChanged;
        public event EventHandler<ButtonSizeEnum> ButtonSizeChanged;

        public enum ColorSchemeEnum
        {
            Auto,
            Dark,
            Light
        }

        public enum ButtonSizeEnum
        {
            IconOnly,
            Standard,
            Wide
        }
    }
}
