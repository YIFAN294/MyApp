namespace MyApp.Models;

public class User   
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int Age { get; set; }  // ← 新增字段

    public string Sex { get; set; } = string.Empty;  // ← 新增

}
