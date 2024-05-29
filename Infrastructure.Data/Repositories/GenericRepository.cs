using Domain.Contracts.Repositories;
using Domain.Contracts.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using Domain.Base;
using Infrastructure.Data.Data;
using Domain.Contracts.Data;

namespace Infrastructure.Data.Repositories
{
    public class GenericRepository<TEntity, TKey, TDbContext>(TDbContext context, ITenant tenant) : IGenericRepository<TEntity, TKey>
        where TEntity : class, IEntity, IEntity<TKey>
        where TDbContext : IApplicationDbContext
    {
     

        protected readonly DbSet<TEntity> _dbSet = (context as DbContext).Set<TEntity>();
        private readonly ITenant _tenant = tenant;

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task UpdateAsync(TEntity entity)
        {
             _dbSet.Update(entity);
             await Task.CompletedTask;
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
             _dbSet.UpdateRange(entities);
            await Task.CompletedTask;
        }

        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = ApplyDefaultFilters(_dbSet.AsQueryable());
            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> DeleteAsync(params object[] keyValues)
        {
            var entity = await _dbSet.FindAsync(keyValues);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await _dbSet.FindAsync(cancellationToken, keyValues);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return true;
            }
            return false;
        }

       

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await _dbSet.AsQueryable().Where(predicate).ToListAsync();
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<TEntity>> FilterAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await ApplyDefaultFilters(query).ToListAsync();
        }

        public IQueryable<TEntity> GetAll(bool withoutDefaultFilters = false)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable<TEntity>();

            if (withoutDefaultFilters)
            {
                if (typeof(ITenant).IsAssignableFrom(typeof(TEntity)) && _tenant.TenantId != null)
                {
                    query = query.Where(x => ((ITenant)x).TenantId == _tenant.TenantId);
                }
                return query;
            }

            return ApplyDefaultFilters(query);
        }

        protected IQueryable<TEntity> ApplyDefaultFilters(IQueryable<TEntity> query)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(x => ((ISoftDelete)x).IsDeleted == false);
            }
            if (typeof(ITenant).IsAssignableFrom(typeof(TEntity)) && _tenant.TenantId != null)
            {
                query = query.Where(x => ((ITenant)x).TenantId == _tenant.TenantId);
            }
            return query;
        }
    }
}
