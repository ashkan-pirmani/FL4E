using System.ComponentModel.DataAnnotations;

namespace FL_Server.Models
{
    public class StudyCatalogue
    {

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string StudyLead { get; set; }

        [Required]
        public bool? Status { get; set; }
        [Required]
        public DateTime StartDate { get; set; }


    }
}
