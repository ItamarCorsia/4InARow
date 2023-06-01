using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Firebase.Firestore;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourInARow
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity, Android.Views.View.IOnClickListener, Android.Hardware.ISensorEventListener, IOnCompleteListener, Firebase.Firestore.IEventListener
    {
        User player;
        Game game;
        FsData fbd;
        private const int PIC_WIDTH = 100, PIC_HEIGHT = 100;
        LinearLayout llMainLayout7;
        LinearLayout llMainLayout1;
        LinearLayout llMainLayout2;
        LinearLayout llMainLayout3;
        LinearLayout llMainLayout4;
        LinearLayout llMainLayout5;
        LinearLayout llMainLayout6;
        ImageButton ivCurrentImage;
        Button btnAccount;
        Button btnReset;
        TextView tvDisplay;
        TextView tvGameNum;
        DocumentGame gameDoc ;
        ImageButton[,] imageButtons;
        LinearLayout[] ll;
        LinearLayout linearLayout;
        SpData sp;
        bool play;
        Toolbar tb;
        System.Random random;
        ImageView imageView;
        Button button;
        Animation edgetocenter, centertoedge;
        int counter;
        string current = "etz";
        bool flag = false;
        SensorManager sensorManager;
        Sensor lightSensor;
        BroadcastBattery broadcastBattery;
        int light;
        protected override void OnDestroy()
        {
            fbd.DeleteDocument(General.GAMES_COLLECTION, gameDoc.Id);
            base.OnDestroy();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_Game);
            InitPlayer();
            InitGame();
            InitFirebase();
            InitViews();
            InitObjects();

        }
        private void InitObjects()
        {
            broadcastBattery = new BroadcastBattery(linearLayout);
            sensorManager = (SensorManager)GetSystemService(SensorService);
            lightSensor = sensorManager.GetDefaultSensor(SensorType.Light);
            sensorManager.RegisterListener(this, lightSensor, Android.Hardware.SensorDelay.Ui);
        }
        void ISensorEventListener.OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            // We don't want to do anything here.
        }
        private void InitFirebase()
        {
            sp = new SpData(this);

            fbd = new FsData();
            if (!play)//host
            {
                CreateGameDoc();
            }
            else//guest
            {
                UpdateGameDoc();
            }
            fbd.AddDocumentSnapshotListener(General.GAMES_COLLECTION, gameDoc.Id, this);
        }

        private void UpdateGameDoc()
        {
            gameDoc = new DocumentGame
            {
                Id = Intent.GetStringExtra(General.KEY_ID),
                HostName = Intent.GetStringExtra(General.KEY_HOST_NAME),
                GuestName = player.UserName,
                NextPlay = General.PlayerType.GUEST,
                Participants = 2
            };
            HashMap hm = gameDoc.GetHashMap();
            fbd.SaveDocument(General.GAMES_COLLECTION, gameDoc.Id, hm, out string id).AddOnCompleteListener(this);
        }
        private void CreateGameDoc()
        {
            gameDoc = new DocumentGame
            {
                HostName = player.UserName,
                NextPlay = General.PlayerType.GUEST,
                Participants = 1
            };
            HashMap hm = gameDoc.GetHashMap();
            fbd.SaveDocument(General.GAMES_COLLECTION, string.Empty, hm, out string id).AddOnCompleteListener(this);
            gameDoc.Id = id;
        }
        private void InitPlayer()
        {
            player = new User
            {
                PlayerType = (General.PlayerType)Intent.GetIntExtra(General.KEY_PLAYER, (int)General.PlayerType.HOST),
                UserName = Intent.GetStringExtra(General.KEY_NAME)
            };
            play = player.PlayerType == General.PlayerType.GUEST;
        }
        private void InitGame()
        {
            game = new Game();
        }
        private void InitViews()
        {
            tb = FindViewById<Toolbar>(Resource.Id.tb);
            SetActionBar(tb);
            tvDisplay = FindViewById<TextView>(Resource.Id.tvDisplay);
            linearLayout = FindViewById<LinearLayout>(Resource.Id.ll);
            ll = new LinearLayout[7];
            ll[0] = FindViewById<LinearLayout>(Resource.Id.llMainLayout1);
            ll[1] = FindViewById<LinearLayout>(Resource.Id.llMainLayout2);
            ll[2] = FindViewById<LinearLayout>(Resource.Id.llMainLayout3);
            ll[3] = FindViewById<LinearLayout>(Resource.Id.llMainLayout4);
            ll[4] = FindViewById<LinearLayout>(Resource.Id.llMainLayout5);
            ll[5] = FindViewById<LinearLayout>(Resource.Id.llMainLayout6);
            ll[6] = FindViewById<LinearLayout>(Resource.Id.llMainLayout7);
            btnAccount = FindViewById<Button>(Resource.Id.btnAccount);
            btnAccount.SetOnClickListener(this);            
            btnReset = FindViewById<Button>(Resource.Id.btnReset);
            btnReset.SetOnClickListener(this);
            random = new System.Random();
            imageView = (ImageView)FindViewById(Resource.Id.imageView1);
            button = (Button)FindViewById(Resource.Id.button);
            edgetocenter = AnimationUtils.LoadAnimation(this, Resource.Animation.edgetocenter);
            centertoedge = AnimationUtils.LoadAnimation(this, Resource.Animation.centertoedge);
            button.Click += Button_Click;
            edgetocenter.RepeatCount = 10;
            centertoedge.RepeatCount = 10;
            edgetocenter.AnimationEnd += Edgetocenter_AnimationEnd;
            centertoedge.AnimationEnd += Centertoedge_AnimationEnd;
            imageButtons = new ImageButton[7, 7];
            LoadImages();

        }
        private void Centertoedge_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            if (counter > 0)
            {
                imageView.StartAnimation(edgetocenter);
                flag = !flag;
                if (flag)
                {
                    counter--;
                }
            }
        }

        private void Edgetocenter_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {

            if (counter > 0)
            {
                if (current == "Red_disc")
                {
                    imageView.SetImageResource(Resource.Drawable.Yellow_disc);
                    current = "Yellow_disc";
                }
                else
                {
                    imageView.SetImageResource(Resource.Drawable.Red_disc);
                    current = "Red_disc";
                }
                imageView.StartAnimation(centertoedge);
                counter--;
            }
        }
        private void Button_Click(object sender, System.EventArgs e)
        {

            imageView.StartAnimation(edgetocenter);
            counter = random.Next(5, 9);
        }
        private string GetMessage(bool play)
        {
            return play ? "▶Play please" : "⏰Please wait";
        }
        
        public void OnClick(View v)
        {
            if (v == btnAccount)
            {
                OpenAccountActivity();
            }
            else if (v == btnReset)
            {
                ResetGame();
            }
            else
            {                
                if (play)
                {
                    ImageButton ib = (ImageButton)v;
                    string ibPos = ib.Tag.ToString();
                    Play(ibPos, true);
                }
            }           
        }
        private void Play(string newIbPos, bool myMove)
        {
            if (myMove)
            {
                play = false;
                gameDoc.GameMove = newIbPos;
                gameDoc.NextPlay = player.PlayerType == General.PlayerType.GUEST ?
                                        General.PlayerType.HOST : General.PlayerType.GUEST;
                HashMap hm = gameDoc.GetHashMap();
                fbd.SaveDocument(General.GAMES_COLLECTION, gameDoc.Id, hm, out string id).AddOnCompleteListener(this);
            }

            string pos = game.Move(newIbPos);
            int col = int.Parse(pos.Substring(1, 1));
            int row = int.Parse(pos.Substring(0, 1));

            if (game.Status == Game.GameStatus.PLAY)
                tvDisplay.Text = GetMessage(play);
            else
                tvDisplay.Text = game.Display;

            if (game.win ==false)
            {
                if (gameDoc.NextPlay == General.PlayerType.GUEST)//host
                {
                    DrawCircle(row, col, 0);
                }
                if (gameDoc.NextPlay == General.PlayerType.HOST)//host
                {
                    DrawCircle(row, col, 1);
                }
            }
            
        }

        private void LoadImages()
        {
            int num = 6;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    ivCurrentImage = new ImageButton(this);
                    imageButtons[j, i] = ivCurrentImage;
                    ivCurrentImage.Tag = j + (string.Empty + i);
                    ivCurrentImage.SetBackgroundResource(Resource.Drawable.grey_disc);
                    LinearLayout.LayoutParams lpImageView = new LinearLayout.LayoutParams(PIC_WIDTH, PIC_HEIGHT);
                    ivCurrentImage.LayoutParameters = lpImageView;
                    int imageKey = Resources.GetIdentifier("a", "drawable", this.PackageName);
                    ivCurrentImage.SetImageResource(imageKey);
                    ivCurrentImage.SetOnClickListener(this);
                    ll[num].AddView(ivCurrentImage);
                }
                num--;
            }
        }
        
        private void DrawCircle(int row, int col, long status)
        {
            if (status == 0)//host
            {
                imageButtons[row, col].SetBackgroundResource(Resource.Drawable.Red_disc);
            }
            else if (status == 1)//guest
            {
                imageButtons[row, col].SetBackgroundResource(Resource.Drawable.Yellow_disc);
            }
        }

        private void ResetGame()
        {
            game.ResetGame();
            play = player.PlayerType == General.PlayerType.GUEST;
            tvDisplay.Text = GetMessage(play);
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    imageButtons[i, j].SetBackgroundResource(Resource.Drawable.blue_disc);
                }
            }
        }
        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_account)
            {
                OpenAccountActivity();
                return true;
            }

            else if (item.ItemId == Resource.Id.action_reset)
            {
                ResetGame();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
        private void OpenAccountActivity()
        {
            Intent intent = new Intent(this, typeof(ProfileActivity));
            StartActivity(intent);
        }
        public void OnComplete(Task task)
        {
            string msg = task.IsSuccessful ? GetMessage(play) : task.Exception.Message;
            Toast.MakeText(this, msg, ToastLength.Long).Show();
        }
        void ISensorEventListener.OnSensorChanged(SensorEvent e)
        {
            Sensor mySensor = e.Sensor;
            if (mySensor.Type.Equals(SensorType.Light))
                light = (int)e.Values[0];
            

        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(broadcastBattery, new IntentFilter(Intent.ActionBatteryChanged));
        }

        protected override void OnPause()
        {
            UnregisterReceiver(broadcastBattery);
            base.OnPause();
        }
        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            DocumentSnapshot ds = (DocumentSnapshot)value;
            if (ds.Exists())
            {
                gameDoc.NextPlay = (General.PlayerType)(int)ds.Get(General.KEY_NEXT_PLAY);
                if (gameDoc.NextPlay == player.PlayerType)
                {
                    Java.Lang.Object o = ds.Get(General.KEY_GAME_MOVE);
                    if (o != null)
                    {
                        gameDoc.GameMove = o.ToString();
                        play = true;
                        Play(gameDoc.GameMove, false);
                    }
                }
            }           
        }
    }
}