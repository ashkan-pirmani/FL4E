using System.ComponentModel.DataAnnotations;

namespace FL_Clients
{
    public class HomeViewModel
    {
        [Required]
        public string IP { get; set; }

        public string ClientID { get; set; }

    }
}
