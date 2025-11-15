namespace Estately.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;

        public IRepository<TblAppointment> AppointmentRepository { get; }
        public IRepository<LkpAppointmentStatus> AppointmentStatusRepository { get; }
        public IRepository<TblBranch> BranchRepository { get; }
        public IRepository<TblBranchDepartment> BranchDepartmentRepository { get; }
        public IRepository<TblCity> CityRepository { get; }
        public IRepository<TblClientProfile> ClientProfileRepository { get; }
        public IRepository<TblDepartment> DepartmentRepository { get; }
        public IRepository<TblDeveloperProfile> DeveloperProfileRepository { get; }
        public IRepository<LkpDocumentType> DocumentTypeRepository { get; }
        public IRepository<TblEmployee> EmployeeRepository { get; }
        public IRepository<TblEmployeeClient> EmployeeClientRepository { get; }
        public IRepository<TblFavorite> FavoriteRepository { get; }
        public IRepository<TblProperty> PropertyRepository { get; }
        public IRepository<TblPropertyDocument> PropertyDocumentRepository { get; }
        public IRepository<TblPropertyFeature> PropertyFeatureRepository { get; }
        public IRepository<TblPropertyFeaturesMapping> PropertyFeaturesMappingRepository { get; }
        public IRepository<TblPropertyHistory> PropertyHistoryRepository { get; }
        public IRepository<TblPropertyImage> PropertyImageRepository { get; }
        public IRepository<LkpPropertyStatus> PropertyStatusRepository { get; }
        public IRepository<LkpPropertyType> PropertyTypeRepository { get; }
        public IRepository<TblUser> UserRepository { get; }
        public IRepository<LkpUserType> UserTypeRepository { get; }
        public IRepository<TblZone> ZoneRepository { get; }
        public IRepository<LKPPropertyHistoryType> PropertyHistoryTypeRepository { get; }

        public UnitOfWork(AppDBContext context,

            IRepository<TblAppointment> appointmentRepo,
            IRepository<LkpAppointmentStatus> appointmentStatusRepo,
            IRepository<TblBranch> branchRepo,
            IRepository<TblBranchDepartment> branchDeptRepo,
            IRepository<TblCity> cityRepo,
            IRepository<TblClientProfile> clientProfileRepo,
            IRepository<TblDepartment> departmentRepo,
            IRepository<TblDeveloperProfile> developerProfileRepo,
            IRepository<LkpDocumentType> documentTypeRepo,
            IRepository<TblEmployee> employeeRepo,
            IRepository<TblEmployeeClient> employeeClientRepo,
            IRepository<TblFavorite> favoriteRepo,
            IRepository<TblProperty> propertyRepo,
            IRepository<TblPropertyDocument> propertyDocumentRepo,
            IRepository<TblPropertyFeature> propertyFeatureRepo,
            IRepository<TblPropertyFeaturesMapping> propertyFeaturesMappingRepo,
            IRepository<TblPropertyHistory> propertyHistoryRepo,
            IRepository<TblPropertyImage> propertyImageRepo,
            IRepository<LkpPropertyStatus> propertyStatusRepo,
            IRepository<LkpPropertyType> propertyTypeRepo,
            IRepository<TblUser> userRepo,
            IRepository<LkpUserType> userTypeRepo,
            IRepository<TblZone> zoneRepo,
            IRepository<LKPPropertyHistoryType> propertyHistoryTypeRepo
            )
        {
            _context = context;

            AppointmentRepository = appointmentRepo ?? throw new ArgumentNullException(nameof(appointmentRepo));
            AppointmentStatusRepository = appointmentStatusRepo ?? throw new ArgumentNullException(nameof(appointmentStatusRepo));

            BranchRepository = branchRepo ?? throw new ArgumentNullException(nameof(branchRepo));
            BranchDepartmentRepository = branchDeptRepo ?? throw new ArgumentNullException(nameof(branchDeptRepo));

            CityRepository = cityRepo ?? throw new ArgumentNullException(nameof(cityRepo));
            ClientProfileRepository = clientProfileRepo ?? throw new ArgumentNullException(nameof(clientProfileRepo));
            DepartmentRepository = departmentRepo ?? throw new ArgumentNullException(nameof(departmentRepo));
            DeveloperProfileRepository = developerProfileRepo ?? throw new ArgumentNullException(nameof(developerProfileRepo));

            DocumentTypeRepository = documentTypeRepo ?? throw new ArgumentNullException(nameof(documentTypeRepo));

            EmployeeRepository = employeeRepo ?? throw new ArgumentNullException(nameof(employeeRepo));
            EmployeeClientRepository = employeeClientRepo ?? throw new ArgumentNullException(nameof(employeeClientRepo));

            FavoriteRepository = favoriteRepo ?? throw new ArgumentNullException(nameof(favoriteRepo));

            PropertyRepository = propertyRepo ?? throw new ArgumentNullException(nameof(propertyRepo));
            PropertyDocumentRepository = propertyDocumentRepo ?? throw new ArgumentNullException(nameof(propertyDocumentRepo));
            PropertyFeatureRepository = propertyFeatureRepo ?? throw new ArgumentNullException(nameof(propertyFeatureRepo));
            PropertyFeaturesMappingRepository = propertyFeaturesMappingRepo ?? throw new ArgumentNullException(nameof(propertyFeaturesMappingRepo));
            PropertyHistoryRepository = propertyHistoryRepo ?? throw new ArgumentNullException(nameof(propertyHistoryRepo));
            PropertyImageRepository = propertyImageRepo ?? throw new ArgumentNullException(nameof(propertyImageRepo));
            PropertyStatusRepository = propertyStatusRepo ?? throw new ArgumentNullException(nameof(propertyStatusRepo));
            PropertyTypeRepository = propertyTypeRepo ?? throw new ArgumentNullException(nameof(propertyTypeRepo));

            UserRepository = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            UserTypeRepository = userTypeRepo ?? throw new ArgumentNullException(nameof(userTypeRepo));

            ZoneRepository = zoneRepo ?? throw new ArgumentNullException(nameof(zoneRepo));

            PropertyHistoryTypeRepository = propertyHistoryTypeRepo ?? throw new ArgumentNullException(nameof(propertyHistoryTypeRepo));
        }


        public int Complete() => _context.SaveChanges();

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _context.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
