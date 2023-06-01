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
    class User
    {
        public General.PlayerType PlayerType { get; set; }
        public string UserName { get; set; }
        public string Mail { get; set; }
        public string Pwd { get; set; }
        public bool Exist { get; }

        public User(Context ctx)
        {
            SpData spData = new SpData(ctx);
            Exist = spData.IsDataExist;
            if (this.Exist)
            {
                HashMap hm = spData.GetUserData();
                this.UserName = hm.Get(General.KEY_NAME).ToString();
                this.Mail = hm.Get(General.KEY_MAIL).ToString();
                this.Pwd = hm.Get(General.KEY_PWD).ToString(); 
            }
        }
        public User()
        {

        }
        public User(string name, string mail, string pwd, bool exist)
        {
           this.UserName = name;
            this.Mail = mail;
            this.Pwd = pwd;
            this.Exist = exist;
        }
    }
}