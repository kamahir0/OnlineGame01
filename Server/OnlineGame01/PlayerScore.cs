namespace OnlineGame01;

// PlayerScore.cs
using System;

public class PlayerScore
{
    public int Id { get; set; }
    public int Score { get; set; }
    public DateTime Timestamp { get; set; }

    // 外部キーとナビゲーションプロパティ
    public int UserId { get; set; }
    public User User { get; set; }
}