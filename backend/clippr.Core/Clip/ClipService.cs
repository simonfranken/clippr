using clippr.Core.Repository;
using clippr.Core.User;

namespace clippr.Core.Clip;

public class ClipService : IClipService
{
    private readonly IRepository<ClipModel> _repository;

    public ClipService(IRepository<ClipModel> repository)
    {
        _repository = repository;
    }

    public int CleanUp(TimeSpan maxAge)
    {
        var clipsToBeDeleted = _repository.Get().Where(clip => clip.CreatedAt < (DateTimeOffset.Now - maxAge)).ToList();
        _repository.Delete(clipsToBeDeleted);
        return clipsToBeDeleted.Count;
    }

    public void Create(ClipContent content, UserModel user)
    {
        var clip = new ClipModel(user, content);
        _repository.Add(clip);
    }
}