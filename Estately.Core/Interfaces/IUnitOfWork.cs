namespace Estately.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TblAppointment> AppointmentRepository { get; }
        IRepository<TblBranch> BranchRepository { get; }
        IRepository<TblCity> CityRepository { get; }
        IRepository<TblClientProfile> ClientProfileRepository { get; }
        IRepository<TblCommission> CommissionRepository { get; }
        IRepository<TblDepartment> DepartmentRepository { get; }
        IRepository<TblDeveloperProfile> DeveloperProfileRepository { get; }
        IRepository<TblEmployee> EmployeeRepository { get; }
        IRepository<TblEmployeeClient> EmployeeClientRepository { get; }
        IRepository<TblFavorite> FavoriteRepository { get; }
        IRepository<TblPayment> PaymentRepository { get; }
        IRepository<TblProperty> PropertyRepository { get; }
        IRepository<TblPropertyFeature> PropertyFeatureRepository { get; }
        IRepository<TblPropertyFeaturesMapping> PropertyFeaturesMappingRepository { get; }
        IRepository<TblPropertyImage> PropertyImageRepository { get; }
        IRepository<TblPropertyType> PropertyTypeRepository { get; }
        IRepository<TblTransaction> TransactionRepository { get; }
        IRepository<TblUser> UserRepository { get; }
        IRepository<TblUserType> UserTypeRepository { get; }
        IRepository<TblZone> ZoneRepository { get; }
        int Complete();
    }
}
