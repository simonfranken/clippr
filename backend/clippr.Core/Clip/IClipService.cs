using clippr.Core.User;

namespace clippr.Core.Clip;

public interface IClipService
{
    public void Create(ClipContent content, UserModel user);
    public int CleanUp(TimeSpan maxAge);
}