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
    class GameItemAdapter : BaseAdapter<GameItem>
    {
        private List<GameItem> lstGameItems;
        private Context context;

        public GameItemAdapter(Context context, List<GameItem> lstGameItems)
        {
            this.context = context;
            this.lstGameItems = lstGameItems;
        }
        public override GameItem this[int position] => lstGameItems[position];

        public override int Count => lstGameItems.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((FindGameActivity)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.game_item, parent, false);
            TextView tvHostName = view.FindViewById<TextView>(Resource.Id.tvHostName);
            GameItem gameItem = lstGameItems[position];


            if (gameItem != null)
            {
                tvHostName.Text = gameItem.HostName;
            }

            return view;
        }
    }
}