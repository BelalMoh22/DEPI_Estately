using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.ViewModels
{
    public class PropertiesViewModel
    {
        public int PropertyID { get; set; }
        public string Address { get; set; } = "";
        public string CityName { get; set; } = "";
        public string ZoneName { get; set; } = "";
        public decimal Price { get; set; }
        public int Beds { get; set; }
        public int Baths { get; set; }

        public string FirstImage { get; set; } = "default.jpg";
    }

    public class PropertiesListViewModel : BaseViewModel
    {
        public List<PropertiesViewModel> Properties { get; set; } = new();
    }
}
