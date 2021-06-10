using AutoMapper;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.SyncLog;
using Microsoft.Extensions.DependencyInjection;
using MobileJO.Data.ViewModels.AssignedCase;
using MobileJO.Data.ViewModels.JobOrder;

namespace MobileJO.Domain
{
    public class MapperConfig
    {
        public static void ConfigureMapper(IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssignedCase, AssignedCasesViewModel>();
                cfg.CreateMap<AssignedCasesViewModel, AssignedCase>();
                cfg.CreateMap<JobOrder, JobOrderViewModel>();
                cfg.CreateMap<JobOrderViewModel, JobOrder>();
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<Account, AccountViewModel>();
                cfg.CreateMap<AccountViewModel, Account>();
                cfg.CreateMap<UserViewModel, User>();
                cfg.CreateMap<User, UserEditViewModel>();
                cfg.CreateMap<UserEditViewModel, User>();
                cfg.CreateMap<EmailSetup, EmailSetupViewModel>();
                cfg.CreateMap<EmailSetupViewModel, EmailSetup>();
                cfg.CreateMap<Account, AccountViewModel>();
                cfg.CreateMap<AccountViewModel, Account>();
                cfg.CreateMap<RevertJobOrder, JobOrderRevertViewModel>();
                cfg.CreateMap<JobOrderRevertViewModel, RevertJobOrder>();
                cfg.CreateMap<SyncLog, SyncLogViewModel>();
                cfg.CreateMap<SyncLogViewModel, SyncLog>();
            });

            services.AddSingleton(sp => mapperConfiguration.CreateMapper());
        }
    }
}
