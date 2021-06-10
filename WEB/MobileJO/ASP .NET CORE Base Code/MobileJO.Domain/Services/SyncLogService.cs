using AutoMapper;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Domain.Contracts;

namespace MobileJO.Domain.Services
{
    public class SyncLogService : ISyncLogService
    {
        private readonly ISyncLogRepository _syncLogRepository;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Constructor for IAssignedCasesRepository and IMapper
        /// </summary>
        /// <param name="syncLogRepository"></param>
        /// <param name="mapper"></param>
        public SyncLogService(ISyncLogRepository syncLogRepository, IMapper mapper)
        {
            _syncLogRepository = syncLogRepository;
            _mapper = mapper;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="syncLog"></param>
        /// <returns></returns>
        public void Create(SyncLog syncLog)
        {
            _syncLogRepository.Create(syncLog);
        }
    }
}
