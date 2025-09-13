namespace OnlineGame01;

// GameDbContext.cs
using Microsoft.EntityFrameworkCore;

public class GameDbContext : DbContext
{
    // コンストラクタを追加して、Program.csからの設定を受け取る
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

    public DbSet<PlayerScore> Scores { get; set; }
    public DbSet<User> Users { get; set; }
}