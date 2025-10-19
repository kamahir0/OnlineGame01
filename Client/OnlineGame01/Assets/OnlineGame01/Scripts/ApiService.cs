using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace OnlineGame01
{
    public class ApiService
    {
        private const string BaseUrlFormat = "http://{0}:8080/api";
        private static string BaseUrl => string.Format(BaseUrlFormat, PublicIp.Current);

        // NOTE: 本来なら Repository で保持する
        private static string _jwtToken;

        /// <summary>
        /// ユーザーを登録します
        /// </summary>
        public async UniTask<string> RegisterAsync(string username, string password)
        {
            var userAuthData = new UserAuthData { Username = username, Password = password };
            var response = await SendRequestAsync<UserAuthData, string>(
                $"{BaseUrl}/auth/register", 
                "POST", 
                userAuthData
            );
            return response;
        }

        /// <summary>
        /// ログインします
        /// </summary>
        public async UniTask<string> LoginAsync(string username, string password)
        {
            var userAuthData = new UserAuthData { Username = username, Password = password };
            var response = await SendRequestAsync<UserAuthData, LoginResponseData>(
                $"{BaseUrl}/auth/login", 
                "POST", 
                userAuthData
            );

            // レスポンスからトークンを抽出し、保存する
            _jwtToken = response.Token;

            return "Login successful!";
        }

        /// <summary>
        /// スコアを投稿します
        /// </summary>
        public async UniTask<string> PostScoreAsync(int score)
        {
            if (string.IsNullOrEmpty(_jwtToken))
            {
                const string Message = "You must be logged in to post a score.";
                Debug.LogError(Message);
                throw new Exception(Message);
            }

            var scoreData = new ScorePostData { Score = score };
            var response = await SendRequestAsync<ScorePostData, string>(
                $"{BaseUrl}/scores", 
                "POST", 
                scoreData, 
                _jwtToken
            );
            return response;
        }

        /// <summary>
        /// スコア一覧を取得します
        /// </summary>
        public async UniTask<List<ScoreResponseData>> GetScoresAsync()
        {
            var response = await SendRequestAsync<List<ScoreResponseData>>(
                $"{BaseUrl}/scores", 
                "GET"
            );
            return response;
        }

        /// <summary>
        /// 共通のHTTPリクエスト処理（ボディあり）
        /// </summary>
        private static async UniTask<TResponse> SendRequestAsync<TRequest, TResponse>(
            string url, 
            string method, 
            TRequest body = default, 
            string token = null
        )
        {
            string responseJson = await SendRequestInternalAsync(url, method, body, token);
            
            // stringの場合はそのまま返す（デシリアライズ不要）
            if (typeof(TResponse) == typeof(string))
            {
                return (TResponse)(object)responseJson;
            }
            
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }

        /// <summary>
        /// 共通のHTTPリクエスト処理（ボディなし）
        /// </summary>
        private static async UniTask<TResponse> SendRequestAsync<TResponse>(
            string url, 
            string method, 
            string token = null
        )
        {
            string responseJson = await SendRequestInternalAsync<object>(url, method, null, token);
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }

        /// <summary> HTTPリクエストの実行と共通ログ処理 </summary>
        private static async UniTask<string> SendRequestInternalAsync<TRequest>(
            string url, 
            string method, 
            TRequest body = default, 
            string token = null
        )
        {
            using var request = new UnityWebRequest(url, method);

            // ボディがある場合（POST, PUTなど）
            if (body != null && !EqualityComparer<TRequest>.Default.Equals(body, default(TRequest)))
            {
                string jsonBody = JsonConvert.SerializeObject(body);
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.SetRequestHeader("Content-Type", "application/json");
            }

            request.downloadHandler = new DownloadHandlerBuffer();

            // トークンが指定されていれば、認証ヘッダーを追加
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            // 開発時用の証明書バイパス
            request.certificateHandler = new BypassCertificate();

            // リクエスト送信
            await request.SendWebRequest();

            // 共通のログ出力とエラーハンドリング
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"[API] {method} {url} - Success: {request.downloadHandler.text}");
                return request.downloadHandler.text;
            }
            else
            {
                var message = $"[API] {method} {url} - Error: {request.error} | {request.downloadHandler.text}";
                Debug.LogError(message);
                throw new Exception(message);
            }
        }
    }

    // 開発時用の証明書バイパスハンドラ
    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) => true;
    }
}