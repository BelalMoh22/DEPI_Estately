using System.ComponentModel.DataAnnotations;

namespace Estately.WebApp.ViewModels
{
    public class LkpUserTypeViewModel
    {
        public int UserTypeID { get; set; }

        [Required(ErrorMessage = "User Type Name is required")]
        [StringLength(255)]
        [Display(Name = "User Type Name")]
        public string UserTypeName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }

    public class LkpPropertyTypeViewModel
    {
        public int PropertyTypeID { get; set; }

        [Required(ErrorMessage = "Type Name is required")]
        [StringLength(255)]
        [Display(Name = "Type Name")]
        public string TypeName { get; set; } = string.Empty;
    }

    public class LkpPropertyStatusViewModel
    {
        public int StatusID { get; set; }

        [Required(ErrorMessage = "Status Name is required")]
        [StringLength(255)]
        [Display(Name = "Status Name")]
        public string StatusName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }
    }

    public class LkpAppointmentStatusViewModel
    {
        public int StatusId { get; set; }

        [Required(ErrorMessage = "Status Name is required")]
        [StringLength(255)]
        [Display(Name = "Status Name")]
        public string StatusName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }
    }

    public class LkpDocumentTypeViewModel
    {
        public int DocumentTypeID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }
    }

    public class LkpPropertyHistoryTypeViewModel
    {
        public int HistoryTypeID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }
    }
}

