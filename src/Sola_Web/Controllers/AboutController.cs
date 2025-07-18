using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Sola_Web.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            var team = new List<TeamMember>
            {
                new TeamMember
                {
                    Id = 1,
                    Name = "Joseph Jeff FORESTAL",
                    Role = "Fondateur",
                    Bio = "Génie Electronique à la Faculté des Sciences - UEH",
                    PhotoUrl = Url.Content("~/images/team/member1.jpg")
                },
                new TeamMember
                {
                    Id = 2,
                    Name = "Evens CLENAT",
                    Role = "Technicien en Installation Systeme Solaire de la Canado Technique",
                    Bio = "Spécialiste en conception de solutions solaires innovantes.",
                    PhotoUrl = Url.Content("~/images/team/member2.jpg")
                },
                new TeamMember
                {
                    Id = 3,
                    Name = "Rickenson Jeune",
                    Role = "Designer",
                    Bio = "Assure le design et les maquettes de notre plateforme.",
                    PhotoUrl = Url.Content("~/images/team/member3.jpg")
                }
            };
            return View(team);
        }
    }
}

