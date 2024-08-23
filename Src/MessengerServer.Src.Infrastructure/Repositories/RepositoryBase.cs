using MessengerServer.Src.Domain.Entities;
using MessengerServer.Src.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MessengerServer.Src.Infrastructure.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext _context;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
    }

    public void Add(TEntity model)
    {
        model.CreatedDate = DateTime.Now;
        _context.Set<TEntity>().Add(model);
    }

    public void AddRange(IEnumerable<TEntity> model)
    {
        foreach (var entity in model)
        {
            entity.CreatedDate = DateTime.Now;
        }
        _context.Set<TEntity>().AddRange(model);
    }

    public TEntity? GetId(Guid id)
    {
        return _context.Set<TEntity>().Find(id);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().FirstOrDefault(predicate);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().Where<TEntity>(predicate).ToList();
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Task.Run(() => _context.Set<TEntity>().Where<TEntity>(predicate));
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().ToList();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Task.Run(() => _context.Set<TEntity>());
    }

    public int Count()
    {
        return _context.Set<TEntity>().Count();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Set<TEntity>().CountAsync();
    }

    public void Update(TEntity objModel)
    {
        objModel.ModifiedDate = DateTime.Now;
        _context.Entry(objModel).State = EntityState.Modified;
    }

    public void Remove(TEntity objModel)
    {
        _context.Set<TEntity>().Remove(objModel);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}