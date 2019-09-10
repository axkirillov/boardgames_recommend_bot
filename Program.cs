using System;
using System.IO;
using Newtonsoft.Json;

namespace boardgame_bot
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            string token = ReadSecret();
            Bot.Run(ReadSecret());
        }
        private static string ReadSecret()
        {
            if (!File.Exists(@"secret.json"))
            {
                Storage.DownloadObject("boardgame_recommend_secrets", "secret.json");
            }
            string secret = System.IO.File.ReadAllText(@"secret.json");
            Secret deserializedSecret = JsonConvert.DeserializeObject<Secret>(secret);
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"){
                return deserializedSecret.development_bot_token;
            } else {
                return deserializedSecret.production_bot_token;
            }
        }
    }
}
