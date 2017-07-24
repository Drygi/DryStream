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
using Java.IO;
using Android.Database;
using Plugin.Media;
using System.Net.Http;
using Plugin.Media.Abstractions;
using DryStreamMobile.Models;
using DryStreamMobile.Helper;
using System.Threading.Tasks;
using Android.Views.InputMethods;
//
namespace DryStreamMobile
{
    [Activity(Label = "RegisterActivity", MainLauncher = false, Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RegisterActivity : Android.App.Activity
    {
        Button imageButton, registerButton;
        EditText loginTxt, passwordTxt, password2Txt, email;
        MediaFile _mediaFile;
        ScrollView scroll;
        LinearLayout LL;
        ProgressBar progressBar;
        bool pickPhoto = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);
            initControls();
            // Create your application here
        }
        void initControls()
        {
   
            imageButton = FindViewById<Button>(Resource.Id.imgButton);
            imageButton.Click += ImageButton_Click;
            registerButton = FindViewById<Button>(Resource.Id.registerClick);
            registerButton.Click += async delegate
            {
                progressBar.Visibility = ViewStates.Visible;
                if (validateRegister())
                {
                    if (await APIHelper.findLogin(loginTxt.Text.Trim()))
                    {
                        progressBar.Visibility = ViewStates.Invisible;
                        Toast.MakeText(this, "Login "+ loginTxt.Text+" jest już zajęty", ToastLength.Long).Show();
                        loginTxt.Text = "";
                    }
                    else if(await APIHelper.findEmail(email.Text.Trim()))
                    {
                        progressBar.Visibility = ViewStates.Invisible;
                        Toast.MakeText(this, "Email " + email.Text + " jest już zajęty", ToastLength.Long).Show();
                        email.Text = "";
                    }
                    else
                    {
                        if (await addUser())
                        {
                            Toast.MakeText(this, "Pomyślnie zarejestrowano!", ToastLength.Long).Show();
                            StartActivity(typeof(LoginActivity));
                            this.Finish();
                        }
                        else
                        {
                            progressBar.Visibility = ViewStates.Invisible;
                            Toast.MakeText(this, "Coś poszło nie tak!", ToastLength.Long).Show();
                        }
                    }
                    progressBar.Visibility = ViewStates.Invisible;
                }
                progressBar.Visibility = ViewStates.Invisible;
            };

            loginTxt = FindViewById<EditText>(Resource.Id.registerLogin);
            passwordTxt = FindViewById<EditText>(Resource.Id.registerPassword);
            password2Txt = FindViewById<EditText>(Resource.Id.registerPassword2);
            email = FindViewById<EditText>(Resource.Id.registerEmail);

            scroll = FindViewById<ScrollView>(Resource.Id.scrollViewRegister);
            LL = FindViewById<LinearLayout>(Resource.Id.LiID);
            LL.Touch += LL_Touch;

            progressBar = FindViewById<ProgressBar>(Resource.Id.registerProgressBar);
            progressBar.Visibility = ViewStates.Invisible;
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(registerButton.WindowToken, 0);
        }

        private void LL_Touch(object sender, View.TouchEventArgs e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(registerButton.WindowToken, 0); 
        }

  

        private async void ImageButton_Click(object sender, EventArgs e)
        {
            if (_mediaFile != null)
            {
                _mediaFile = null;
                pickPhoto = false; 
            }
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "Nie wybrano zdjęcia", ToastLength.Long).Show();
                return;
            }
             _mediaFile = await CrossMedia.Current.PickPhotoAsync();

            if (_mediaFile == null)
                return;
            else
            {
                imageButton.Text = "Wybrano zdjęcie";
                pickPhoto = true;
            }
        }

   

        private bool validateRegister()
        {
            bool validate = true;
            if(loginTxt.Text.Trim().Length < 5)
            {
                loginTxt.Text = "";
                loginTxt.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                Toast.MakeText(this, "Login jest za krótki!", ToastLength.Long).Show();
                validate = false;
            }
            if ((password2Txt.Text != passwordTxt.Text))
            {
                passwordTxt.Text = "";
                password2Txt.Text = "";
               // registerBar.Visibility = ViewStates.Invisible;
                passwordTxt.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                password2Txt.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                validate = false;
                Toast.MakeText(this, "Podane hasła są różne!", ToastLength.Long).Show();
            }
            if (passwordTxt.Text.Length<5)
            {
                passwordTxt.Text = "";
                password2Txt.Text = "";
                // registerBar.Visibility = ViewStates.Invisible;
                passwordTxt.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                password2Txt.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                Toast.MakeText(this, "Hasło jest krótsze niż 5 znaków!", ToastLength.Long).Show();
                validate = false;
            }
            if (!Android.Util.Patterns.EmailAddress.Matcher(email.Text.Trim()).Matches())
            {
                email.Text = "";
                email.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.editTextBorder));
                Toast.MakeText(this, "Adres email jest nieprawidłowy!", ToastLength.Long).Show();
                validate = false;
            }
            if(_mediaFile==null)
            {
                Toast.MakeText(this, "Nie wybrano zdjęcia!", ToastLength.Long).Show();
                validate = false;
            }
            if(!validate)
              scroll.SmoothScrollTo(0, 0);

            progressBar.Visibility = ViewStates.Invisible;
            return validate;
        }
        private async Task<bool> addUser()
        {
            User user = new User();


            user.Login = loginTxt.Text.Trim();
            user.Password = GlobalHelper.GenerateSHA512(passwordTxt.Text);
            user.Email = email.Text.Trim();
            user.CoverLink = await APIHelper.UploadCoverGetLink(_mediaFile);
            
            return await APIHelper.PostUser(user);
        }
    }
}