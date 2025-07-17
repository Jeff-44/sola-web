using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }

        public int? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
