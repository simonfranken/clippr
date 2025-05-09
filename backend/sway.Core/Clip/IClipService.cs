using sway.Core.User;

namespace sway.Core.Clip;

public interface IClipService
{
    public void Create(ClipContent content, UserModel user);
}