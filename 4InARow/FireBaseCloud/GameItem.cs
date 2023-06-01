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
    class GameItem
    {
        public string Id { get; set; }
        public string HostName { get; set; }
        public int GameSize { get; set; }

        public GameItem()
        {

        }
    }
}