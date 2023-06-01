using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourInARow
{
    [Activity(Label = "ProfileActivity")]
    public class ProfileActivity : Activity, Android.Views.View.IOnClickListener
    {
        EditText etName, etMail, etPwd;
        Button btnChangeDetails, btnBackActivity;
        SpData spd;
        FsData fsd;
        User user;
        string action;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_Profile);
            InitViews();
            InitObjects();
            if (user.Exist)
                ShowUserData();
        }
        private void InitObjects()
        {
            fsd = new FsData();
            spd = new SpData(this);
            user = new User(this);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == General.REQUEST_REGISTER)
                if (resultCode == Result.Ok)
                {
                    user.UserName = data.GetStringExtra(General.KEY_NAME);
                    user.Mail = data.GetStringExtra(General.KEY_MAIL);
                    user.Pwd = data.GetStringExtra(General.KEY_PWD);
                    ShowUserData();
                    user.Pwd = string.Empty;
                    spd.SaveUser(user);
                }
        }
        private void InitViews()
        {
            etName = FindViewById<EditText>(Resource.Id.etName);
            etMail = FindViewById<EditText>(Resource.Id.etMail);
            etPwd = FindViewById<EditText>(Resource.Id.etPwd);
            btnBackActivity = FindViewById<Button>(Resource.Id.btnBackActivity);
            btnBackActivity.SetOnClickListener(this);
        }

        private void ShowUserData()
        {
            etMail.Text = user.Mail;
            etName.Text = user.UserName;
            etPwd.Text = user.Pwd;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public void OnClick(View v)
        {
            if (v == btnBackActivity)
            {
                OpenGameActivity();
            }
        }
        private void OpenGameActivity()
        {
            Intent intent = new Intent(this, typeof(GameActivity));
            StartActivity(intent);
        }
    }
}