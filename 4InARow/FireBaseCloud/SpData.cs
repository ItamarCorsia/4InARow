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
    class SpData
    {
        private readonly ISharedPreferences sp;
        private ISharedPreferencesEditor editor;

        public bool IsDataExist { get; }

        public SpData(Context ctx)
        {
            sp = ctx.GetSharedPreferences(General.SP_FILE_NAME, FileCreationMode.Private);
            string tmp = sp.GetString(General.KEY_NAME, string.Empty);
            IsDataExist = tmp != string.Empty;
            editor = sp.Edit();
        }

        public HashMap GetUserData()
        {
            HashMap hm = new HashMap();
            hm.Put(General.KEY_NAME, sp.GetString(General.KEY_NAME, string.Empty));
            hm.Put(General.KEY_MAIL, sp.GetString(General.KEY_MAIL, string.Empty));
            hm.Put(General.KEY_PWD, sp.GetString(General.KEY_PWD, string.Empty));
            return hm;
        }

        public bool SetStringValue(string key, string value)
        {
            editor.PutString(key, value);
            return editor.Commit();
        }

        public string GetStringValue(string key)
        {
            return sp.GetString(key, string.Empty);
        }


        public bool SaveUser(User user)
        {
            ISharedPreferencesEditor editor = sp.Edit();
            editor.PutString(General.KEY_NAME, user.UserName);
            editor.PutString(General.KEY_MAIL, user.Mail);
            editor.PutString(General.KEY_PWD, user.Pwd);
            return editor.Commit();
        }
    }   
}