using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estately.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LkpAppointmentStatus",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LkpAppointmentStatus", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "LkpPropertyStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LkpPropertyStatus", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "LkpPropertyTypes",
                columns: table => new
                {
                    PropertyTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LkpPropertyTypes", x => x.PropertyTypeID);
                });

            migrationBuilder.CreateTable(
                name: "LkpUserType",
                columns: table => new
                {
                    UserTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LkpUserType", x => x.UserTypeID);
                });

            migrationBuilder.CreateTable(
                name: "TblBranches",
                columns: table => new
                {
                    BranchID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
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
                name: "TblDepartments",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblDepartments", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "TblJobTitles",
                columns: table => new
                {
                    JobTitleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblJobTitles", x => x.JobTitleId);
                });

            migrationBuilder.CreateTable(
                name: "TblPropertyFeatures",
                columns: table => new
                {
                    FeatureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    IsAmenity = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPropertyFeatures", x => x.FeatureID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserTypeID = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_LkpUserType_UserTypeID",
                        column: x => x.UserTypeID,
                        principalTable: "LkpUserType",
                        principalColumn: "UserTypeID",
                        onDelete: ReferentialAction.Restrict);
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
                name: "TblBranchDepartments",
                columns: table => new
                {
                    BranchDepartmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchID = table.Column<int>(type: "int", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblBranchDepartments", x => x.BranchDepartmentID);
                    table.ForeignKey(
                        name: "FK_TblBranchDepartments_TblBranches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "TblBranches",
                        principalColumn: "BranchID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblBranchDepartments_TblDepartments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "TblDepartments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblClientProfiles",
                columns: table => new
                {
                    ClientProfileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ProfilePhoto = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblClientProfiles", x => x.ClientProfileID);
                    table.ForeignKey(
                        name: "FK_TblClientProfiles_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblDeveloperProfiles",
                columns: table => new
                {
                    DeveloperProfileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    DeveloperTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeveloperName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WebsiteURL = table.Column<string>(type: "varchar(800)", unicode: false, maxLength: 800, nullable: true),
                    PortofolioPhoto = table.Column<string>(type: "varchar(800)", unicode: false, maxLength: 800, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblDeveloperProfiles", x => x.DeveloperProfileID);
                    table.ForeignKey(
                        name: "FK_TblDeveloperProfiles_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblEmployees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    BranchDepartmentId = table.Column<int>(type: "int", nullable: true),
                    JobTitleId = table.Column<int>(type: "int", nullable: false),
                    ReportsTo = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nationalid = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    ProfilePhoto = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblEmployees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_TblEmployees_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblEmployees_TblBranchDepartments_BranchDepartmentId",
                        column: x => x.BranchDepartmentId,
                        principalTable: "TblBranchDepartments",
                        principalColumn: "BranchDepartmentID");
                    table.ForeignKey(
                        name: "FK_TblEmployees_TblEmployees_ReportsTo",
                        column: x => x.ReportsTo,
                        principalTable: "TblEmployees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_TblEmployees_TblJobTitles_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "TblJobTitles",
                        principalColumn: "JobTitleId",
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
                    AssignmentDate = table.Column<DateTime>(type: "datetime", nullable: true)
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
                name: "TblProperties",
                columns: table => new
                {
                    PropertyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: true),
                    DeveloperProfileID = table.Column<int>(type: "int", nullable: true),
                    PropertyTypeID = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    ZoneID = table.Column<int>(type: "int", nullable: false),
                    YearBuilt = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FloorNo = table.Column<int>(type: "int", nullable: true),
                    BedsNo = table.Column<int>(type: "int", nullable: false),
                    BathsNo = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Area = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ListingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExpectedRentPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    PropertyCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblProperties", x => x.PropertyID);
                    table.ForeignKey(
                        name: "FK_TblProperties_LkpPropertyStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "LkpPropertyStatus",
                        principalColumn: "StatusID");
                    table.ForeignKey(
                        name: "FK_TblProperties_LkpPropertyTypes_PropertyTypeID",
                        column: x => x.PropertyTypeID,
                        principalTable: "LkpPropertyTypes",
                        principalColumn: "PropertyTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblProperties_TblDeveloperProfiles_DeveloperProfileID",
                        column: x => x.DeveloperProfileID,
                        principalTable: "TblDeveloperProfiles",
                        principalColumn: "DeveloperProfileID");
                    table.ForeignKey(
                        name: "FK_TblProperties_TblEmployees_AgentId",
                        column: x => x.AgentId,
                        principalTable: "TblEmployees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_TblProperties_TblZones_ZoneID",
                        column: x => x.ZoneID,
                        principalTable: "TblZones",
                        principalColumn: "ZoneID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblAppointments",
                columns: table => new
                {
                    AppointmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    PropertyID = table.Column<int>(type: "int", nullable: false),
                    EmployeeClientID = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblAppointments", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK_TblAppointments_LkpAppointmentStatus_StatusID",
                        column: x => x.StatusID,
                        principalTable: "LkpAppointmentStatus",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "TblFavorites",
                columns: table => new
                {
                    FavoriteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientProfileID = table.Column<int>(type: "int", nullable: false),
                    PropertyID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
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
                    UploadedDate = table.Column<DateTime>(type: "datetime", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserTypeID",
                table: "AspNetUsers",
                column: "UserTypeID");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TblAppointments",
                table: "TblAppointments",
                column: "StatusID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblAppointments_EmployeeClientID",
                table: "TblAppointments",
                column: "EmployeeClientID");

            migrationBuilder.CreateIndex(
                name: "IX_TblAppointments_PropertyID",
                table: "TblAppointments",
                column: "PropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_TblBranchDepartments_BranchID",
                table: "TblBranchDepartments",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_TblBranchDepartments_DepartmentID",
                table: "TblBranchDepartments",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_TblClientProfiles",
                table: "TblClientProfiles",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblDeveloperProfiles",
                table: "TblDeveloperProfiles",
                column: "UserID",
                unique: true);

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
                name: "IX_TblEmployees_BranchDepartmentId",
                table: "TblEmployees",
                column: "BranchDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TblEmployees_JobTitleId",
                table: "TblEmployees",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_TblEmployees_ReportsTo",
                table: "TblEmployees",
                column: "ReportsTo");

            migrationBuilder.CreateIndex(
                name: "IX_TblFavorites_ClientProfileID",
                table: "TblFavorites",
                column: "ClientProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_TblFavorites_PropertyID",
                table: "TblFavorites",
                column: "PropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_TblProperties_1",
                table: "TblProperties",
                column: "PropertyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblProperties_AgentId",
                table: "TblProperties",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_TblProperties_DeveloperProfileID",
                table: "TblProperties",
                column: "DeveloperProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_TblProperties_PropertyTypeID",
                table: "TblProperties",
                column: "PropertyTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TblProperties_StatusId",
                table: "TblProperties",
                column: "StatusId");

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
                name: "IX_TblZones_CityID",
                table: "TblZones",
                column: "CityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "TblAppointments");

            migrationBuilder.DropTable(
                name: "TblFavorites");

            migrationBuilder.DropTable(
                name: "TblPropertyFeaturesMapping");

            migrationBuilder.DropTable(
                name: "TblPropertyImages");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "LkpAppointmentStatus");

            migrationBuilder.DropTable(
                name: "TblEmployeeClients");

            migrationBuilder.DropTable(
                name: "TblPropertyFeatures");

            migrationBuilder.DropTable(
                name: "TblProperties");

            migrationBuilder.DropTable(
                name: "TblClientProfiles");

            migrationBuilder.DropTable(
                name: "LkpPropertyStatus");

            migrationBuilder.DropTable(
                name: "LkpPropertyTypes");

            migrationBuilder.DropTable(
                name: "TblDeveloperProfiles");

            migrationBuilder.DropTable(
                name: "TblEmployees");

            migrationBuilder.DropTable(
                name: "TblZones");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TblBranchDepartments");

            migrationBuilder.DropTable(
                name: "TblJobTitles");

            migrationBuilder.DropTable(
                name: "TblCities");

            migrationBuilder.DropTable(
                name: "LkpUserType");

            migrationBuilder.DropTable(
                name: "TblBranches");

            migrationBuilder.DropTable(
                name: "TblDepartments");
        }
    }
}
