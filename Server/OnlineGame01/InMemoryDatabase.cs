namespace OnlineGame01;

// InMemoryDatabase.cs
// static にすることで、どこからでも同じリストにアクセスできます
public static class InMemoryDatabase
{
    // スコアを保存しておくためのリスト
    public static readonly List<PlayerScore> Scores = new List<PlayerScore>();
    private static int _nextId = 1;

    // 新しいスコアを追加する処理
    public static void AddScore(PlayerScore score)
    {
        score.Id = _nextId++; // 新しいIDを割り振る
        score.Timestamp = DateTime.UtcNow; // 現在時刻を記録
        Scores.Add(score);
    }
}