using Google.Cloud.Firestore;
namespace boardgame_bot
{
    public static class Firestore
    {
        static FirestoreDb db;
        public static CollectionReference Connect()
        {
            db = FirestoreDb.Create("boardgame-recommend-bot");
            var collection = db.Collection("games");
            return collection;
        }
    }
}