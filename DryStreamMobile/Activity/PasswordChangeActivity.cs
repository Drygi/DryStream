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
    [Activity(Label = "Zmień hasło", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
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


            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
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

                    Toast.MakeText(this, "Hasło zostało zmienione", ToastLength.Long).Show();
                    //przejscie do MENU jakiego
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
                    Toast.MakeText(this, "Podane hasło jest identyczne z aktualnym!", ToastLength.Long).Show();
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
                        Toast.MakeText(this, "Podane hasła muszą być identyncze!", ToastLength.Long).Show();
                        valid = false;
                    }
                    if (newPassword1.Text.Length < 5)
                    {
                        newPassword1.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                        newPassword2.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                        newPassword1.Text = "";
                        newPassword2.Text = "";
                        Toast.MakeText(this, "Haslo musi mieć minimum 5 znaków!", ToastLength.Long).Show();
            
                        valid = false;
                    }
                }

            }
            progressBar.Visibility = ViewStates.Invisible;
            return valid;
        }
    }
}