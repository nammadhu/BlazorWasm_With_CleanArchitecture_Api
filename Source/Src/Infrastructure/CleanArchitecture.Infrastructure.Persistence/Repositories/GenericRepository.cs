using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using SharedResponse.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Persistence.Repositories;

public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : class
{
    public virtual async Task<T> GetByIdIntAsync(int id)
        {
        var result = await dbContext.Set<T>().FindAsync(id);
        if (result != null)
            dbContext.Entry(result).State = EntityState.Detached;//to avoid error of Already tracking object
        return result;
        }
    public virtual async Task<T> GetByIdAsync(long id)
    {
        return await dbContext.Set<T>().FindAsync(id);
    }

        public async Task<T> AddAsync(T entity)
            {
            //_dbContext.Entry(entity).State = EntityState.Added;//may be required but not sure 
            await dbContext.Set<T>().AddAsync(entity);
            return entity;
            }

    public void Update(T entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        dbContext.Set<T>().Remove(entity);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await dbContext
             .Set<T>()
             .AsNoTracking()
             .ToListAsync();
    }

        protected async Task<PagenationResponseDto<TEntity>> Paged<TEntity>(IQueryable<TEntity> query, int pageNumber, int pageSize) where TEntity : class
            {
            if (pageSize == int.MaxValue && pageNumber == 1)//means all
                {
                List<TEntity> resultAll = await query.AsNoTracking().ToListAsync();
                return new(resultAll, resultAll.Count, 1, pageSize);
                }

            var count = await query.CountAsync();
            List<TEntity> pagedResult = await query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize).AsNoTracking()
                                .ToListAsync();

            return new(pagedResult, count, pageNumber, pageSize);
            }
        }