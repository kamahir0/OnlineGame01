using System;

namespace OnlineGame01
{
    // APIとのデータ送受信に使うクラス定義

    // publicフィールドでないとJsonUtilityではシリアライズできないが、
    // Newtonsoft.Jsonを使う場合はプロパティ({ get; set; })でOK
    [Serializable]
    public class UserAuthData
    {
        public string Username;
        public string Password;
    }

    [Serializable]
    public class LoginResponseData
    {
        public string Token;
    }

    [Serializable]
    public class ScorePostData
    {
        public int Score;
    }

    // サーバーからのレスポンスはプロパティ名が大文字で始まらないため
    // Newtonsoft.Jsonの属性を使ってマッピングする
    [Serializable]
    public class ScoreResponseData
    {
        public string Username;
        public int Score;
    }
}