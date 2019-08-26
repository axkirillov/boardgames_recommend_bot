using Google.Cloud.Firestore;

namespace boardgame_bot
{
    [FirestoreData]
    public class Game
    {
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Site_Id { get; set; }
        [FirestoreProperty]
        public int MinPlayers { get; set; }
        [FirestoreProperty]
        public int MaxPlayers { get; set; }
        [FirestoreProperty]
        public int MinPlayTime { get; set; }
        [FirestoreProperty]
        public int MaxPlayTime { get; set; }
        [FirestoreProperty]
        public int MinAge { get; set; }
        [FirestoreProperty]
        public string Image { get; set; }
        [FirestoreProperty]
        public string Rating { get; set; }
    }
}