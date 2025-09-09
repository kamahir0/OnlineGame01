namespace OnlineGame01;

// PlayerScore.cs
public class PlayerScore
{
    public int Id { get; set; } // データの識別番号
    public required string PlayerName { get; set; } // プレイヤー名
    public int Score { get; set; } // スコア
    public DateTime Timestamp { get; set; } // 記録された日時
}