using MobileJO.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;

namespace MobileJO.Data.Contracts
{
    public interface ISyncLogRepository
    {
        void Create(SyncLog syncLog);
    }
}
