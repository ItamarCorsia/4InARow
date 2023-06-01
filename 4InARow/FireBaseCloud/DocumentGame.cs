using Android.App;
using Android.Content;
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
    class DocumentGame
    {
        public string Id { get; set; }
        public string HostName { get; set; }
        public string GuestName { get; set; }
        public General.PlayerType NextPlay { get; set; }
        public string GameMove { get; set; }
        public int Participants { get; set; }

        public DocumentGame()
        {

        }

        public HashMap GetHashMap()
        {
            HashMap hm = new HashMap();
            hm.Put(General.KEY_HOST_NAME, this.HostName);
            hm.Put(General.KEY_GUEST_NAME, this.GuestName);
            hm.Put(General.KEY_PARTICIPANTS, this.Participants);
            hm.Put(General.KEY_NEXT_PLAY, (int)this.NextPlay);
            hm.Put(General.KEY_GAME_MOVE, this.GameMove);

            return hm;
        }

    }

}