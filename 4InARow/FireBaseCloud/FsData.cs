using Android.App;
using Android.Gms.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Java.Util;
using System;

namespace FourInARow
{
    class FsData
    {
        private readonly FirebaseFirestore firestore;
        private readonly FirebaseAuth auth;
        private readonly FirebaseApp app;

        public FsData()
        {
            app = FirebaseApp.InitializeApp(Application.Context);
            if (app is null)
            {
                FirebaseOptions options = GetMyOptions();
                app = FirebaseApp.InitializeApp(Application.Context, options);
            }
            firestore = FirebaseFirestore.GetInstance(app);
            auth = FirebaseAuth.Instance;
        }

        private FirebaseOptions GetMyOptions()
        {
            return new FirebaseOptions.Builder()
                    .SetProjectId("fourinarow-66fe6")
                    .SetApplicationId("fourinarow-66fe6")
                    .SetApiKey("AIzaSyBvhvuhGMc_dtx0tFM7cnw2zrg3f0dngA8")
                    .SetStorageBucket("fourinarow-66fe6.appspot.com")
                    .Build();
        }

        public Android.Gms.Tasks.Task CreateUser(string email, string password)
        {
            return auth.CreateUserWithEmailAndPassword(email, password);
        }

        public Android.Gms.Tasks.Task SignIn(string email, string password)
        {
            return auth.SignInWithEmailAndPassword(email, password);
        }

        public Android.Gms.Tasks.Task ResetPwd(string email)
        {
            return auth.SendPasswordResetEmail(email);
        }

        public Task SaveDocument(string cName, string id, HashMap hmFields, out string newId)
        {
            DocumentReference dr;
            if (id == string.Empty)
                dr = firestore.Collection(cName).Document();
            else
                dr = firestore.Collection(cName).Document(id);
            newId = dr.Id;
            return dr.Set(hmFields);
        }
            //this function also updates the document if it is already exist
            public Task AddDocumentToCollection(string cName, string id, HashMap hmFields, out string newId)
        {
            DocumentReference dr;
            if (id == string.Empty)
                dr = firestore.Collection(cName).Document();
            else
                dr = firestore.Collection(cName).Document(id);
            newId = dr.Id;
            return dr.Set(hmFields);
        }

        public string GetNewDocumentId(string cName)
        {
            DocumentReference dr = firestore.Collection(cName).Document();
            return dr.Id;
        }

        public Task DeleteDocument(string cName, string dName)
        {
            return firestore.Collection(cName).Document(dName).Delete();
        }

        public Task GetDocument(string cName, string dName)
        {
            return firestore.Collection(cName).Document(dName).Get();
        }

        public Task GetCollection(string cName)
        {
            return firestore.Collection(cName).Get();
        }
        public void AddSnapshotListener(GameActivity gameActivity)
        {
            firestore.Collection(General.GAMES_COLLECTION).AddSnapshotListener(gameActivity);
        }
        public Task GetEqualToCollection(string collection, string field, int value)
        {
            return firestore.Collection(collection).WhereEqualTo(field, value).Get();
        }
        public void AddDocumentSnapshotListener(string collection, string docId, Firebase.Firestore.IEventListener listener)
        {
            firestore.Collection(collection).Document(docId).AddSnapshotListener(listener);
        }
    }
}