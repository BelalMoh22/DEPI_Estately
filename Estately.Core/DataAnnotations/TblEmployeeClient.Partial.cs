namespace Estately.Core.Entities
{
    [MetadataType(typeof(TblEmployeeClientMetadata))]
    public partial class TblEmployeeClient
    {
        public TblEmployeeClient()
        {
            AssignmentDate ??= DateTime.Now;
        }

        private class TblEmployeeClientMetadata
        {
            [Column(TypeName = "datetime")]
            public DateTime? AssignmentDate { get; set; }

            [ForeignKey("ClientProfileID")]
            [InverseProperty("TblEmployeeClients")]
            public virtual TblClientProfile ClientProfile { get; set; }

            [ForeignKey("EmployeeID")]
            [InverseProperty("TblEmployeeClients")]
            public virtual TblEmployee Employee { get; set; }

            [InverseProperty("EmployeeClient")]
            public virtual ICollection<TblAppointment> TblAppointments { get; set; } = new List<TblAppointment>();
        }
    }
}