using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

/// <summary>Tracks call sessions for coin deduction (10/min audio, 100/min video)</summary>
public class CallSession : BaseEntity
{
    public Guid CallerId { get; set; }
    public Guid ReceiverId { get; set; }

    /// <summary>audio | video</summary>
    public string CallType { get; set; } = "audio";

    /// <summary>ringing | active | ended | declined | missed</summary>
    public string Status { get; set; } = "ringing";

    public DateTime? AnsweredAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int? DurationSeconds { get; set; }
    public int? CoinsDeducted { get; set; }
    public string? EndReason { get; set; }

    public User? Caller { get; set; }
    public User? Receiver { get; set; }
}
