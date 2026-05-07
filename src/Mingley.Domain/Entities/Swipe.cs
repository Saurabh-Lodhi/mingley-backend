using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class Swipe : BaseEntity
{
    public Guid SwiperId { get; set; }
    public Guid TargetId { get; set; }

    /// <summary>like | dislike | superlike</summary>
    public string Action { get; set; } = "like";

    public User? Swiper { get; set; }
    public User? Target { get; set; }
}
