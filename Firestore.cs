using Google.Cloud.Firestore;
namespace boardgame_bot
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