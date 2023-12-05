using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookStoreApp.API.Repository.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> GetSingleOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
    Task<T> GetFirstOrDefaultWithNoTracking(Expression<Func<T, bool>> filter, string? includeProperties = null);
    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    void IsModified(T obj, Expression<Func<T, string>> filter, bool isModified);
    Task<EntityEntry<T>> AddAsync(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entity);
    Task AddRange(IEnumerable<T> entity);
    Task<int> SaveChangesAsync();
    Task SetEntityState(T entity, EntityState state);
}