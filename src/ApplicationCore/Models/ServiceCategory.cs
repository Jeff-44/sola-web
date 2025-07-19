using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}