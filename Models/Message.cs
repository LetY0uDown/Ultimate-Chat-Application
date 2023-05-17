using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("Messages")]
public partial class Message : Entity
{
    public string? SenderId { get; set; }

    public string ChatId { get; set; } = null!;

    public string Text { get; set; } = null!;

    public DateTime SendedDate { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User? Sender { get; set; }
}
