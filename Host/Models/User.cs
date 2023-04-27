using Newtonsoft.Json;

namespace Host.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? Picture { get; set; }

    [JsonIgnore]
    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    [JsonIgnore]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
