using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.RevertJO;

namespace MobileJO.Domain.Contracts
{
    public interface IRevertJOService
    {
        ListViewModel SearchRevertJO(RevertJOSearchViewModel searchModel);
        bool RevertJO(RevertJORequestViewModel requestModel);
        bool IsJobOrderExists(int jobOrderId);
        bool IsJobOrderForRevert(int jobOrderId);
    }
}