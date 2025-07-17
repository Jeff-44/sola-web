using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}