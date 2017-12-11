using System.ComponentModel.DataAnnotations;

using static BookHeaven.Data.Infrastructure.Constants.LocationDataConstants;

namespace BookHeaven.Data.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        [MinLength(CountryNameMinLength)]
        [MaxLength(CountryNameMaxLength)]
        public string Country { get; set; }

        [Required]
        [MinLength(CityNameMinLength)]
        [MaxLength(CityNameMaxLength)]
        public string City { get; set; }

        public int SiteVisits { get; set; }
    }
}