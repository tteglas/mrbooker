using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MRBooker.Data.Models.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
