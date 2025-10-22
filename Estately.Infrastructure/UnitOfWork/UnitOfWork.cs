using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estately.Core.Interfaces;
using Estately.Infrastructure.Data;

namespace Estately.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;

        public IRepository<TblAppointment> AppointmentRepository { get; }
        public IRepository<TblBranch> BranchRepository { get; }
        public IRepository<TblCity> CityRepository { get; }
        public IRepository<TblClientProfile> ClientProfileRepository { get; }
        public IRepository<TblCommission> CommissionRepository { get; }
        public IRepository<TblDepartment> DepartmentRepository { get; }
        public IRepository<TblDeveloperProfile> DeveloperProfileRepository { get; }
        public IRepository<TblEmployee> EmployeeRepository { get; }
        public IRepository<TblEmployeeClient> EmployeeClientRepository { get; }
        public IRepository<TblFavorite> FavoriteRepository { get; }
        public IRepository<TblPayment> PaymentRepository { get; }
        public IRepository<TblProperty> PropertyRepository { get; }
        public IRepository<TblPropertyFeature> PropertyFeatureRepository { get; }
        public IRepository<TblPropertyFeaturesMapping> PropertyFeaturesMappingRepository { get; }
        public IRepository<TblPropertyImage> PropertyImageRepository { get; }
        public IRepository<TblPropertyType> PropertyTypeRepository { get; }
        public IRepository<TblTransaction> TransactionRepository { get; }
        public IRepository<TblUser> UserRepository { get; }
        public IRepository<TblUserType> UserTypeRepository { get; }
        public IRepository<TblZone> ZoneRepository { get; }

        public UnitOfWork(AppDBContext context,
                          IRepository<TblAppointment> appointmentRepository,
                          IRepository<TblBranch> branchRepository,
                          IRepository<TblCity> cityRepository,
                          IRepository<TblClientProfile> clientProfileRepository,
                          IRepository<TblCommission> commissionRepository,
                          IRepository<TblDepartment> departmentRepository,
                          IRepository<TblDeveloperProfile> developerProfileRepository,
                          IRepository<TblEmployee> employeeRepository,
                          IRepository<TblEmployeeClient> employeeClientRepository,
                          IRepository<TblFavorite> favoriteRepository,
                          IRepository<TblPayment> paymentRepository,
                          IRepository<TblProperty> propertyRepository,
                          IRepository<TblPropertyFeature> propertyFeatureRepository,
                          IRepository<TblPropertyFeaturesMapping> propertyFeaturesMappingRepository,
                          IRepository<TblPropertyImage> propertyImageRepository,
                          IRepository<TblPropertyType> propertyTypeRepository,
                          IRepository<TblTransaction> transactionRepository,
                          IRepository<TblUser> userRepository,
                          IRepository<TblUserType> userTypeRepository,
                          IRepository<TblZone> zoneRepository)
        {
            _context = context;
            AppointmentRepository = appointmentRepository?? throw new ArgumentNullException(nameof(appointmentRepository));
            BranchRepository = branchRepository?? throw new ArgumentNullException(nameof(branchRepository));
            CityRepository = cityRepository?? throw new ArgumentNullException(nameof(cityRepository));
            ClientProfileRepository = clientProfileRepository?? throw new ArgumentNullException(nameof(clientProfileRepository));
            CommissionRepository = commissionRepository?? throw new ArgumentNullException(nameof(commissionRepository));
            DepartmentRepository = departmentRepository?? throw new ArgumentNullException(nameof(departmentRepository));
            DeveloperProfileRepository = developerProfileRepository?? throw new ArgumentNullException(nameof(developerProfileRepository));
            EmployeeRepository = employeeRepository?? throw new ArgumentNullException(nameof(employeeRepository));
            EmployeeClientRepository = employeeClientRepository?? throw new ArgumentNullException(nameof(employeeClientRepository));
            FavoriteRepository = favoriteRepository?? throw new ArgumentNullException(nameof(favoriteRepository));
            PaymentRepository = paymentRepository?? throw new ArgumentNullException(nameof(paymentRepository));
            PropertyRepository = propertyRepository?? throw new ArgumentNullException(nameof(propertyRepository));
            PropertyFeatureRepository = propertyFeatureRepository?? throw new ArgumentNullException(nameof(propertyFeatureRepository));
            PropertyFeaturesMappingRepository = propertyFeaturesMappingRepository?? throw new ArgumentNullException(nameof(propertyFeaturesMappingRepository));
            PropertyImageRepository = propertyImageRepository?? throw new ArgumentNullException(nameof(propertyImageRepository));
            PropertyTypeRepository = propertyTypeRepository?? throw new ArgumentNullException(nameof(propertyTypeRepository));
            TransactionRepository = transactionRepository?? throw new ArgumentNullException(nameof(transactionRepository));
            UserRepository = userRepository?? throw new ArgumentNullException(nameof(userRepository));
            UserTypeRepository = userTypeRepository?? throw new ArgumentNullException(nameof(userTypeRepository));
            ZoneRepository = zoneRepository?? throw new ArgumentNullException(nameof(zoneRepository));
        }

        public int Complete()
        {
            var rows = _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return rows;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
