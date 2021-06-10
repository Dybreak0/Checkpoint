using MobileJO.Data.Models;

namespace MobileJO.Domain.Contracts
{
    public interface ISyncLogService
    {
        void Create(SyncLog syncLog);
    }
}
