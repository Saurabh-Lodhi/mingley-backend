using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class Message : BaseEntity
{
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }

    public string? Text { get; set; }

    /// <summary>text | image | gift | coins</summary>
    public string Type { get; set; } = "text";

    public string? ImageUrl { get; set; }
    public string? GiftName { get; set; }
    public int? GiftCost { get; set; }
    public int? CoinAmount { get; set; }

    public DateTime? ReadAt { get; set; }
    public int? CoinsDeducted { get; set; }

    public Chat? Chat { get; set; }
    public User? Sender { get; set; }
}
