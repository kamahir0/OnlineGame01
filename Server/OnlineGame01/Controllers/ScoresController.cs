namespace OnlineGame01.Controllers;

// Controllers/ScoresController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")] // このAPIのURLは /api/scores になります
public class ScoresController : ControllerBase
{
    // --- スコアを登録するための窓口 (POST) ---
    [HttpPost]
    public IActionResult AddScore([FromBody] PlayerScore score)
    {
        InMemoryDatabase.AddScore(score);
        return Ok("Score added successfully!"); // 「成功しましたよ」と返事をする
    }

    // --- ランキングを取得するための窓口 (GET) ---
    [HttpGet]
    public IActionResult GetScores()
    {
        var topScores = InMemoryDatabase.Scores
            .OrderByDescending(s => s.Score) // スコアが高い順に並び替え
            .Take(10) // 上位10件だけ取得
            .ToList();

        return Ok(topScores); // 取得したランキングデータを返す
    }
}