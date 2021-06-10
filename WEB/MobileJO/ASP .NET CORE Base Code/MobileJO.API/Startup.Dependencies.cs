using Microsoft.Extensions.DependencyInjection;
using MobileJO.API.Authentication;
using MobileJO.Data;
using MobileJO.Data.Contracts;
using MobileJO.Data.Repositories;
using MobileJO.Domain.Contracts;
using MobileJO.Domain.Services;

namespace MobileJO.API
{
    public partial class Startup
    {
        private void ConfigureDependencies(IServiceCollection services)
        {            
            // Common
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ClaimsProvider, ClaimsProvider>();
            services.AddScoped<ClaimsProvider>();

            // Services
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IAssignedCasesService, AssignedCasesService>();
            services.AddScoped<IEmailJOService, EmailJOService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IDropdownService, DropdownService>();
            services.AddScoped<IRevertJOService, RevertJOService>();
            services.AddScoped<IJobOrderService, JobOrderService>();
            services.AddScoped<IEmailJOService, EmailJOService>();
            services.AddScoped<ISyncLogService, SyncLogService>();
            services.AddScoped<IResponseService, ResponseService>();
            services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
            services.AddScoped<IQuestionnaireService, QuestionnaireService>();
            services.AddScoped<ILoanService, LoanService>();

            // Repositories
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IDropdownRepository, DropdownRepository>();
            services.AddScoped<IRevertJORepository, RevertJORepository>();
            services.AddScoped<IEmailJORepository, EmailJORepository>();
            services.AddScoped<IAssignedCasesRepository, AssignedCasesRepository>();            
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IJobOrderRepository, JobOrderRepository>();
            services.AddScoped<ISyncLogRepository, SyncLogRepository>();
            services.AddScoped<IResponseRepository, ResponseRepository>();
            services.AddScoped<IForgotPasswordRepository, ForgotPasswordRepository>();
            services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
        }
    }
}
