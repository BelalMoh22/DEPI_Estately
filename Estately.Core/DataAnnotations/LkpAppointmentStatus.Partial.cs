namespace Estately.Core.Entities
{

    [MetadataType(typeof(LkpAppointmentStatusMetadata))]
    public partial class LkpAppointmentStatus
    {
        public LkpAppointmentStatus()
        {
        }

        private class LkpAppointmentStatusMetadata
        {
            [Required]
            [StringLength(255)]
            public string StatusName { get; set; }

            [StringLength(255)]
            public string? Description { get; set; }

        }
    }
}
