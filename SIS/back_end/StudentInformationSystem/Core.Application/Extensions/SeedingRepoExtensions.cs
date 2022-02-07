using Core.Application.Interfaces;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Application.Extensions
{
    static public class SeedingRepoExtensions
    {
        /// <summary>
        /// Use this to seed data in a repostory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">Repository to fill with seed data</param>
        /// <param name="list">List of entities to add as seed data</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static public async Task SeedDataAsync<T>(this IRepository<T> repository, IEnumerable<T> list, CancellationToken cancellationToken) where T: EntityBase
        {

            // info: https://docs.microsoft.com/en-us/ef/core/saving/transactions#using-systemtransactions
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            foreach (T item in list)
            {
                if (repository.Query().Any(e => e.Id == item.Id))
                    throw new Exception("Duplicate entity ID attempted to seed in database.");
                repository.Add(item);
            }
            await repository.CommitAsync(cancellationToken).ConfigureAwait(false);
            scope.Complete();
        }

        /// <summary>
        /// Use this to seed data in a one to many navigation relationship.
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <typeparam name="M"></typeparam>
        /// <param name="manyRepository">Destination repository containing entities to seed</param>
        /// <param name="itemsToAdd">Originating entities to seed</param>
        /// <param name="itemsToAddTo">Destination navigation property list to fill</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static public async Task SeedDataOneToMany<O, M>(this IRepository<M> manyRepository, IEnumerable<O> itemsToAdd, IList<O> itemsToAddTo
            , CancellationToken cancellationToken) where O : EntityBase where M : EntityBase
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            foreach(var itemToAdd in itemsToAdd)
            {
                if (itemsToAddTo.Any(e => e.Id == itemToAdd.Id))
                    throw new Exception("Duplicate entity ID attempted to seed in database.");
                itemsToAddTo.Add(itemToAdd);
            }
            await manyRepository.CommitAsync(cancellationToken).ConfigureAwait(false);
            scope.Complete();
        }

    }
}
