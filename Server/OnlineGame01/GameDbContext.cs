namespace OnlineGame01;

// GameDbContext.cs
using Microsoft.EntityFrameworkCore;

public class GameDbContext : DbContext
{
    // 1. PlayerScoreを保存するためのテーブルが欲しい、とEF Coreに伝えます
    public DbSet<PlayerScore> Scores { get; set; }

    // 2. データベースのファイル名を指定します
    private const string DatabaseName = "ranking.db";

    // 3. このDbContextがどうやってデータベースに接続するかを設定します
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // "ranking.db" という名前のSQLiteファイルを使うように設定
        optionsBuilder.UseSqlite($"Data Source={DatabaseName}");
    }
}