using Database.Entities;

namespace Repositories.DropDownViewModel
{
    public class ZoneDropDownViewMode
    {
        public ZoneDropDownViewMode()
        {
            Zones = new List<ApplicationUser>();
        }
        public List<ApplicationUser> Zones { get; set; }
    }
}
