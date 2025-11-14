namespace Estately.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TblAppointment> AppointmentRepository { get; }
        IRepository<TblAppointmentStatus> AppointmentStatusRepository { get; }
        IRepository<TblBranch> BranchRepository { get; }
        IRepository<TblBranchDepartment> BranchDepartmentRepository { get; }
        IRepository<TblCity> CityRepository { get; }
        IRepository<TblClientProfile> ClientProfileRepository { get; }
        IRepository<TblDepartment> DepartmentRepository { get; }
        IRepository<TblDeveloperProfile> DeveloperProfileRepository { get; }
        IRepository<TblDocumentType> DocumentTypeRepository { get; }
        IRepository<TblEmployee> EmployeeRepository { get; }
        IRepository<TblEmployeeClient> EmployeeClientRepository { get; }
        IRepository<TblFavorite> FavoriteRepository { get; }
        IRepository<TblProperty> PropertyRepository { get; }
        IRepository<TblPropertyDocument> PropertyDocumentRepository { get; }
        IRepository<TblPropertyFeature> PropertyFeatureRepository { get; }
        IRepository<TblPropertyFeaturesMapping> PropertyFeaturesMappingRepository { get; }
        IRepository<TblPropertyHistory> PropertyHistoryRepository { get; }
        IRepository<TblPropertyImage> PropertyImageRepository { get; }
        IRepository<TblPropertyStatus> PropertyStatusRepository { get; }
        IRepository<TblPropertyType> PropertyTypeRepository { get; }
        IRepository<TblUser> UserRepository { get; }
        IRepository<TblUserType> UserTypeRepository { get; }
        IRepository<TblZone> ZoneRepository { get; }
        IRepository<LKPPropertyHistoryType> PropertyHistoryTypeRepository { get; }

        int Complete();
    }
}
