namespace SteveProjectServer.Controllers
{
    public class GameData
    {
        public string game { get; set; } = null!;
        public string developer { get; set; } = null!;
        public string publisher { get; set; } = null!;
        public long year { get; set; }
        public decimal? players { get; set; }
        public long appid { get; set; }
    }
}
