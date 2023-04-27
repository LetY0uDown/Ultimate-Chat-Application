namespace Host.Models;

public partial class ChatMember
{
    public string ChatId { get; set; } = null!;

    public string MemberId { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;

    public virtual User Member { get; set; } = null!;
}
