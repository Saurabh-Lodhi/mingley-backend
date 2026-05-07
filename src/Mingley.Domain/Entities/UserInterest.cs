namespace Mingley.Domain.Entities;

public class UserInterest
{
    public Guid UserId { get; set; }
    public Guid InterestId { get; set; }
    public User? User { get; set; }
    public Interest? Interest { get; set; }
}
