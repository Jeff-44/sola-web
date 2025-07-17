namespace ApplicationCore.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Client { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public ICollection<ProjectMedia> Media { get; set; }
        public ICollection<Testimonial> Testimonials { get; set; }
    }
}