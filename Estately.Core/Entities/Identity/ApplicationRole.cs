namespace Estately.Core.Entities.Identity
{
    /// <summary>
    /// Custom Application Role using int as the primary key.
    /// Extend this class if you want to add RoleType, Description,
    /// Permissions, or Audit Fields later.
    /// </summary>
    public class ApplicationRole : IdentityRole<int>
    {
        //public ApplicationRole() : base() { }

        //public ApplicationRole(string roleName) : base(roleName) { }

        // Add any custom fields here (optional)
        // public string? Description { get; set; }
        // public bool IsSystemRole { get; set; }
    }
}
