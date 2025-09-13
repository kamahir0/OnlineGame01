using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace OnlineGame01
{
    public class ApiService
    {
        // どこからでもアクセスできる静的なインスタンス（シングルトン）
        public static readonly ApiService Instance = new();

        private const string BaseUrl = "https://localhost:7037/api"; // 自身の環境に合わせて変更
        private string _jwtToken;

        // コンストラクタをprivateにして、外部からのインスタンス化を防ぐ
        private ApiService() { }

        // --- 各APIを呼び出す非同期メソッド ---

        /// <summary>
        /// ユーザーを登録します
        /// </summary>
        public UniTask<string> RegisterAsync(string username, string password)
        {
            var userAuthData = new UserAuthData { Username = username, Password = password };
            return PostJsonAsync($"{BaseUrl}/auth/register", userAuthData);
        }

        /// <summary>
        /// ログインします
        /// </summary>
        public async UniTask<string> LoginAsync(string username, string password)
        {
            var userAuthData = new UserAuthData { Username = username, Password = password };
            string responseJson = await PostJsonAsync($"{BaseUrl}/auth/login", userAuthData);

            // レスポンスからトークンを抽出し、保存する
            var response = JsonConvert.DeserializeObject<LoginResponseData>(responseJson);
            _jwtToken = response.Token;

            return "Login successful!";
        }

        /// <summary>
        /// スコアを投稿します
        /// </summary>
        public UniTask<string> PostScoreAsync(int score)
        {
            if (string.IsNullOrEmpty(_jwtToken))
            {
                throw new Exception("You must be logged in to post a score.");
            }

            var scoreData = new ScorePostData { Score = score };
            // 認証ヘッダー付きでPOSTリクエストを送信
            return PostJsonAsync($"{BaseUrl}/scores", scoreData, _jwtToken);
        }

        /// <summary>
        /// スコア一覧を取得します
        /// </summary>
        public async UniTask<List<ScoreResponseData>> GetScoresAsync()
        {
            using var request = UnityWebRequest.Get($"{BaseUrl}/scores");
            
            // 開発時用の証明書バイパス
            request.certificateHandler = new BypassCertificate();

            // SendWebRequestをawaitする
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                return JsonConvert.DeserializeObject<List<ScoreResponseData>>(request.downloadHandler.text);
            }
            else
            {
                // エラー時は例外をスローする
                throw new Exception(request.error + ": " + request.downloadHandler.text);
            }
        }

        // --- 共通のPOST処理メソッド ---
        private static async UniTask<string> PostJsonAsync(string url, object body, string token = null)
        {
            using var request = new UnityWebRequest(url, "POST");
            
            string jsonBody = JsonConvert.SerializeObject(body);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // トークンが指定されていれば、認証ヘッダーを追加
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            // 開発時用の証明書バイパス
            request.certificateHandler = new BypassCertificate();

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
            else
            {
                throw new Exception(request.error + ": " + request.downloadHandler.text);
            }
        }
    }

    // 開発時用の証明書バイパスハンドラ
    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) => true;
    }
}