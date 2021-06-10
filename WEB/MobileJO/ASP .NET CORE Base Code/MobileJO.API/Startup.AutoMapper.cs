using AutoMapper;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MobileJO.Data.ViewModels.Reports;
using MobileJO.Data.ViewModels.JobOrder;
using MobileJO.Data.ViewModels.LoanApplication;

namespace MobileJO.API
{
    public partial class Startup
    {
        private void ConfigureMapper(IServiceCollection services)
        {
            var Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<JobOrder, JobOrderReportViewModel>();
                cfg.CreateMap<JobOrderReportViewModel, JobOrder>();
                cfg.CreateMap<AssignedCase, AssignedCasesReportViewModel>();
                cfg.CreateMap<AssignedCasesReportViewModel, AssignedCase>();
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<UserViewModel, User>();
                cfg.CreateMap<User, UserEditViewModel>();
                cfg.CreateMap<UserEditViewModel, User>();
                cfg.CreateMap<JobOrder, JobOrderViewModel>();
                cfg.CreateMap<JobOrderViewModel, JobOrder>();
                cfg.CreateMap<RevertJobOrder, JobOrderRevertViewModel>();
                cfg.CreateMap<JobOrderRevertViewModel, RevertJobOrder>();
                cfg.CreateMap<JobOrder, JobOrderListViewModel>();
                cfg.CreateMap<JobOrderListViewModel, JobOrder>();
                cfg.CreateMap<Account, AccountViewModel>();
                cfg.CreateMap<AccountViewModel, Account>();
                cfg.CreateMap<EmailSetup, EmailSetupViewModel>();
                cfg.CreateMap<EmailSetupViewModel, EmailSetup>();

                cfg.CreateMap<Loan, LoanDetailsViewModel>();
                cfg.CreateMap<LoanDetailsViewModel, Loan>();
            });

            services.AddSingleton(Config.CreateMapper());
        }
    }
}