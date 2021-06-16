namespace MobileJO.Core.Contracts
{
    public interface IAppSettings
    {
        string PersistentText { get; set; }
        string AccessToken { get; set; }
        string RefreshToken { get; set; }
        string UserID { get; set; }
        string UserName { get; set; }
        int LoginAttempts { get; set; }
        string UserTypeID { get; set; }
        string CompanyID { get; set; }
        string BranchID { get; set; }
    }
}