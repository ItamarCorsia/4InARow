using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourInARow
{
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity, Android.Views.View.IOnClickListener, IOnCompleteListener
    {
        Button btnBackActivity;
        EditText etName, etMail, etPwd;
        Button btnSignUp;
        FsData fsd;
        User user;
        
        private void RegisterUser()
        {
            user = new User(etName.Text, etMail.Text, etPwd.Text, false);
            fsd.CreateUser(user.Mail, user.Pwd).AddOnCompleteListener(this);
            Toast.MakeText(this, "You create new account!", ToastLength.Long).Show();         
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_signUp);
            InitViews();
            fsd = new FsData();
        }
        private void InitViews()
        {
            btnBackActivity = FindViewById<Button>(Resource.Id.btnBackActivity);
            btnBackActivity.SetOnClickListener(this);
            etName = FindViewById<EditText>(Resource.Id.etUserName);
            etMail = FindViewById<EditText>(Resource.Id.etEmail);
            etPwd = FindViewById<EditText>(Resource.Id.etPassword);
            btnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            btnSignUp.SetOnClickListener(this);          
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                Intent intent = new Intent();
                intent.PutExtra(General.KEY_NAME, user.UserName);
                intent.PutExtra(General.KEY_MAIL, user.Mail);
                intent.PutExtra(General.KEY_PWD, user.Pwd);
                SetResult(Result.Ok, intent);
                Finish();
            }
           
        }

        public void OnClick(View v)
        {
            if (v ==btnSignUp)
                RegisterUser();

            if (v == btnBackActivity)
                OpenMainActivity();
        }

        private void OpenMainActivity()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }    
}