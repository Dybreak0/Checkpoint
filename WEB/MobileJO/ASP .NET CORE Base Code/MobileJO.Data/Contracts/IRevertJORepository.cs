using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.RevertJO;
using System.Linq;

namespace MobileJO.Data.Contracts
{
    public interface IRevertJORepository
    {
        IQueryable<RevertJobOrder> RetrieveRevertJO();
        bool IsJobOrderExists(int jobOrderId);
        bool IsJobOrderForRevert(int jobOrderId);
        bool RevertJO(RevertJORequestViewModel requestModel);
        RevertJobOrder FindRevertJOId(int jobOrderId);
        ListViewModel SearchRevertJO(RevertJOSearchViewModel searchModel);
    }
}