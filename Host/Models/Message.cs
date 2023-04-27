using System;
using System.Collections.Generic;

namespace Host.Models;

public partial class Message
{
    public string Id { get; set; } = null!;

    public string? SenderId { get; set; }

    public string ChatId { get; set; } = null!;

    public string Text { get; set; } = null!;

    public DateTime SendedDate { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User? Sender { get; set; }
}
