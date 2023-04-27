namespace Host.Models;

public partial class Chat
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public byte[]? Picture { get; set; }

    public string? CreatorId { get; set; }

    public virtual User? Creator { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
