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
        public string PropertyCode { get; set; } = "";
        public string Description { get; set; } = "";
        public string Address { get; set; } = "";
        public string PropertyTypeName { get; set; } = "";
        public string PropertyStatusName { get; set; } = "";
        public string CityName { get; set; } = "";
        public string ZoneName { get; set; } = "";
        public decimal Price { get; set; }
        public int Beds { get; set; }
        public int Baths { get; set; }
        public int? FloorNo { get; set; } = null;
        public decimal Area { get; set; }
        public decimal? ExpectedRent { get; set; } = 0m;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        // Unlimited images
        public List<string> Images { get; set; } = new();

        // Property features
        public List<PropertyFeatureViewModel> Features { get; set; } = new();

        // Agent contact info for WhatsApp
        public string? AgentPhone { get; set; }
        public string? AgentName { get; set; }

        // Optional relations
        public virtual LkpPropertyType? PropertyType { get; set; }
        public virtual LkpPropertyStatus? PropertyStatus { get; set; }
        public virtual TblCity? City { get; set; }
        public virtual TblZone? Zone { get; set; }
    }
}