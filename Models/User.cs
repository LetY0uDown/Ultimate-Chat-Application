using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models;

[Table("Users")]
public partial class User : Entity
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? Picture { get; set; }

    [JsonIgnore]
    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    [JsonIgnore]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
