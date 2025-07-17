using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ProjectMedia
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Url { get; set; }
        public string MediaType { get; set; } // e.g. "Image","Video"

        public Project Project { get; set; }
    }
}
