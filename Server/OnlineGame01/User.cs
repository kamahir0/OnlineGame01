namespace OnlineGame01;

// User.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string PasswordHash { get; set; }

    // プロパティを初期化してNullを回避
    public List<PlayerScore> Scores { get; set; } = new();
}