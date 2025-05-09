using clippr.Core.User;

namespace clippr.Core.Clip;

public class ClipModel
{
    public ClipModel(Guid id, DateTimeOffset createdAt)
    {
        Id = id;
        CreatedAt = createdAt;
    }
    public ClipModel(UserModel user, ClipContent content)
    {
        Id = Guid.NewGuid();
        User = user;
        Content = content;
        CreatedAt = DateTimeOffset.Now;
    }

    public Guid Id { get; set; }
    public virtual UserModel User { get; set; }
    public virtual ClipContent Content { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}