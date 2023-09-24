using System.ComponentModel.DataAnnotations;

namespace Recipee.Web.Models
{
    public class MeasurementUnit : BaseModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
