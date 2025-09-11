namespace OnlineGame01.Controllers;

// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// ユーザー登録時に受け取るデータのためのクラス
public class UserRegisterDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly GameDbContext _context;

    public AuthController(GameDbContext context)
    {
        _context = context;
    }

    // --- ユーザー登録のための窓口 (POST api/auth/register) ---
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto request)
    {
        // 1. 同じユーザー名が既に存在するかチェック
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        {
            return BadRequest("Username already exists."); // エラーを返す
        }

        // 2. 新しいユーザーを作成
        var newUser = new User
        {
            Username = request.Username,
            // 🚨注意：今はパスワードをそのまま保存しますが、将来的にはハッシュ化します
            PasswordHash = request.Password
        };

        // 3. データベースに保存
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully!");
    }

    // --- ログインのための窓口 (POST api/auth/login) ---
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserRegisterDto request)
    {
        // 1. ユーザー名でユーザーを探す
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null)
        {
            return BadRequest("Invalid username or password."); // ユーザーが見つからない
        }

        // 2. パスワードを比較
        // 🚨注意：今は単純な文字列比較ですが、将来的にはハッシュを比較します
        if (user.PasswordHash != request.Password)
        {
            return BadRequest("Invalid username or password."); // パスワードが違う
        }

        // 実際のアプリではここで「トークン」を発行しますが、今回は成功メッセージだけ返します
        return Ok("Login successful!");
    }
}