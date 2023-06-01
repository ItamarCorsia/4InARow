using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInARow
{
    [Activity(Label = "FindGameActivity")]
    public class FindGameActivity : Activity, ListView.IOnItemClickListener, IOnCompleteListener
    {
        ListView lvGames;
        List<GameItem> lstGameItems;
        GameItemAdapter gameItemAdapter;
        FsData fbd;


        private void GetGameItems(QuerySnapshot querySnapshot)
        {
            foreach (DocumentSnapshot doc in querySnapshot.Documents)
            {
                GameItem gameItem = new GameItem();
                gameItem.Id = doc.Id;
                gameItem.HostName = doc.Get(General.KEY_HOST_NAME).ToString();
                lstGameItems.Add(gameItem);
            }
            gameItemAdapter = new GameItemAdapter(this, lstGameItems);
            lvGames.Adapter = gameItemAdapter;
        }
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            OpenGameActivity(position);
        }

        private void OpenGameActivity(int gameItemIndex)
        {
            Intent intent = new Intent(this, typeof(GameActivity));
            intent.PutExtra(General.KEY_HOST_NAME, lstGameItems[gameItemIndex].HostName);
            intent.PutExtra(General.KEY_NAME, Intent.GetStringExtra(General.KEY_NAME));
            intent.PutExtra(General.KEY_PLAYER, (int)General.PlayerType.GUEST);
            intent.PutExtra(General.KEY_ID, lstGameItems[gameItemIndex].Id);
            StartActivity(intent);
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_find_game);
            InitObjects();
            InitViews();
        }

        private void InitObjects()
        {
            fbd = new FsData();
            lstGameItems = new List<GameItem>();
            fbd.GetEqualToCollection(General.GAMES_COLLECTION, General.KEY_PARTICIPANTS, 1).AddOnCompleteListener(this);
        }

        private void InitViews()
        {
            lvGames = FindViewById<ListView>(Resource.Id.lvGames);
            lvGames.OnItemClickListener = this;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
                GetGameItems((QuerySnapshot)task.Result);
        }


    }
}