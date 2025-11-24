namespace Estately.Core.Entities;

[Table("LkpAppointmentStatus")]
public partial class LkpAppointmentStatus
{
    [Key]
    public int StatusId { get; set; }

    [Required]
    [StringLength(255)]
    public string StatusName { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    [InverseProperty("Status")]
    public virtual TblAppointment TblAppointment { get; set; }
}