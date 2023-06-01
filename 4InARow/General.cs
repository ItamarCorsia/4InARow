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
    class General
    {
        public const string KEY_GAME_MOVE = "Move";
        public const string SP_FILE_NAME = "data.sp";
        public const string USERS_COLLECTION = "Users";
        public const string GAMES_COLLECTION = "Games";
        public const string FS_IMAGES = "Images/";
        public const string KEY_ID = "Id";
        public const string KEY_NAME = "Name";
       
        public const string KEY_MAIL = "mail";
        public const string KEY_PWD = "pwd";
        public const string KEY_PLAYER = "player";
        public const string KEY_COL = "col";
        public const string KEY_PARTICIPANTS = "Participants";

        public const string KEY_CAMERA_IMAGE = "data";
        public const int REQUEST_REGISTER = 1;
        public const int REQUEST_OPEN_CAMERA = 1;
        public const int REQUEST_ADD_DOCUMENT = 1;
        public const int REQUEST_CHECK_DOCUMENT = 2;
        public const int REQUEST_FIND_GAME = 5;

        public const int GAME_COLS = 7;

        public enum PlayerType
        {
            HOST,
            GUEST
        }
        public const string KEY_HOST_NAME = "Host Name";
        public const string KEY_GUEST_NAME = "Guest Name";
        public const string KEY_NEXT_PLAY = "Next Play";
        public enum Player
        {
            Host,
            Guest
        }        
    }
}