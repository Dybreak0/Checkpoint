using MobileJO.Data;
using MobileJO.Data.ViewModels.RevertJO;
using MobileJO.Domain.Contracts;

namespace MobileJO.Domain.Handlers
{
    public class RevertJOHandler
    {
        private readonly IRevertJOService _revertJOService;

        public RevertJOHandler(IRevertJOService revertJOService)
        {
            _revertJOService = revertJOService;
        }

        /// <summary>
        ///     Used to check if a job order revert request can be reverted
        /// </summary>
        /// <param name="requestModel">Holds the job order revert request data</param>
        /// <returns>Holds the list of validation errors</returns>
        public ValidationResult CanRevert(RevertJORequestViewModel requestModel)
        {
            ValidationResult validationErrors = null;

            if (_revertJOService.IsJobOrderExists(requestModel.JobOrderId))
            {
                if (!_revertJOService.IsJobOrderForRevert(requestModel.JobOrderId))
                {
                    validationErrors = new ValidationResult(Constants.RevertJO.NotForRevert);
                }
            }
            else
            {
                validationErrors = new ValidationResult(Constants.Common.RecordDoesNotExist);
            }

            return validationErrors;
        }        
    }
}