namespace OnlineGame01;

// User.cs
using System.ComponentModel.DataAnnotations; // [Key] や [Required] のために必要

public class User
{
    [Key] // これが主キー（一意の識別子）であることを示す
    public int Id { get; set; }

    [Required] // この項目が必須であることを示す
    public required string Username { get; set; }

    [Required] // この項目が必須であることを示す
    public required string PasswordHash { get; set; } // パスワードそのものではなく、ハッシュ化したものを保存
}