// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Ardalis.Specification;
using MonkeyStore.Models.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonkeyStore.Repositories
{
    public interface IAsyncRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(int id);

        Task<IReadOnlyList<TEntity>> ListAllAsync();

        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec);

        Task<TEntity> AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<int> CountAsync(ISpecification<TEntity> spec);

        Task<TEntity> FirstAsync(ISpecification<TEntity> spec);

        Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> spec);
    }
}
