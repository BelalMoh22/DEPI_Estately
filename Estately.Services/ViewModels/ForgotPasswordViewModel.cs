using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress] public string Email { get; set; } = "";
    }
}
