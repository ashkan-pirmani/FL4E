using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FL_Server.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        /// <summary>
        /// 0 => EndUser
        /// 1 => DataPartner
        /// 2 => DataScientist
        /// 3 => AdminUser
        /// </summary>

        [Required]
        [DefaultValue("0")]
        [Display(Name = "User Role")]
        public int UserRole { get; set; }

        [Required]
        [Display(Name = "Affilation")]
        public string Affilation { get; set; }
        [Required]
        [DefaultValue(false)]
        public Boolean IsActivated { get; set; }
    }
}
