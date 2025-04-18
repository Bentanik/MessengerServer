﻿using System.Linq.Expressions;

namespace MessengerServer.Src.Application.Repositories;

public interface IRepositoryBase<T> where T : class
{
    void Add(T objModel);
    void AddRange(IEnumerable<T> objModel);
    T? GetId(Guid id);
    Task<T?> GetByIdAsync(Guid id);
    T? Get(Expression<Func<T, bool>> predicate);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    IEnumerable<T> GetList(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate);
    IEnumerable<T> GetAll();
    Task<IEnumerable<T>> GetAllAsync();
    int Count();
    Task<int> CountAsync();
    void Update(T objModel);
    void Remove(T objModel);
    void Dispose();
}
