using System;

namespace OnlineGame01
{
    // APIとのデータ送受信に使うクラス定義
    // ファイル名はApiDataModels.csなどが分かりやすい

    // publicフィールドでないとJsonUtilityではシリアライズできないが、
    // Newtonsoft.Jsonを使う場合はプロパティ({ get; set; })でOK
    [Serializable]
    public class UserAuthData
    {
        public string username;
        public string password;
    }

    [Serializable]
    public class LoginResponseData
    {
        public string token;
    }

    [Serializable]
    public class ScorePostData
    {
        public int score;
    }

    // サーバーからのレスポンスはプロパティ名が大文字で始まらないため
    // Newtonsoft.Jsonの属性を使ってマッピングする
    [Serializable]
    public class ScoreResponseData
    {
        public string username;
        public int score;
    }
}