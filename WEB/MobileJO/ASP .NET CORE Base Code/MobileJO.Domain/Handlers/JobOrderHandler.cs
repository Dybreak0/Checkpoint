using MobileJO.Data.ViewModels;
using MobileJO.Domain.Contracts;
using System.Collections.Generic;
using Constants = MobileJO.Data.Constants;
using MobileJO.Data.ViewModels;

namespace MobileJO.Domain.Handlers
{
    public class JobOrderHandler
    {
        private readonly IJobOrderService _jobOrderService;

        public JobOrderHandler(IJobOrderService jobOrderService)
        {
            _jobOrderService = jobOrderService;
        }

        public IEnumerable<ValidationResult> CanDelete(int id)
        {
            var validationErrors = new List<ValidationResult>();

            var jobOrder = _jobOrderService.Find(id);
            if (jobOrder == null)
            {
                if (jobOrder.StatusID != Constants.Common.PendingValue)
                {
                    validationErrors.Add(new ValidationResult(Constants.Common.CannotDelete));
                }

                else
                {
                    validationErrors.Add(new ValidationResult(Constants.Common.SuccessDelete));
                }
            }
            return validationErrors;
        }

        /// <summary>
        ///     Used to check if a job order revert request can be reverted
        /// </summary>
        public IEnumerable<ValidationResult> CanRequestRevert(int id)
        {
            var validationErrors = new List<ValidationResult>();

                var dbJobOrder = _jobOrderService.Find(id);
                var jobOrderId = dbJobOrder.JobOrderNumber;

                if (id > 0 && jobOrderId != null)
                {

                    if (dbJobOrder.StatusID.Equals(Constants.Common.RequestRevertValue))
                    {
                        validationErrors.Add(new ValidationResult(Constants.JOStatus.RequestRevert));
                    }

                    else if (dbJobOrder.StatusID.Equals(Constants.Common.PendingValue)) 
                    {
                        validationErrors.Add(new ValidationResult(Constants.JOStatus.CannotRevertJO));
                    }

                   else if (dbJobOrder.StatusID.Equals(Constants.Common.SignedValue))
                   {
                    validationErrors.Add(new ValidationResult(Constants.JOStatus.Signed));
                   }
            }
         
            else
            {
                validationErrors.Add(new ValidationResult(Constants.Common.InvalidJobOrderId));
            }

            return validationErrors;
        }

        public IEnumerable<ValidationResult> CanUpdate(JobOrderDetailsViewModel jobOrderVM)
        {
            var validationResult = new List<ValidationResult>();

            if (jobOrderVM.ID > 0)
            {
                if (_jobOrderService.IsJobOrderExists(jobOrderVM.ID))
                {
                    validationResult = new List<ValidationResult>();
                }
                else
                {
                    validationResult.Add(new ValidationResult(Constants.Common.RecordDoesNotExist));
                }
            }
            else
            {
                validationResult.Add(new ValidationResult(Constants.Common.InvalidJobOrderId));
            }

            return validationResult;
        }
    }
}
