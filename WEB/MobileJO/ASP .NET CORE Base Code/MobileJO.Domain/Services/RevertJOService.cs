using MobileJO.Data.Contracts;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.RevertJO;
using MobileJO.Domain.Contracts;

namespace MobileJO.Domain.Services
{
    public class RevertJOService : IRevertJOService
    {
        private readonly IRevertJORepository _revertJORepository;
        
        public RevertJOService (IRevertJORepository revertJORepository)
        {
            _revertJORepository = revertJORepository;
        }

        /// <summary>
        ///     Used to call the repository layer to check if a job order record exists
        /// </summary>
        /// <param name="jobOrderId">Hold the job order ID</param>
        /// <returns>Holds the value whether job order exists or not</returns>
        public bool IsJobOrderExists(int jobOrderId)
        {
            return _revertJORepository.IsJobOrderExists(jobOrderId);
        }

        /// <summary>
        ///     Used to call the repository layer to check if a job order status is Requested For Revert
        /// </summary>
        /// <param name="jobOrderId">Hold the job order ID</param>
        /// <returns>Holds the value whether job order status is Requested For Revert</returns>
        public bool IsJobOrderForRevert(int jobOrderId)
        {
            return _revertJORepository.IsJobOrderForRevert(jobOrderId);
        }

        /// <summary>
        ///     Used to call the repository layer for the approval/denial of the job order revert request
        /// </summary>
        /// <param name="requestModel">Holds the job order revert request data</param>
        /// <returns>Holds the value whether the job order revert request was successfully approved/denied</returns>
        public bool RevertJO(RevertJORequestViewModel requestModel)
        {
            return _revertJORepository.RevertJO(requestModel);
        }

        /// <summary>
        ///     Used to call the repository layer and retrieve job order revert request records
        /// </summary>
        /// <param name="searchModel">Holds job order revert request search filters</param>
        /// <returns>Holds the list of job order revert request records</returns>
        public ListViewModel SearchRevertJO(RevertJOSearchViewModel searchModel)
        {
            return _revertJORepository.SearchRevertJO(searchModel);
        }
    }
}