namespace Estately.Core.Entities;

[MetadataType(typeof(TblAppointmentMetadata))]
public partial class TblAppointment
{
    public TblAppointment()
    {
        // No special defaults beyond EF mapping
    }

    private class TblAppointmentMetadata
    {
        [Column(TypeName = "datetime")]
        public DateTime AppointmentDate { get; set; }

        [Column(TypeName = "text")]
        public string? Notes { get; set; }
    }
}
