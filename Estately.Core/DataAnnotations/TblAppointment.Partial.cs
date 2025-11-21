namespace Estately.Core.Entities;

[MetadataType(typeof(TblAppointmentMetadata))]
public partial class TblAppointment
{
    public TblAppointment()
    {
    }

    private class TblAppointmentMetadata
    {
        [Column(TypeName = "datetime")]
        public DateTime AppointmentDate { get; set; }

        [Column(TypeName = "text")]
        public string? Notes { get; set; }

        [ForeignKey("EmployeeClientID")]
        [InverseProperty("TblAppointments")]
        public virtual TblEmployeeClient? EmployeeClient { get; set; }

        [ForeignKey("PropertyID")]
        [InverseProperty("TblAppointments")]
        public virtual TblProperty? Property { get; set; }

        [ForeignKey("StatusID")]
        [InverseProperty("TblAppointment")]
        public virtual LkpAppointmentStatus? Status { get; set; }
    }
}
