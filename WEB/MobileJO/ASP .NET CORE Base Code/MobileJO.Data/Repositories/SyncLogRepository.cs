using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using System;

namespace MobileJO.Data.Repositories
{
    public class SyncLogRepository : BaseRepository, ISyncLogRepository
    {
        public SyncLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        protected BaseCodeEntities newContext => (BaseCodeEntities)UnitOfWork.Database;

        public void Create(SyncLog syncLog)
        {
            using (var logTransaction = newContext.Database.BeginTransaction())
            {
                try
                {
                    GetDbSet<SyncLog>().Add(syncLog);
                    UnitOfWork.SaveChanges();
                    logTransaction.Commit();
                }
                catch(Exception e)
                {
                    logTransaction.Commit();
                }
            }
        }
    }
}
