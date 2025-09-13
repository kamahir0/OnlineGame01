namespace OnlineGame01.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using OnlineGame01.Dtos; // DTOの名前空間を追加
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ZLinq;

[ApiController]
[Route("api/[controller]")]
public class ScoresController : ControllerBase
{
    private readonly GameDbContext _context;

    public ScoresController(GameDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize] // この属性により、認証済みユーザーのみがこのAPIを呼び出せる
    public async Task<IActionResult> AddScore([FromBody] ScorePostDto scoreData)
    {
        // より安全で簡潔なユーザーIDの取得方法
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Invalid user token.");
        }

        var newScore = new PlayerScore
        {
            Score = scoreData.Score,
            Timestamp = DateTime.UtcNow,
            UserId = userId
        };

        _context.Scores.Add(newScore);
        await _context.SaveChangesAsync();

        return Ok("Score added successfully!");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScoreResponseDto>>> GetScores()
    {
        var topScores = await _context.Scores
            .Include(s => s.User)
            .OrderByDescending(s => s.Score)
            .Take(10)
            .Select(s => new ScoreResponseDto
            {
                Username = s.User.Username,
                Score = s.Score
            })
            .ToListAsync();

        return Ok(topScores);
    }
}