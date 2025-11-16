using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.ViewModels
{
    public class SinglePropertyViewModel
    {
        public int PropertyID { get; set; }
        public string PropertyCode { get; set; }
        public string Address { get; set; } = "";
        public string CityName { get; set; } = "";
        public string ZoneName { get; set; } = "";
        public decimal Price { get; set; }
        public int Beds { get; set; }
        public int Baths { get; set; }
        public int FloorsNo { get; set; }
        public decimal Area { get; set; }
        public string FirstImage { get; set; } = "default.jpg";
        public string SecondImage { get; set; } = "default.jpg";
        public string ThirdImage { get; set; } = "default.jpg";
        public virtual LkpPropertyType? PropertyType { get; set; }

    }
}
