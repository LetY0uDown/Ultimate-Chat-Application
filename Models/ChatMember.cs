using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("ChatMembers")]
public partial class ChatMember : Entity
{
    public string ChatId { get; set; } = null!;

    public string MemberId { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;

    public virtual User Member { get; set; } = null!;

    public DateTime JoinDate { get; set; }
}
