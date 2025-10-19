﻿using KwikNesta.Infrastruture.Svc.Domain.Entities;
using KwikNesta.Infrastruture.Svc.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KwikNesta.Infrastruture.Svc.Infrastructure.Repositories
{
    public class Repository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<TEntity?> FindByIdAsync(Guid id,
                                                 bool track = false,
                                                 CancellationToken cancellation = default)
        {
            return track ?
                await _context.Set<TEntity>()
                    .FirstOrDefaultAsync(i => i.Id == id, cancellation) :
                await _context.Set<TEntity>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id, cancellation);
        }

        public async Task AddAsync(TEntity entity,
                                   bool save = true,
                                   CancellationToken cancellation = default)
        {
            await _context.Set<TEntity>().AddAsync(entity, cancellation);
            if (save)
            {
                await SaveAsync(cancellation);
            }
        }

        public async Task AddRangeAsync(List<TEntity> entity,
                                   bool save = true,
                                   CancellationToken cancellation = default)
        {
            await _context.Set<TEntity>().AddRangeAsync(entity, cancellation);
            if (save)
            {
                await SaveAsync(cancellation);
            }
        }

        public async Task UpdateAsync(TEntity entity,
                                      bool save = true,
                                      CancellationToken cancellation = default)
        {
            _context.Set<TEntity>().Update(entity);
            if (save)
            {
                await SaveAsync(cancellation);
            }
        }

        public async Task DeleteAsync(TEntity entity,
                                      bool save = true,
                                      CancellationToken cancellation = default)
        {
            _context.Set<TEntity>().Remove(entity);
            if (save)
            {
                await SaveAsync(cancellation);
            }
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>()
               .Where(expression)
               .AsNoTracking()
               .AsQueryable();
        }

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression)
                .AnyAsync();
        }

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression)
                .LongCountAsync();
        }

        private async Task<bool> SaveAsync(CancellationToken cancellation)
        {
            return await _context.SaveChangesAsync(cancellation) > 0;
        }
    }
}