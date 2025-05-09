using Microsoft.EntityFrameworkCore;
using sway.Core.Clip;

namespace sway.Repository.Repositories;

public class ClipRepository : Repository<ClipModel>
{
    public ClipRepository(SwayDbContext dbContext) : base(dbContext)
    {
    }

    public override void Add(ClipModel entity)
    {
        _dbContext.Add(entity);
        _dbContext.Update(entity.User);
        _dbContext.SaveChanges();
        _dbContext.Entry(entity).State = EntityState.Detached;
        _dbContext.Entry(entity.User).State = EntityState.Detached;
    }
}