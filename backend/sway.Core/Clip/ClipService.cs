using sway.Core.Repository;
using sway.Core.User;

namespace sway.Core.Clip;

public class ClipService : IClipService
{
    private readonly IRepository<ClipModel> _repository;

    public ClipService(IRepository<ClipModel> repository)
    {
        _repository = repository;
    }

    public void Create(ClipContent content, UserModel user)
    {
        var clip = new ClipModel(user, content);
        _repository.Add(clip);
    }
}