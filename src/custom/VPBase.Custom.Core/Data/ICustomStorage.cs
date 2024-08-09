/*═══════════════════════════════════════════════════════════════════════════════════════════╗
║ MODIFY CAREFULLY: ICustomStorage
╟────────────────────────────────────────────────────────────────────────────────────────────╢
║ This file is provided from base.
║ If you do not like painful updates in the future, be careful with the changes you make
║ to this file. Try to keep your changes to the highlighted areas below.
╚═══════════════════════════════════════════════════════════════════════════════════════════*/
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using VPBase.Custom.Core.Data.Entities;

namespace VPBase.Custom.Core.Data
{
    public interface ICustomStorage : IDisposable
    {
        /* VP_Template_Mvc BEGIN */
        IQueryable<VP_Template_Mvc> VP_Template_Mvcs { get; }
        /* VP_Template_Mvc END */

        /* VP_Template_SimpleMvcs BEGIN */
        IQueryable<VP_Template_SimpleMvc> VP_Template_SimpleMvcs { get; }
        /* VP_Template_SimpleMvcs END */

        /*═══════════════════════════════════════════════════════════════════════════════════════════
        ║ Custom modifications - START 
        ║ */



        /* Custom modifications - END                                                               ║
        ════════════════════════════════════════════════════════════════════════════════════════════*/

        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
        bool IsInMemory { get; }

        int SaveChanges();
        T Attach<T>(T entity) where T : class;
        T Add<T>(T entity) where T : class;
        T Delete<T>(T entity) where T : class;

        // Async

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
