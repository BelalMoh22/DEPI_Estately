namespace Estately.Core.Entities;

[MetadataType(typeof(TblUserMetadata))]
public partial class TblUser
{
    public TblUser()
    {
        UserTypeID ??= 1;
        IsEmployee ??= false;
        IsClient ??= true;
        IsDeveloper ??= false;
        CreatedAt ??= DateTime.Now;
        IsDeleted ??= false;
    }

    private class TblUserMetadata
    {
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(500)]
        [Unicode(false)]
        public string PasswordHash { get; set; }
    }
}
