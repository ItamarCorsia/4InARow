using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace FourInARow
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnClickListener, IOnCompleteListener
    {
        EditText etName, etMail, etPwd;
        Button btnLogin,btnSignUp , btnFindGame;
        SpData spd;
        FsData fsd;

        User user;
        string action;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
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
                    spd.SaveUser(user);
                }
        }
        private void InitViews()
        {
            etName = FindViewById<EditText>(Resource.Id.etName);
            etMail = FindViewById<EditText>(Resource.Id.etMail);
            etPwd = FindViewById<EditText>(Resource.Id.etPwd);
            btnLogin = FindViewById<Button>(Resource.Id.btnlogin);
            btnLogin.SetOnClickListener(this);
            btnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            btnSignUp.SetOnClickListener(this);
            btnFindGame = FindViewById<Button>(Resource.Id.btnFindGame);
            btnFindGame.SetOnClickListener(this);

        }
        private void OpenSignUpActivity()
        {
            Intent intent = new Intent(this, typeof(SignUpActivity));
            StartActivityForResult(intent, General.REQUEST_REGISTER);
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
            if (v == btnLogin)
            {
                LoginUser();
            }
            else if (v== btnFindGame)
            {
                OpenFindGameActivity();
            }
            else if (v == btnSignUp)
                OpenSignUpActivity();
           
        }
        private void OpenFindGameActivity()
        {
            Intent intent = new Intent(this, typeof(FindGameActivity));
            intent.PutExtra(General.KEY_NAME, etName.Text);
            intent.PutExtra(General.KEY_PLAYER, (int)General.PlayerType.GUEST);
            StartActivity(intent);
        }
        private void LoginUser()
        {
            string pwd = etPwd.Text;
            action = "Login";
            if (pwd != string.Empty)
            {
                spd.SaveUser(user);
                fsd.SignIn(user.Mail, pwd).AddOnCompleteListener(this);
            }
            else
                Toast.MakeText(this, "Enter password", ToastLength.Long).Show();
        }
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
                OpenGameActivity();
        }
        private void OpenGameActivity()
        {
            Intent intent = new Intent(this, typeof(GameActivity));
            intent.PutExtra(General.KEY_NAME, etName.Text);
            intent.PutExtra(General.KEY_PLAYER, (int)General.PlayerType.HOST);
            StartActivity(intent);
        }
    }
}
