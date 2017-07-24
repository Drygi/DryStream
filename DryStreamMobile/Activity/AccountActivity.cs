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
using Plugin.Media.Abstractions;
using Android.Views.InputMethods;
using Plugin.Media;
using DryStreamMobile.Models;
using System.Threading.Tasks;
using DryStreamMobile.Helper;
using System.Net;
using Android.Graphics;

namespace DryStreamMobile
{
    [Activity(Label = "Moje konto", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AccountActivity : Android.App.Activity
    {
        Button imgButton, updateButton, updateValidity, deleteAccount;
        TextView validity, login;
        EditText email;
        User user;
        ImageView img;
        MediaFile mediaFile;
        ScrollView scrollView;
        LinearLayout LL;
        ProgressBar progressBar;
        bool pickPhoto;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Account);
            initControls();

        }
        void initControls()
        {
            user = new User();
            img = FindViewById<ImageView>(Resource.Id.accountCover);

            img.SetImageBitmap(GlobalHelper.GetImageBitmapFromUrl(GlobalMemory.serverAddressIP + GlobalMemory._user.CoverLink));

            imgButton = FindViewById<Button>(Resource.Id.accountCoverButton);
            imgButton.Click += ImgButton_Click;

            updateButton = FindViewById<Button>(Resource.Id.accountUpdateButton);
            updateButton.Click += UpdateButton_Click;

            updateValidity = FindViewById<Button>(Resource.Id.extendValidityButton);
            updateValidity.Click += UpdateValidity_Click;

            deleteAccount = FindViewById<Button>(Resource.Id.deleteAccount);
            deleteAccount.Click += DeleteAccount_Click;

            login = FindViewById<TextView>(Resource.Id.loginTextView);
            login.Text += GlobalMemory._user.Login;

            email = FindViewById<EditText>(Resource.Id.accountEmail);
            email.Text = GlobalMemory._user.Email.Trim();
            validity = FindViewById<TextView>(Resource.Id.validityText);

            progressBar = FindViewById<ProgressBar>(Resource.Id.accountProgressBar);
            progressBar.Visibility = ViewStates.Invisible;

            if (GlobalMemory._user.Access)
                validity.Text += GlobalMemory._user.Validity.Day + "." + GlobalMemory._user.Validity.Month + "." + GlobalMemory._user.Validity.Year;
            else
                validity.Text = "Brak dostepu";

            scrollView = FindViewById<ScrollView>(Resource.Id.AccountScrollView);
            LL = FindViewById<LinearLayout>(Resource.Id.AccountLayout);
            LL.Touch += LL_Touch;

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
        private void DeleteAccount_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Czy na pewno chcesz usunąć konto?");

            alert.SetPositiveButton("TAK", async (senderAlert, args) =>
            {
                scrollView.PageScroll(FocusSearchDirection.Down);
                progressBar.Visibility = ViewStates.Visible;
                if (await APIHelper.DeleteUser(GlobalMemory._user))
                {
                    if (GlobalHelper.isSavedUser())
                    {
                        ISharedPreferences pref = Application.Context.GetSharedPreferences("savedUser", FileCreationMode.Private);
                        ISharedPreferencesEditor edit = pref.Edit();
                        edit.Clear();
                        edit.Apply();
                    }
                    StartActivity(typeof(LoginActivity));
                    GlobalMemory._user = null;
                    this.Finish();
                }
                else
                {
                    Toast.MakeText(this, "Coś poszło nie tak", ToastLength.Long).Show();
                }
                progressBar.Visibility = ViewStates.Invisible;
            });
            alert.SetNegativeButton("NIE", (senderAlert, args) => {
                Toast.MakeText(this, "Dobry wybór", ToastLength.Long).Show();
            });
            RunOnUiThread(() => {
                alert.Show();
            });

            // Debug.WriteLine("Answer: " + answer);


        }


        private void UpdateValidity_Click(object sender, EventArgs e)
        {
           // setAlert("Dać tu albo hyperlink na serwer albo jakies info");

        }

        private async void UpdateButton_Click(object sender, EventArgs e)
        {
            progressBar.Visibility = ViewStates.Visible;
            if (isChange())
            {
                if (await validateUpdate())
                {
                    if (await updateUser())
                    {
                        Toast.MakeText(this, "Zapisano zmiany pomyślnie", ToastLength.Long).Show();
                        if (GlobalHelper.isSavedUser())
                            GlobalHelper.switchSavedUser(user);

                        GlobalMemory._user = user;
                    }
                    else
                        Toast.MakeText(this, "Coś poszło nie tak", ToastLength.Long).Show();
                }
            }
            else
                Toast.MakeText(this, "Brak zmian", ToastLength.Long).Show();

            progressBar.Visibility = ViewStates.Invisible;

        }

        private void LL_Touch(object sender, View.TouchEventArgs e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(updateButton.WindowToken, 0);
        }

        private async void ImgButton_Click(object sender, EventArgs e)
        {
            if (mediaFile != null)
                mediaFile = null;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "Nie wybrano zdjęcia", ToastLength.Long).Show();
                return;
            }
            mediaFile = await CrossMedia.Current.PickPhotoAsync();

            if (mediaFile == null)
                return;

            img.SetImageBitmap(GlobalHelper.GetImageBitmapFromUrl(mediaFile.Path));
        }

        private async Task<bool> validateUpdate()
        {
            bool validate = true;

            if (!Android.Util.Patterns.EmailAddress.Matcher(email.Text).Matches())
            {
                email.Text = "";
                email.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                Toast.MakeText(this, "Adres e-mail jest nieprawidłowy", ToastLength.Long).Show();
                validate = false;
            }
            if (await APIHelper.findEmail(email.Text))
            {
                Toast.MakeText(this, "Email " + email.Text + " jest już zajęty", ToastLength.Long).Show();
                email.Text = "";
                validate = false;
            }

            return validate;
        }
        private bool isChange()
        {
            if (email.Text.Trim() != GlobalMemory._user.Email.Trim())
                return true;
            if (mediaFile != null)
                return true;

            return false;
        }
        public async Task<bool> updateUser()
        {
            user = GlobalMemory._user;
            string oldLink;
            if (user.Email.Trim() != email.Text.Trim())
                user.Email = email.Text;

            if (mediaFile != null)
            {
                oldLink = user.CoverLink;
                user.CoverLink = await APIHelper.UploadCoverGetLink(mediaFile);
                if (user.CoverLink == "")
                    return false;
                else
                {
                    await APIHelper.deletePhoto(oldLink);
                }
            }
            if (await APIHelper.UpdateUser(user))
                return true;
            else
                return false;
        }
    }
}