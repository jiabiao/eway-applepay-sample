// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonkeyStore.Data.Catalog;
using MonkeyStore.Models.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonkeyStore.Repositories
{
    public class EfRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly CatalogContext _dbContext;

        public EfRepository(CatalogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IReadOnlyList<TEntity>> ListAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.CountAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> FirstAsync(ISpecification<TEntity> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstOrDefaultAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            var evaluator = new SpecificationEvaluator<TEntity>();
            return evaluator.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), spec);
        }
    }
}
