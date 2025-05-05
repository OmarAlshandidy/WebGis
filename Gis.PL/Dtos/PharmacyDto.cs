using System.ComponentModel.DataAnnotations;

namespace Gis.PL.Dtos
{
    public class PharmacyDto
    {
        public int Objectid { get; set; }

        [Required(ErrorMessage = "Name Is Requird !!")]
        public string Name { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage = "Latitude  Is Requird !!")]
        public decimal Lat { get; set; }
        [Required(ErrorMessage = "Longitude  Is Requird !!")]
        public decimal Lon { get; set; }
    }
}
