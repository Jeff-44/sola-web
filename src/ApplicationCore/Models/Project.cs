namespace ApplicationCore.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CompletionDate { get; set; }
        public string Client { get; set; } = string.Empty;

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public ICollection<ProjectMedia> Media { get; set; } = new List<ProjectMedia>();
        public ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
    }
}