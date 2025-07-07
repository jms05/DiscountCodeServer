using JMS.Application.Helpers;
using JMS.Domain.Models.DiscountCodes;
using JMS.Domain.Models.DiscountCodes.Repository;
using JMS.Plugins.EntityFramework;
using JMS.Plugins.EntityFramework.Application;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFrameworkRepository.Application.Repositories
{
    public class DiscountCodeRepository : RepositoryBase<ApplicationDbContext, DiscountCode>, IAddDiscountCode, IListDiscountCode, IGetDiscountCode, IUpdateDiscountCode, IDeleteDiscountCode
    {

        public DiscountCodeRepository(ApplicationDbContext dbContext, ICustomLogger logger)
        : base(dbContext,logger) { }

        async Task IAddDiscountCode.ExecuteAsync(DiscountCode discountCode, CancellationToken cancellationToken)
        {
            await DbSet.AddAsync(discountCode, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        async Task IAddDiscountCode.ExecuteAsync(IEnumerable<DiscountCode> discountCodes, CancellationToken cancellationToken)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    await DbSet.AddRangeAsync(discountCodes, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch (DbUpdateException)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.Log("Failed to add discount codes. Transaction rolled back.");
                }
            }
        }

        async Task<bool> IUpdateDiscountCode.ExecuteAsync(DiscountCode discountCode, CancellationToken cancellationToken)
        {
            DbSet.Update(discountCode);
            return await _dbContext.SaveChangesAsync(cancellationToken) == 1;
        }

        async Task<DiscountCode?> IGetDiscountCode.ExecuteAsync(string discountCode, bool trackCkanges, CancellationToken cancellationToken)
        {
            return await Query(trackCkanges)
            .Where(discountCodeEF => discountCodeEF.Code.Equals(discountCode))
            .FirstOrDefaultAsync(cancellationToken);
        }

        async Task<IEnumerable<DiscountCode>> IListDiscountCode.ExecuteAsync(ListDiscountCodeFilter filter, bool trackChanges, CancellationToken cancellationToken)
        {
            var baseQuery = Query(trackChanges);

            if (filter.Codes!= null && filter.Codes.Any())
                baseQuery = baseQuery
                    .Where(discountCode => filter.Codes.Contains(discountCode.Code));

            if(!filter.IncludeUsed )
            baseQuery = baseQuery
                        .Where(discountCode => !discountCode.Used);

            return await baseQuery.ToListAsync();
        }

        Task IDeleteDiscountCode.ExecuteAsync(DiscountCode discountCode, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
