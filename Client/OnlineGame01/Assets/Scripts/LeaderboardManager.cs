// LeaderboardManager.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.Collections.Generic;
using TMPro; // Listのために必要

// Unity側でも、サーバーと通信するためのデータの形を定義
// サーバーからの返答(GET)を受けるためのクラス
[System.Serializable]
public class ScoreResponse
{
    public int id;
    public string playerName;
    public int score;
    public string timestamp;
}

// サーバーへ送る(POST)ためのクラス
[System.Serializable]
public class ScorePostData
{
    public string playerName;
    public int score;
}

public class LeaderboardManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    public Button submitButton;
    public Button fetchButton;
    public TMP_Text leaderboardText;
    
    // サーバーのURL。Visual Studioでデバッグ実行した際のアドレスに合わせる
    // 先日のトラブルシューティングで確認したポート番号を指定してください
    private const string ServerUrl = "https://localhost:7037/api/scores"; 

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmitScore);
        fetchButton.onClick.AddListener(OnFetchScores);
        OnFetchScores(); // 起動時にもランキングを読み込む
    }

    public async void OnSubmitScore() => await SubmitScoreAsync();
    public async void OnFetchScores() => await FetchScoresAsync();

    private async Awaitable SubmitScoreAsync()
    {
        var scoreData = new ScorePostData
        {
            playerName = nameInput.text,
            score = Random.Range(100, 1000) // テスト用にランダムなスコア
        };

        string jsonData = JsonUtility.ToJson(scoreData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(ServerUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Score submitted!");
                await FetchScoresAsync(); // 送信成功したらランキングを自動更新
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
    
    private async Awaitable FetchScoresAsync()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(ServerUrl))
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                
                // UnityのJsonUtilityはJSON配列を直接パースできないため、ラッパーを使う
                string newJson = "{ \"scores\": " + jsonResponse + "}";
                ScoreList scoreList = JsonUtility.FromJson<ScoreList>(newJson);
                
                // UIに表示
                var displayText = new StringBuilder();
                int rank = 1;
                foreach (var score in scoreList.scores)
                {
                    displayText.AppendLine($"{rank++}. {score.playerName} - {score.score}");
                }
                leaderboardText.text = displayText.ToString();
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}

// JsonUtilityでJSON配列をパースするためのヘルパークラス
[System.Serializable]
public class ScoreList
{
    public List<ScoreResponse> scores;
}