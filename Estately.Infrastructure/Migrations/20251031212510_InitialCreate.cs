using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estately.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblBranches",
                columns: table => new
                {
                    BranchID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblBranches", x => x.BranchID);
                });

            migrationBuilder.CreateTable(
                name: "TblCities",
                columns: table => new
                {
                    CityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblCities", x => x.CityID);
                });

            migrationBuilder.CreateTable(
                name: "TblClientProfiles",
                columns: table => new
                {
                    ClientProfileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblClientProfiles", x => x.ClientProfileID);
                });

            migrationBuilder.CreateTable(
                name: "TblDeveloperProfiles",
                columns: table => new
                {
                    DeveloperProfileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    DeveloperName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WebsiteURL = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    LogoURL = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblDeveloperProfiles", x => x.DeveloperProfileID);
                });

            migrationBuilder.CreateTable(
                name: "TblPropertyFeatures",
                columns: table => new
                {
                    FeatureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPropertyFeatures", x => x.FeatureID);
                });

            migrationBuilder.CreateTable(
                name: "TblPropertyTypes",
                columns: table => new
                {
                    PropertyTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPropertyTypes", x => x.PropertyTypeID);
                });

            migrationBuilder.CreateTable(
                name: "TblUserType",
                columns: table => new
                {
                    UserTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUserType", x => x.UserTypeID);
                });

            migrationBuilder.CreateTable(
                name: "TblDepartments",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    DepartmentName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblDepartments", x => x.DepartmentID);
                    table.ForeignKey(
                        name: "FK_TblDepartments_TblBranches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "TblBranches",
                        principalColumn: "BranchID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblZones",
                columns: table => new
                {
                    ZoneID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    ZoneName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblZones", x => x.ZoneID);
                    table.ForeignKey(
                        name: "FK_TblZones_TblCities_CityID",
                        column: x => x.CityID,
                        principalTable: "TblCities",
                        principalColumn: "CityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblUsers",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeID = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    IsEmployee = table.Column<bool>(type: "bit", nullable: false),
                    ISClient = table.Column<bool>(type: "bit", nullable: false),
                    IsDeveloper = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUsers", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_TblUsers_TblUserType_UserTypeID",
                        column: x => x.UserTypeID,
                        principalTable: "TblUserType",
                        principalColumn: "UserTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblProperties",
                columns: table => new
                {
                    PropertyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeveloperProfileID = table.Column<int>(type: "int", nullable: false),
                    PropertyTypeID = table.Column<int>(type: "int", nullable: false),
                    ZoneID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsSold = table.Column<bool>(type: "bit", nullable: false),
                    ListingDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblProperties", x => x.PropertyID);
                    table.ForeignKey(
                        name: "FK_TblProperties_TblDeveloperProfiles_DeveloperProfileID",
                        column: x => x.DeveloperProfileID,
                        principalTable: "TblDeveloperProfiles",
                        principalColumn: "DeveloperProfileID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblProperties_TblPropertyTypes_PropertyTypeID",
                        column: x => x.PropertyTypeID,
                        principalTable: "TblPropertyTypes",
                        principalColumn: "PropertyTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblProperties_TblZones_ZoneID",
                        column: x => x.ZoneID,
                        principalTable: "TblZones",
                        principalColumn: "ZoneID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblEmployees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblEmployees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_TblEmployees_TblDepartments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "TblDepartments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblEmployees_TblUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "TblUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblFavorites",
                columns: table => new
                {
                    FavoriteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientProfileID = table.Column<int>(type: "int", nullable: false),
                    PropertyID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblFavorites", x => x.FavoriteID);
                    table.ForeignKey(
                        name: "FK_TblFavorites_TblClientProfiles_ClientProfileID",
                        column: x => x.ClientProfileID,
                        principalTable: "TblClientProfiles",
                        principalColumn: "ClientProfileID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblFavorites_TblProperties_PropertyID",
                        column: x => x.PropertyID,
                        principalTable: "TblProperties",
                        principalColumn: "PropertyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblPropertyFeaturesMapping",
                columns: table => new
                {
                    PropertyID = table.Column<int>(type: "int", nullable: false),
                    FeatureID = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPropertyFeaturesMapping", x => new { x.PropertyID, x.FeatureID });
                    table.ForeignKey(
                        name: "FK_TblPropertyFeaturesMapping_TblProperties_PropertyID",
                        column: x => x.PropertyID,
                        principalTable: "TblProperties",
                        principalColumn: "PropertyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblPropertyFeaturesMapping_TblPropertyFeatures_FeatureID",
                        column: x => x.FeatureID,
                        principalTable: "TblPropertyFeatures",
                        principalColumn: "FeatureID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblPropertyImages",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyID = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPropertyImages", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_TblPropertyImages_TblProperties_PropertyID",
                        column: x => x.PropertyID,
                        principalTable: "TblProperties",
                        principalColumn: "PropertyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblEmployeeClients",
                columns: table => new
                {
                    EmployeeClientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    ClientProfileID = table.Column<int>(type: "int", nullable: false),
                    AssignmentDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblEmployeeClients", x => x.EmployeeClientID);
                    table.ForeignKey(
                        name: "FK_TblEmployeeClients_TblClientProfiles_ClientProfileID",
                        column: x => x.ClientProfileID,
                        principalTable: "TblClientProfiles",
                        principalColumn: "ClientProfileID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblEmployeeClients_TblEmployees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "TblEmployees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblAppointments",
                columns: table => new
                {
                    AppointmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyID = table.Column<int>(type: "int", nullable: false),
                    EmployeeClientID = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblAppointments", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK_TblAppointments_TblEmployeeClients_EmployeeClientID",
                        column: x => x.EmployeeClientID,
                        principalTable: "TblEmployeeClients",
                        principalColumn: "EmployeeClientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblAppointments_TblProperties_PropertyID",
                        column: x => x.PropertyID,
                        principalTable: "TblProperties",
                        principalColumn: "PropertyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblTransactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyID = table.Column<int>(type: "int", nullable: false),
                    EmployeeClientID = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FileURL = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblTransactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_TblTransactions_TblEmployeeClients_EmployeeClientID",
                        column: x => x.EmployeeClientID,
                        principalTable: "TblEmployeeClients",
                        principalColumn: "EmployeeClientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblTransactions_TblProperties_PropertyID",
                        column: x => x.PropertyID,
                        principalTable: "TblProperties",
                        principalColumn: "PropertyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblCommission",
                columns: table => new
                {
                    CommissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblCommission", x => x.CommissionID);
                    table.ForeignKey(
                        name: "FK_TblCommission_TblTransactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "TblTransactions",
                        principalColumn: "TransactionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblPayments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionID = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPayments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_TblPayments_TblTransactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "TblTransactions",
                        principalColumn: "TransactionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblAppointments_EmployeeClientID",
                table: "TblAppointments",
                column: "EmployeeClientID");

            migrationBuilder.CreateIndex(
                name: "IX_TblAppointments_PropertyID",
                table: "TblAppointments",
                column: "PropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_TblCommission_TransactionID",
                table: "TblCommission",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_TblDepartments_BranchId",
                table: "TblDepartments",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TblEmployeeClients_ClientProfileID",
                table: "TblEmployeeClients",
                column: "ClientProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_TblEmployeeClients_EmployeeID",
                table: "TblEmployeeClients",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_TblEmployees",
                table: "TblEmployees",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblEmployees_DepartmentID",
                table: "TblEmployees",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_TblFavorites_ClientProfileID",
                table: "TblFavorites",
                column: "ClientProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_TblFavorites_PropertyID",
                table: "TblFavorites",
                column: "PropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_TblPayments_TransactionID",
                table: "TblPayments",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_TblProperties_DeveloperProfileID",
                table: "TblProperties",
                column: "DeveloperProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_TblProperties_PropertyTypeID",
                table: "TblProperties",
                column: "PropertyTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TblProperties_ZoneID",
                table: "TblProperties",
                column: "ZoneID");

            migrationBuilder.CreateIndex(
                name: "IX_TblPropertyFeaturesMapping_FeatureID",
                table: "TblPropertyFeaturesMapping",
                column: "FeatureID");

            migrationBuilder.CreateIndex(
                name: "IX_TblPropertyImages_PropertyID",
                table: "TblPropertyImages",
                column: "PropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_TblTransactions_EmployeeClientID",
                table: "TblTransactions",
                column: "EmployeeClientID");

            migrationBuilder.CreateIndex(
                name: "IX_TblTransactions_PropertyID",
                table: "TblTransactions",
                column: "PropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_TblUsers_UserTypeID",
                table: "TblUsers",
                column: "UserTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TblZones_CityID",
                table: "TblZones",
                column: "CityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblAppointments");

            migrationBuilder.DropTable(
                name: "TblCommission");

            migrationBuilder.DropTable(
                name: "TblFavorites");

            migrationBuilder.DropTable(
                name: "TblPayments");

            migrationBuilder.DropTable(
                name: "TblPropertyFeaturesMapping");

            migrationBuilder.DropTable(
                name: "TblPropertyImages");

            migrationBuilder.DropTable(
                name: "TblTransactions");

            migrationBuilder.DropTable(
                name: "TblPropertyFeatures");

            migrationBuilder.DropTable(
                name: "TblEmployeeClients");

            migrationBuilder.DropTable(
                name: "TblProperties");

            migrationBuilder.DropTable(
                name: "TblClientProfiles");

            migrationBuilder.DropTable(
                name: "TblEmployees");

            migrationBuilder.DropTable(
                name: "TblDeveloperProfiles");

            migrationBuilder.DropTable(
                name: "TblPropertyTypes");

            migrationBuilder.DropTable(
                name: "TblZones");

            migrationBuilder.DropTable(
                name: "TblDepartments");

            migrationBuilder.DropTable(
                name: "TblUsers");

            migrationBuilder.DropTable(
                name: "TblCities");

            migrationBuilder.DropTable(
                name: "TblBranches");

            migrationBuilder.DropTable(
                name: "TblUserType");
        }
    }
}
