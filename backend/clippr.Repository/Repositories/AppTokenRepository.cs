using Microsoft.EntityFrameworkCore;
using clippr.Core.Clip;
using clippr.Core.AppToken;

namespace clippr.Repository.Repositories;

public class AppTokenRepository : Repository<AppTokenModel>
{
    public AppTokenRepository(ClipprDbContext dbContext) : base(dbContext)
    {
    }

    public override void Add(AppTokenModel entity)
    {
        _dbContext.Add(entity);
        _dbContext.Update(entity.User);
        _dbContext.SaveChanges();
        _dbContext.Entry(entity).State = EntityState.Detached;
        _dbContext.Entry(entity.User).State = EntityState.Detached;
    }
}