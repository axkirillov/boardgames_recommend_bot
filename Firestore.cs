using Google.Cloud.Firestore;
namespace firebase
{
    public static class Firestore
    {
        static FirestoreDb db;
        static CollectionReference collection;
        public static CollectionReference Connect()
        {
            db = FirestoreDb.Create("boardgame-recommend-bot");
            collection = db.Collection("games");
            return collection;
        }
    }
}