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
using DryStreamMobile.Helper;
using DryStreamMobile.Models;

namespace DryStreamMobile
{
    [Activity(Label = "PasswordChangeActivity", MainLauncher = false, Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PasswordChangeActivity : Android.App.Activity
    {
        Button saveChange;
        EditText actualPassword, newPassword1, newPassword2;
        LinearLayout LL;
        ProgressBar progressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PasswordChange);
            initControls();
        }
        private void initControls()
        {
            saveChange = FindViewById<Button>(Resource.Id.saveChange);
            saveChange.Click += SaveChange_Click;

            LL = FindViewById<LinearLayout>(Resource.Id.passwordChangeLayout);
            LL.Touch += LL_Touch;

            actualPassword = FindViewById<EditText>(Resource.Id.actualPassword);
            newPassword1 = FindViewById<EditText>(Resource.Id.newPassword1);
            newPassword2 = FindViewById<EditText>(Resource.Id.newPassword2);
            progressBar = FindViewById<ProgressBar>(Resource.Id.passwordChangeProgressBar);
        }

        private void LL_Touch(object sender, View.TouchEventArgs e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(saveChange.WindowToken, 0);
        }

        private async void SaveChange_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                progressBar.Visibility = ViewStates.Visible;
                User user = GlobalMemory._user;
                user.Password = GlobalHelper.GenerateSHA512(newPassword1.Text);
               
                if (await APIHelper.UpdateUser(user))
                {
                    GlobalMemory._user = user;
                    if (GlobalHelper.isSavedUser())
                        GlobalHelper.switchSavedUser(user);

                    setAlert("Hasło zostało zmienione!");
                    //przejscie do MENU jakiegos
                }
            }
            progressBar.Visibility = ViewStates.Invisible;

        }

        private bool validate()
        {
            bool valid = true;

            if (GlobalHelper.GenerateSHA512(actualPassword.Text) == GlobalMemory._user.Password)
            {
                if (GlobalHelper.GenerateSHA512(newPassword1.Text) == GlobalMemory._user.Password)
                {
                    actualPassword.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                    actualPassword.Text = "";
                    setAlert("Podane hasło jest identyczne z aktualnym!");
                    valid = false;
                }
                else
                {
                    if (newPassword1.Text != newPassword2.Text)
                    {
                        newPassword1.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                        newPassword2.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                        newPassword1.Text = "";
                        newPassword2.Text = "";
                        setAlert("Podane hasła muszą być identyncze!");
                        valid = false;
                    }
                    if (newPassword1.Text.Length < 5)
                    {
                        newPassword1.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                        newPassword2.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                        newPassword1.Text = "";
                        newPassword2.Text = "";
                        setAlert("Haslo musi mieć minimum 5 znaków!");
                        valid = false;
                    }
                }

            }
           
            return valid;
        }

        private void setAlert(string message)
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alertDialog = alert.Create();
            alertDialog.SetTitle(message);
            alertDialog.Show();
        }
    }
}