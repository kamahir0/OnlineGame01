namespace OnlineGame01
{
    // NOTE: 今回は JsonUtility ではなく Newtonsoft.Json を使っているため、
    // record でシリアライズ・デシリアライズできる
    
    public record UserAuthData
    {
        public string Username { get; init; }
        public string Password { get; init; }
    }

    public record LoginResponseData
    {
        public string Token { get; init; }
    }

    public record ScorePostData
    {
        public int Score { get; init; }
    }

    public record ScoreResponseData
    {
        public string Username { get; init; }
        public int Score { get; init; }
    }
}