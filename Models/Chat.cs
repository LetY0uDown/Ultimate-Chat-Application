using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("Chats")]
public partial class Chat : Entity
{
    public string Title { get; set; } = null!;

    public byte[]? Picture { get; set; }

    public string? CreatorId { get; set; }

    public virtual User? Creator { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
