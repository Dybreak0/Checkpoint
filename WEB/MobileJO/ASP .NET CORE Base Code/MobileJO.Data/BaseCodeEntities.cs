using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MobileJO.Data.Models;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace MobileJO.Data
{
    public partial class BaseCodeEntities : IdentityDbContext<IdentityUser>
    {
        public BaseCodeEntities(DbContextOptions<BaseCodeEntities> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Disables cascade delete for tables with foreign key relationships
            var cascadeTables = modelBuilder.Model.GetEntityTypes()
                .SelectMany(foreignKeysTables => foreignKeysTables.GetForeignKeys())
                .Where(foreignKeysTables => !foreignKeysTables.IsOwnership &&
                       foreignKeysTables.DeleteBehavior == DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshToken>()
               .HasAlternateKey(c => c.Username)
               .HasName("refreshToken_UserId");
            modelBuilder.Entity<RefreshToken>()
                .HasAlternateKey(c => c.Token)
                .HasName("refreshToken_Token");

            modelBuilder.Entity<Template_Branch>()
                 .HasKey(c => new { c.BranchID, c.TemplateID });

            modelBuilder.Entity<Branch>()
                .HasOne(u => u.UserCreatedBy)
                .WithMany();

            modelBuilder.Entity<Branch>()
                .HasOne(u => u.UserUpdatedBy)
                .WithMany();

            modelBuilder.Entity<Company>()
                .HasOne(u => u.UserCreatedBy)
                .WithMany();

            modelBuilder.Entity<Company>()
                .HasOne(u => u.UserUpdateBy)
                .WithMany();



            foreach (var table in cascadeTables)
            {
                table.DeleteBehavior = DeleteBehavior.Restrict;
            }

            ModelBuilderExtensions.Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public void InsertNew(RefreshToken token)
        {
            var tokenModel = RefreshToken.SingleOrDefault(i => i.Username == token.Username);
            if (tokenModel != null)
            {
                RefreshToken.Remove(tokenModel);
                SaveChanges();
            }
            RefreshToken.Add(token);
            SaveChanges();
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<ApplicationType> ApplicationType { get; set; }
        public virtual DbSet<AssignedCase> AssignedCase { get; set; }
        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<BillingType> BillingType { get; set; }
        public virtual DbSet<JobOrder> JobOrder { get; set; }
        public virtual DbSet<JobOrderBillingType> JobOrderBillingType { get; set; }
        public virtual DbSet<JobOrderStatus> JobOrderStatus { get; set; }
        public virtual DbSet<EmailSetup> EmailSetup { get; set; }
        public virtual DbSet<EmailType> EmailType { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<RevertJobOrder> RevertJobOrder { get; set; }
        public virtual DbSet<RevertJobOrder> Revert { get; set; }
        public virtual DbSet<SyncLog> SyncLog { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<QuestionType> QuestionType { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Choice> Choice { get; set; }
        public virtual DbSet<Response> Response { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<Template_Branch> Template_Branch { get; set; }
        public virtual DbSet<ForgotPassword> ForgotPassword { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Loan> Loan { get; set; }
        public virtual DbSet<LoanAttachment> LoanAttachment { get; set; }
        public virtual DbSet<LoanCreditHistory> LoanCreditHistory { get; set; }
        public virtual DbSet<LoanCustomerChildren> LoanCustomerChildren { get; set; }
        public virtual DbSet<LoanPersonalProperty> LoanPersonalProperty { get; set; }
        public virtual DbSet<LoanUnitDesired> LoanUnitDesired { get; set; }
        public virtual DbSet<LoanUnitDesiredTC> LoanUnitDesiredTC { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<RegionCity> RegionCity { get; set; }

        public virtual void SaveEmails(string emailsJson)
        {
            var parameters = new object[]
            {
                new SqlParameter("@emailsJson", (object)emailsJson ?? DBNull.Value),
            };
            var res = this.Database.ExecuteSqlCommand("SaveEmails @emailsJson", parameters);
        }
    }
}
