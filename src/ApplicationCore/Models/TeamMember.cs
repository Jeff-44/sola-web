using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class TeamMember
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;

        public string PhotoUrl { get; set; } = string.Empty;
    }
}
