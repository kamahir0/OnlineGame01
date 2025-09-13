namespace OnlineGame01.Dtos;

// ユーザー登録・ログイン時にクライアントから受け取るデータ
public class UserAuthDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

// スコア登録時にクライアントから受け取るデータ
public class ScorePostDto
{
    public int Score { get; set; }
}

// ランキングをクライアントに返すためのデータ
public class ScoreResponseDto
{
    public required string Username { get; set; }
    public int Score { get; set; }
}

// ログイン成功時にクライアントに返すデータ
public class LoginResponseDto
{
    public required string Token { get; set; }
}