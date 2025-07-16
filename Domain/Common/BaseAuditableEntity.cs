namespace Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedUtc { get; private set; } = DateTime.UtcNow;
    public DateTime ModifiedUtc { get; private set; } = DateTime.UtcNow;

    public void Touch() => ModifiedUtc = DateTime.UtcNow;
}
