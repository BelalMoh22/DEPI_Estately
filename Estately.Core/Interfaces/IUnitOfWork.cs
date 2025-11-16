namespace Estately.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TblAppointment> AppointmentRepository { get; }
        IRepository<LkpAppointmentStatus> AppointmentStatusRepository { get; }
        IRepository<TblBranch> BranchRepository { get; }
        IRepository<TblBranchDepartment> BranchDepartmentRepository { get; }
        IRepository<TblCity> CityRepository { get; }
        IRepository<TblClientProfile> ClientProfileRepository { get; }
        IRepository<TblClientPropertyInterest> ClientPropertyInterestRepository { get; }
        IRepository<TblDepartment> DepartmentRepository { get; }
        IRepository<TblDeveloperProfile> DeveloperProfileRepository { get; }
        IRepository<LkpDocumentType> DocumentTypeRepository { get; }
        IRepository<TblEmployee> EmployeeRepository { get; }
        IRepository<TblEmployeeClient> EmployeeClientRepository { get; }
        IRepository<TblFavorite> FavoriteRepository { get; }
        IRepository<TblJobTitle> JobTitleRepository { get; }
        IRepository<TblProperty> PropertyRepository { get; }
        IRepository<TblPropertyDocument> PropertyDocumentRepository { get; }
        IRepository<TblPropertyFeature> PropertyFeatureRepository { get; }
        IRepository<TblPropertyFeaturesMapping> PropertyFeaturesMappingRepository { get; }
        IRepository<TblPropertyHistory> PropertyHistoryRepository { get; }
        IRepository<TblPropertyImage> PropertyImageRepository { get; }
        IRepository<LkpPropertyStatus> PropertyStatusRepository { get; }
        IRepository<LkpPropertyHistoryType> PropertyHistoryTypeRepository { get; }
        IRepository<LkpPropertyType> PropertyTypeRepository { get; }
        IRepository<TblUser> UserRepository { get; }
        IRepository<LkpUserType> UserTypeRepository { get; }
        IRepository<TblZone> ZoneRepository { get; }
        Task<int> CompleteAsync();
    }
}