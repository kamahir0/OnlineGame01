namespace OnlineGame01.Controllers;

// Controllers/ScoresController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // これを追加

[ApiController]
[Route("api/[controller]")]
public class ScoresController : ControllerBase
{
    private readonly GameDbContext _context;

    // コンストラクタで、登録しておいたDbContextを受け取ります
    public ScoresController(GameDbContext context)
    {
        _context = context;
    }

    // --- スコアを登録するための窓口 (POST) ---
    [HttpPost]
    public async Task<IActionResult> AddScore([FromBody] PlayerScore score)
    {
        // InMemoryDatabaseの代わりにDbContextを使います
        score.Timestamp = DateTime.UtcNow;
        _context.Scores.Add(score);
        await _context.SaveChangesAsync(); // 変更をデータベースに保存（非同期）

        return Ok("Score added successfully!");
    }

    // --- ランキングを取得するための窓口 (GET) ---
    [HttpGet]
    public async Task<IActionResult> GetScores()
    {
        // InMemoryDatabaseの代わりにDbContextを使います
        var topScores = await _context.Scores
            .OrderByDescending(s => s.Score)
            .Take(10)
            .ToListAsync(); // データベースからリストを取得（非同期）

        return Ok(topScores);
    }
}