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
    public class BroadcastBattery : BroadcastReceiver
    {
        private int battery;
        private LinearLayout ll;
        public BroadcastBattery()
        {

        }
        public BroadcastBattery(LinearLayout ll)
        {
            this.ll = ll;
        }
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == Intent.ActionBatteryChanged)
            {
                battery = intent.GetIntExtra("level", 0);
            }
            if (battery < 25)
            {
                MakeDarkBackround();
            }
        }
        private void MakeDarkBackround()
        {
            ll.SetBackgroundColor(new Android.Graphics.Color(0, 0, 0));
        }
        public int GetBattery()
        {
            return battery;
        }
    }
}