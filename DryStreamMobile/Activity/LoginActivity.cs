using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;
using DryStreamMobile.Activity;
using DryStreamMobile.Helper;
using DryStreamMobile.Models;
using Newtonsoft.Json;

namespace DryStreamMobile
{
    [Activity(Label = "LoginActivity", MainLauncher =false, Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LoginActivity : Android.App.Activity
    {
        Button loginButton, registerButton;
        EditText loginEditText, passwordEditText;
        CheckBox rememberMe;
        ProgressBar progressBar;
        LinearLayout linearLayout;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            initControls();

            // Create your application here
        }
        void initControls()
        {
            loginButton = FindViewById<Button>(Resource.Id.loginButton);
            loginButton.Click += LoginButton_Click;
            loginButton.Clickable = false;
            loginButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#9FA2A8"));

            registerButton = FindViewById<Button>(Resource.Id.registerButton);
            registerButton.Click += RegisterButton_Click;

            loginEditText = FindViewById<EditText>(Resource.Id.loginText);
            loginEditText.TextChanged += LoginEditText_TextChanged;

            passwordEditText = FindViewById<EditText>(Resource.Id.passwordText);
            passwordEditText.TextChanged += PasswordEditText_TextChanged;

            rememberMe = FindViewById<CheckBox>(Resource.Id.rememberMeBox);
            
            progressBar = FindViewById<ProgressBar>(Resource.Id.loginProgressBar);
            progressBar.Visibility = ViewStates.Invisible;

            linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout);
            linearLayout.Touch += LinearLayout_Touch;


        }

        private void LinearLayout_Touch(object sender, View.TouchEventArgs e)
        {
            //chowanie klawiatury
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(loginButton.WindowToken, 0);
        }

        private void PasswordEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (loginEditText.Text.Trim() == String.Empty || passwordEditText.Text.Trim() == String.Empty)
            {
                loginButton.Clickable = false;
                loginButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#9FA2A8"));
            }
            else
            {
                loginButton.Clickable = true;
                loginButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#375a7f"));
            }
        }

        private void LoginEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (loginEditText.Text.Trim() == String.Empty || passwordEditText.Text.Trim() == String.Empty)
            {
                loginButton.Clickable = false;
                loginButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#9FA2A8"));
            }
            else
            {
                loginButton.Clickable = true;
                loginButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#375a7f"));
            }
        }
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
            loginEditText.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
            passwordEditText.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            User u = new User();
            progressBar.Visibility = ViewStates.Visible;
            if (loginEditText.Text.Length >= 5 && passwordEditText.Text.Length >= 5)
            {
                u.Password = GlobalHelper.GenerateSHA512(passwordEditText.Text);
                u.Login = loginEditText.Text.Trim();
                var user = await APIHelper.getUser(u);

                if (user == null)
                {
                    Toast.MakeText(this, "Niepoprwany login lub hasło", ToastLength.Long).Show();
                    progressBar.Visibility = ViewStates.Invisible;
                    loginEditText.Text = "";
                    passwordEditText.Text = "";
                    loginEditText.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                    passwordEditText.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                    return;
                }
                if (rememberMe.Checked)
                {
                    if (rememberMe.Checked)
                    {
                        GlobalHelper.switchSavedUser(user);
                    }
                }

                GlobalMemory._user = user;
                if (!GlobalMemory._user.Access)
                {
                    this.StartActivity(typeof(AccessActivity));
                    this.Finish();
                }
                else
                {
                    this.StartActivity(typeof(MainPageActivity));
                    this.Finish();
                }
            }
            else
            {
                Toast.MakeText(this, "Podany login lub hasło są za krótkie!", ToastLength.Long).Show();
                loginEditText.Text = "";
                passwordEditText.Text = "";
                loginEditText.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                passwordEditText.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
            }
            progressBar.Visibility = ViewStates.Invisible;
            //hasło poddać działaniu funkcji skrótu;

        }
    }
}